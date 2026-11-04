import test from "node:test";
import assert from "node:assert/strict";
import { randomUUID } from "node:crypto";
import { NativeHttpBridge } from "./native-http";
import { createServer } from "./server";
const request={raidId:randomUUID().replaceAll("-",""),session:{steamId64:"76561198000000001",connectionId:"c",pawnGeneration:"p",mapGeneration:"m"},amountCents:100};
test("native queue leases once, rejects arbitrary/stale replies and resolves only current UUID",async()=>{
 const n=new NativeHttpBridge();try{
 const pending=n.validateAndFreeze("key","cashin",request);
 assert.throws(()=>n.next({command:"give_money"}));
 const job=n.next({}).job!;assert.equal(job.method,"freeze");assert.deepEqual(job.request,request);
 assert.equal(n.next({}).job,null);
 assert.throws(()=>n.reply({id:randomUUID(),accepted:true}));
 assert.throws(()=>n.reply({id:job.id,accepted:true,receipt:"fake"}));
 assert.deepEqual(n.reply({id:job.id,accepted:true}),{accepted:true});assert.equal(await pending,true);
 assert.throws(()=>n.reply({id:job.id,accepted:true}));assert.equal(n.next({}).job,null);
 }finally{n.close();}
});
test("bounded timeout and close fail closed, late callback cannot affect a new lease",async()=>{
 const n=new NativeHttpBridge(10,1);try{
 const first=n.validateAndFreeze("key","cashin",request),job=n.next({}).job!;
 assert.equal(await n.validateAndFreeze("other","cashin",request),false);
 assert.equal(await first,false);
 const second=n.validateAndFreeze("key","cashin",request),next=n.next({}).job!;
 assert.notEqual(next.id,job.id);assert.throws(()=>n.reply({id:job.id,accepted:true}));
 n.close();assert.equal(await second,false);assert.throws(()=>n.reply({id:next.id,accepted:true}));
 }finally{n.close();}
});
test("finalize callback contains only structured durable proof and immutable request clone",async()=>{
 const n=new NativeHttpBridge();try{
 const proof={operationKey:"key",operation:"cashin" as const,requestFingerprint:"a".repeat(64),amountCents:100,transactionId:randomUUID(),appUserId:randomUUID()};
 const pending=n.finalize(proof,request);proof.amountCents=500;
 const job=n.next({}).job!;assert.equal(job.proof?.amountCents,100);assert.equal(job.method,"finalize");
 n.reply({id:job.id,accepted:false});assert.equal(await pending,false);
 }finally{n.close();}
});
test("native polling routes have identical private auth, Origin rejection and no command surface",async()=>{
 const n=new NativeHttpBridge(),token="n".repeat(40);
 // Route-only test: no ledger/money call is possible in this scenario.
 const app=createServer(token,new Map(),{async create(){return {};},async poll(){return {};}},
 {ledger:{} as never,api:{} as never,native:n});
 try{
 const headers={authorization:"Bearer "+token};
 assert.equal((await app.inject({method:"POST",url:"/native/next",payload:{}})).statusCode,401);
 assert.equal((await app.inject({method:"POST",url:"/native/next",headers:{...headers,origin:""},payload:{}})).statusCode,401);
 const pending=n.validateAndFreeze("key","cashin",request);
 const next=await app.inject({method:"POST",url:"/native/next",headers,payload:{}});
 const id=next.json().job.id;assert.equal(next.statusCode,200);
 assert.equal((await app.inject({method:"POST",url:"/native/reply",payload:{id,accepted:true}})).statusCode,401);
 assert.equal((await app.inject({method:"POST",url:"/native/reply",headers,payload:{id,accepted:true,command:"give"}})).statusCode,400);
 assert.equal((await app.inject({method:"POST",url:"/native/reply",headers,payload:{id,accepted:true}})).statusCode,200);
 assert.equal(await pending,true);
 assert.equal((await app.inject({method:"POST",url:"/native/reply",headers,payload:{id,accepted:true}})).statusCode,400);
 assert.equal((await app.inject({method:"POST",url:"/native/command",headers,payload:{}})).statusCode,404);
 }finally{await app.close();}
});
