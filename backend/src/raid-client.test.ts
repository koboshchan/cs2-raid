import test from "node:test";
import assert from "node:assert/strict";
import { randomUUID } from "node:crypto";
import { SsgClient, ApiError } from "./client";
test("raid HTTP debit and mint use exact cents, stable key and confidential headers",async()=>{
 const calls:{url:string;init:RequestInit}[]=[];
 const client=new SsgClient("https://ssg.example","app-test","ac_"+randomUUID()+"."+"s".repeat(43),
 (async(url:unknown,init:RequestInit)=>{
 calls.push({url:String(url),init});const body=JSON.parse(String(init.body));
 return Response.json({transaction_id:randomUUID(),app_user_id:appUserId,grant_id:grantId,status:"committed",
 direction:String(url).endsWith("/credits")?"credit":"debit",amount_cents:body.amount_cents,
 external_reference:body.external_reference,...(body.credit_reason?{credit_reason:body.credit_reason}:{})},{status:201});
 }) as typeof fetch);
 const appUserId=randomUUID(),grantId=randomUUID();
 for(const operation of ["cashin","extract"] as const){
 const key="cs2.raid."+operation+"."+randomUUID();
 const receipt=await client.moveRaidMoney({key,appUserId,amountCents:1500000,operation});
 assert.equal(receipt.grant_id,grantId);
 const c=calls.at(-1)!,h=c.init.headers as Record<string,string>,body=JSON.parse(String(c.init.body));
 assert.equal(h["Idempotency-Key"],key);assert.equal(h["X-Client-Id"],"app-test");
 assert.ok(h.Authorization.startsWith("Bearer ac_"));assert.ok(!c.url.includes("Bearer"));
 assert.equal(c.url,"https://ssg.example/api/v1/users/"+appUserId+(operation==="cashin"?"/debits":"/credits"));
 assert.equal(body.amount_cents,1500000);assert.equal(body.external_reference,key);
 assert.equal(body.credit_reason,operation==="extract"?"mint":undefined);
 assert.equal(body.original_transaction_id,undefined);assert.equal(c.init.redirect,"error");
 }
});
test("denied transaction reads are never interpreted as absence",async()=>{
 const client=new SsgClient("https://ssg.example","app-test","ac_"+randomUUID()+"."+"s".repeat(43),
 (async()=>Response.json({error:{code:"insufficient_scope"}},{status:403})) as typeof fetch);
 await assert.rejects(client.byKey("cs2.raid.test"),e=>e instanceof ApiError&&e.code==="insufficient_scope");
});
