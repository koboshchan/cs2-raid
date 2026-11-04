import test from "node:test";
import assert from "node:assert/strict";
import { randomUUID } from "node:crypto";
import { createServer } from "./server";
import { parseCatalog } from "./catalog";
import { LinkStore } from "./links";
const steam="76561198000000001", token="t".repeat(40);
test("HTTP authenticates, rejects Origin, and never invokes money movement",async()=>{
 let calls=0;
 const app=createServer(token,parseCatalog([{id:"ak",entity:"weapon_ak47",kind:"weapon",priceDollars:2700}]),{
 async create(){calls++;return {state:"pending"};},async poll(){calls++;return {state:"approved"};}
 });
 try {
 assert.equal((await app.inject({url:"/health"})).statusCode,401);
 const headers={authorization:"Bearer "+token};
 assert.equal((await app.inject({url:"/health",headers:{...headers,origin:"https://evil.example"}})).statusCode,401);
 for(const url of ["/purchase","/buy","/debit","/sell","/sellback","/cashin","/cashout","/extract","/payout","/settlement"])
 assert.equal((await app.inject({url,method:"POST",headers,payload:{steamId64:steam}})).statusCode,503);
 assert.equal(calls,0);
 assert.equal((await app.inject({url:"/link",method:"POST",headers,payload:{steamId64:steam}})).statusCode,200);
 assert.equal(calls,1);
 assert.equal((await app.inject({url:"/catalog",headers})).json().items[0].defaultLimitBlocked,true);
 }finally{await app.close();}
});
test("durable link hides token, throttles poll and checks debit scope",async(t)=>{
 let scopes:string[]=[];
 const link={request_id:randomUUID(),device_token:"s".repeat(43),user_code:"four test words here",verification_uri:"https://example.test/link",expires_at:new Date(Date.now()+600000).toISOString(),interval_seconds:5};
 const store=new LinkStore(":memory:","raid",{
 async createLink(){return link;},
 async pollLink(){return {state:"approved" as const,app_user_id:randomUUID(),grant_id:randomUUID(),granted_scopes:scopes};}
 });
 try {
 assert.equal("device_token" in await store.create(steam),false);
 await assert.rejects(store.poll(steam),/slow_down/);
 const now=Date.now(); t.mock.method(Date,"now",()=>now+6000);
 await assert.rejects(store.poll(steam),/insufficient_scope/);
 scopes=["balance:debit"]; t.mock.method(Date,"now",()=>now+12000);
 assert.equal((await store.poll(steam)).state,"approved");
 }finally{store.close();}
});
test("catalog rejects fractional cents and duplicate IDs",()=>{
 assert.throws(()=>parseCatalog([{id:"x",entity:"weapon_x",kind:"weapon",priceDollars:1.001}]));
 const item={id:"x",entity:"weapon_x",kind:"weapon",priceDollars:1};
 assert.throws(()=>parseCatalog([item,item]));
});
