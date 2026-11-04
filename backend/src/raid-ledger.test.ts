import test from "node:test";
import assert from "node:assert/strict";
import { DatabaseSync } from "node:sqlite";
import { mkdtempSync, rmSync } from "node:fs";
import { tmpdir } from "node:os";
import { join } from "node:path";
import { randomUUID } from "node:crypto";
import { RaidLedger, type RaidMoneyApi, type RaidNativeBridge, type RaidMoney } from "./raid-ledger";
import type { Receipt } from "./outbox";
import { parseCatalog } from "./catalog";
import { createServer } from "./server";
const steamId64 = "76561198000000001", appUserId = randomUUID(), grantId = randomUUID();
const session = { steamId64, connectionId: "conn", pawnGeneration: "pawn", mapGeneration: "map" };
const cashin = () => ({ raidId: randomUUID(), session, amountCents: 1_500_000 });
const extract = () => ({ raidId: randomUUID(), session, extractionIntentId: "intent", extractionWindowId: "window",
 cashCents: 10000, inventory: [
 { instanceId: randomUUID(), catalogId: "ak", kind: "gun" },
 { instanceId: randomUUID(), catalogId: "he", kind: "unused_utility" },
 ] });
const catalog = parseCatalog([
 {id:"ak",entity:"weapon_ak47",kind:"rifle",priceDollars:2700},
 {id:"he",entity:"weapon_hegrenade",kind:"grenade",priceDollars:300},
 {id:"armor",entity:"item_kevlar",kind:"armor",priceDollars:650},
]);
function setup() {
 const dir=mkdtempSync(join(tmpdir(),"raid-ledger-")), path=join(dir,"raid.sqlite");
 let ledger=new RaidLedger(path,"raid",catalog);
 const db=new DatabaseSync(path); db.prepare("INSERT INTO bindings VALUES (?,?,?)").run(steamId64,appUserId,grantId); db.close();
 const receipts=new Map<string,Receipt>(), given=new Set<string>();
 let calls=0, finals=0, valid=true, lose=false, finalCrash=false;
 const api: RaidMoneyApi={
 async byKey(key){return receipts.get(key)??null;},
 async moveRaidMoney(op: RaidMoney) {
   calls++; const receipt:Receipt={transaction_id:randomUUID(),app_user_id:op.appUserId,grant_id:grantId,
     status:"committed",direction:op.operation==="cashin"?"debit":"credit",amount_cents:op.amountCents,
     external_reference:op.key,...(op.operation==="extract"?{credit_reason:"mint" as const}:{})};
   receipts.set(op.key,receipt); if(lose){lose=false;throw new Error("lost response");} return receipt;
 }};
 const native: RaidNativeBridge={
 async validateAndFreeze(){return valid;},
 async finalize(proof){ if(!valid)return false; if(!given.has(proof.operationKey)){given.add(proof.operationKey);finals++;}
   if(finalCrash){finalCrash=false;throw new Error("lost native ack");} return true; }
 };
 return {get ledger(){return ledger;},api,native,receipts,given,
 get calls(){return calls;},get finals(){return finals;},
 invalid(){valid=false;}, lose(){lose=true;},crash(){finalCrash=true;},
 reopen(){ledger.close();ledger=new RaidLedger(path,"raid",catalog);},
 raw(sql:string){const db=new DatabaseSync(path);try{return db.prepare(sql).all();}finally{db.close();}},
 cleanup(){ledger.close();rmSync(dir,{recursive:true,force:true});}};
}
test("cash-in durable debit max15k and replay never debit/native-add twice",async()=>{
 const f=setup();try{const input=cashin();
 const proof=await f.ledger.execute("cashin",input,f.api,f.native);
 assert.equal(proof.state,"settled");assert.equal(proof.amountCents,1500000);
 f.reopen();assert.deepEqual(await f.ledger.execute("cashin",input,f.api,f.native),proof);
 assert.equal(f.calls,1);assert.equal(f.finals,1);
 await assert.rejects(f.ledger.execute("cashin",{...input,amountCents:1},f.api,f.native),/Idempotency/);
 await assert.rejects(f.ledger.execute("cashin",{...cashin(),amountCents:1500001},f.api,f.native));
 }finally{f.cleanup();}
});
test("extraction mints full server-price guns and unused utility, not arbitrary caller value",async()=>{
 const f=setup();try{const input=extract();
 const proof=await f.ledger.execute("extract",input,f.api,f.native);
 assert.equal(proof.amountCents,310000);assert.equal([...f.receipts.values()][0].credit_reason,"mint");
 assert.equal((await f.ledger.execute("extract",{...input,inventory:[...input.inventory].reverse()},f.api,f.native)).state,"settled");
 assert.equal(f.calls,1);
 await assert.rejects(f.ledger.execute("extract",{...extract(),inventory:[{...input.inventory[0],catalogId:"armor"}]},f.api,f.native));
 await assert.rejects(f.ledger.execute("extract",{...extract(),inventory:input.inventory},f.api,f.native));
 }finally{f.cleanup();}
});
test("lost mint and debit responses survive crash/reopen and reconcile same key",async()=>{
 for(const kind of ["cashin","extract"] as const){const f=setup();try{
 const input=kind==="cashin"?cashin():extract();f.lose();
 await assert.rejects(f.ledger.execute(kind,input,f.api,f.native),/lost response/);
 f.reopen();assert.equal((await f.ledger.execute(kind,input,f.api,f.native)).state,"settled");
 assert.equal(f.calls,1);assert.equal(f.finals,1);
 }finally{f.cleanup();}}
});
test("native ack loss after SSG commit retries idempotent proof, never issues money again",async()=>{
 const f=setup();try{const input=extract();f.crash();
 await assert.rejects(f.ledger.execute("extract",input,f.api,f.native),/lost native ack/);
 f.reopen();assert.equal((await f.ledger.execute("extract",input,f.api,f.native)).state,"settled");
 assert.equal(f.calls,1);assert.equal(f.finals,1);
 }finally{f.cleanup();}
});
test("native stale session or forged snapshot fails before any financial call",async()=>{
 const f=setup();try{f.invalid();
 await assert.rejects(f.ledger.execute("cashin",cashin(),f.api,f.native),/Native session/);
 assert.equal(f.calls,0);assert.equal(f.finals,0);
 }finally{f.cleanup();}
});
test("missing consent, scope, revoked credential or exhausted upstream budget cannot produce a proof",async()=>{
 for(const code of ["insufficient_scope","invalid_client","limit_exceeded","unavailable"]){
 const f=setup();try{const api={...f.api,async moveRaidMoney():Promise<Receipt>{throw new Error(code);}};
 await assert.rejects(f.ledger.execute("extract",extract(),api,f.native),new RegExp(code));
 assert.equal(f.finals,0);
 }finally{f.cleanup();}}
});
test("canonical mismatched receipt becomes durable manual state, never accepts arbitrary text",async()=>{
 const f=setup();try{const input=extract();
 const api={...f.api,async moveRaidMoney(op:RaidMoney){const r=await f.api.moveRaidMoney(op);return {...r,grant_id:randomUUID()};}};
 await assert.rejects(f.ledger.execute("extract",input,api,f.native),/receipt mismatch/);
 f.reopen();await assert.rejects(f.ledger.execute("extract",input,f.api,f.native),/Manual/);
 assert.equal(f.finals,0);
 }finally{f.cleanup();}
});
test("receipt persists after player disconnect and cannot auto-refund or pay another generation",async()=>{
 const f=setup();try{const input=cashin();f.lose();
 await assert.rejects(f.ledger.execute("cashin",input,f.api,f.native));
 f.invalid();f.reopen();
 await assert.rejects(f.ledger.execute("cashin",input,f.api,f.native),/completion unresolved/);
 await assert.rejects(f.ledger.execute("cashin",{...input,session:{...session,connectionId:"new"}},f.api,f.native),/Idempotency/);
 assert.equal(f.calls,1);assert.equal(f.finals,0);
 assert.equal(JSON.parse(String(f.raw("SELECT data FROM raid_operations")[0].data)).state,"paid");
 }finally{f.cleanup();}
});
test("zero-value extraction has durable local completion but no fake SSG transaction",async()=>{
 const f=setup();try{
 const proof=await f.ledger.execute("extract",{...extract(),cashCents:0,inventory:[]},f.api,f.native);
 assert.equal(proof.transactionId,null);assert.equal(proof.amountCents,0);assert.equal(proof.state,"settled");assert.equal(f.calls,0);
 }finally{f.cleanup();}
});
test("unlinked account and forged armor fail closed",async()=>{
 const f=setup();try{
 await assert.rejects(f.ledger.execute("cashin",{...cashin(),session:{...session,steamId64:"76561198000000002"}},f.api,f.native),/Link/);
 await assert.rejects(f.ledger.execute("extract",{...extract(),inventory:[{instanceId:randomUUID(),catalogId:"armor",kind:"gun"}]},f.api,f.native),/Non-refundable/);
 assert.equal(f.calls,0);
 }finally{f.cleanup();}
});
test("injected HTTP routes enforce confidential auth and native checks, default boot closed",async()=>{
 const f=setup(),token="t".repeat(40);
 const app=createServer(token,catalog,{async create(){return {};},async poll(){return {};}},
 {ledger:f.ledger,api:f.api,native:f.native});
 try {
 const input=extract();
 assert.equal((await app.inject({method:"POST",url:"/extract",payload:input})).statusCode,401);
 const headers={authorization:"Bearer "+token};
 assert.equal((await app.inject({method:"POST",url:"/extract",headers:{...headers,origin:"https://evil.test"},payload:input})).statusCode,401);
 assert.equal((await app.inject({method:"POST",url:"/extract",headers,payload:{...input,receipt:"fake"}})).statusCode,400);
 const response=await app.inject({method:"POST",url:"/extract",headers,payload:input});
 assert.equal(response.statusCode,200);assert.equal(response.json().state,"settled");assert.equal(f.calls,1);
 }finally{await app.close();f.cleanup();}
});
test("two duplicate workers may reconcile but cannot double issue under canonical SSG idempotency",async()=>{
 const f=setup();try{
 const canonical=new Map<string,Promise<Receipt>>();
 const api={...f.api,moveRaidMoney(op:RaidMoney){let pending=canonical.get(op.key);if(!pending){pending=f.api.moveRaidMoney(op);canonical.set(op.key,pending);}return pending;}};
 const input=extract();
 const results=await Promise.all([f.ledger.execute("extract",input,api,f.native),f.ledger.execute("extract",input,api,f.native)]);
 assert.equal(results[0].operationKey,results[1].operationKey);assert.equal(f.calls,1);assert.equal(f.finals,1);
 }finally{f.cleanup();}
});
test("freeze crash before network reopens identical durable request without money movement",async()=>{
 const f=setup();try{const input=cashin();let crash=true;
 const native={...f.native,async validateAndFreeze(){if(crash){crash=false;throw new Error("freeze ack lost");}return true;}};
 await assert.rejects(f.ledger.execute("cashin",input,f.api,native),/freeze ack lost/);
 assert.equal(f.calls,0);f.reopen();
 assert.equal((await f.ledger.execute("cashin",input,f.api,native)).state,"settled");assert.equal(f.calls,1);
 }finally{f.cleanup();}
});
test("proof lookup is authenticated, read-only, durable and requires exact persisted fingerprint",async()=>{
 const f=setup(),token="p".repeat(40),input=extract();
 const settled=await f.ledger.execute("extract",input,f.api,f.native); f.reopen();
 const app=createServer(token,catalog,{async create(){return {};},async poll(){return {};}},{ledger:f.ledger,api:f.api,native:f.native});
 try{
 const payload={operationKey:settled.operationKey,requestFingerprint:settled.requestFingerprint};
 const headers={authorization:"Bearer "+token};
 assert.equal((await app.inject({method:"POST",url:"/settlement/proof",payload})).statusCode,401);
 assert.equal((await app.inject({method:"POST",url:"/settlement/proof",headers:{...headers,origin:""},payload})).statusCode,401);
 assert.equal((await app.inject({method:"POST",url:"/settlement/proof",headers,payload:{...payload,transactionId:"fake"}})).statusCode,400);
 assert.equal((await app.inject({method:"POST",url:"/settlement/proof",headers,payload:{...payload,requestFingerprint:"0".repeat(64)}})).statusCode,400);
 const response=await app.inject({method:"POST",url:"/settlement/proof",headers,payload});
 assert.deepEqual(response.json(),settled);assert.equal(f.calls,1);assert.equal(f.finals,1);
 assert.equal((await app.inject({method:"POST",url:"/finalize",headers,payload:settled})).statusCode,404);
 }finally{await app.close();f.cleanup();}
});
