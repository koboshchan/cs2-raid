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
 await assert.rejects(f.ledger.execute("cashin",{...input,amountCents:100},f.api,f.native),/Idempotency/);
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
 const app=createServer(token,catalog,{status(){return {linked:false};},async create(){return {};},async poll(){return {};}},
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
 const app=createServer(token,catalog,{status(){return {linked:false};},async create(){return {};},async poll(){return {};}},{ledger:f.ledger,api:f.api,native:f.native});
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

test("recovery phases persist every crash boundary, replay older ack without regressing or replacing evidence",async()=>{
 const f=setup();try{
 const input=extract();f.crash();
 await assert.rejects(f.ledger.execute("extract",input,f.api,f.native));
 const receipt=[...f.receipts.values()][0];
 const row=JSON.parse(String(f.raw("SELECT data FROM raid_operations")[0].data));
 const identity={operationKey:row.key,requestFingerprint:row.fingerprint};
 const {state,...proof}=f.ledger.proof(identity);
 const snapshot=JSON.stringify({session,ammo:"x".repeat(50000)});
 for(const phase of ["snapshot","consumption_intent","consumed","cleanup_complete"] as const){
   const body={...identity,nativeSnapshot:snapshot,phase,evidence:"original "+phase,
      ...(phase!=="snapshot"?{proof}:{})};
   f.ledger.journalPhase(body);f.reopen();
   assert.equal(f.ledger.journalRead(identity).journal?.phase,phase);
 }
 const before=f.ledger.journalRead(identity);
 for(const phase of ["snapshot","consumption_intent","consumed","cleanup_complete"] as const)
   f.ledger.journalPhase({...identity,nativeSnapshot:snapshot,phase,evidence:"replacement",
    ...(phase!=="snapshot"?{proof}:{})});
 assert.deepEqual(f.ledger.journalRead(identity),before);
 assert.equal(f.calls,1);assert.equal(f.finals,1);
 assert.equal(proof.transactionId,receipt.transaction_id);
 }finally{f.cleanup();}
});

test("recovery cannot manufacture operation, change immutable identity/snapshot, skip phase or forge proof",async()=>{
 const f=setup();try{
 const proof=await f.ledger.execute("extract",extract(),f.api,f.native);
 const identity={operationKey:proof.operationKey,requestFingerprint:proof.requestFingerprint};
 const base={...identity,nativeSnapshot:"snapshot",phase:"snapshot",evidence:"freeze"};
 assert.throws(()=>f.ledger.journalPhase({...base,operationKey:"unknown.operation"}));
 assert.throws(()=>f.ledger.journalPhase({...base,requestFingerprint:"0".repeat(64)}));
 assert.throws(()=>f.ledger.journalPhase({...base,phase:"consumed",proof}));
 f.ledger.journalPhase(base);
 assert.throws(()=>f.ledger.journalPhase({...base,nativeSnapshot:"changed"}),/Immutable/);
 assert.throws(()=>f.ledger.journalPhase({...base,phase:"consumption_intent",proof:{...proof,transactionId:randomUUID()}}));
 const {state,...exact}=proof;
 assert.throws(()=>f.ledger.journalPhase({...base,phase:"consumption_intent",proof:{...exact,transactionId:randomUUID()}}),/Original proof/);
 assert.throws(()=>f.ledger.journalPhase({...base,receipt:"arbitrary"}));
 assert.equal(f.ledger.journalRead(identity).journal?.phase,"snapshot");
 }finally{f.cleanup();}
});

test("manual quarantine original-key lookup reconciles lost payment, never replays money or releases claims",async()=>{
 const f=setup();try{
 const input=extract();f.lose();await assert.rejects(f.ledger.execute("extract",input,f.api,f.native));
 const row=JSON.parse(String(f.raw("SELECT data FROM raid_operations")[0].data));
 const identity={operationKey:row.key,requestFingerprint:row.fingerprint};
 const body={...identity,operator:"operator-id",reason:"map lost",evidence:"canonical investigation",
   isolationEvidence:"pawn and entities isolated by operator"};
 let queried="";
 const api={...f.api,async byKey(key:string){queried=key;return f.api.byKey(key);}};
 const result=await f.ledger.quarantine(body,api);f.reopen();
 assert.equal(queried,row.key);assert.equal(result.disposition,"quarantine_requested");
 assert.equal(result.proof?.transactionId,[...f.receipts.values()][0].transaction_id);
 assert.equal(result.journal?.phase,undefined);
 assert.equal(result.audit.length,2);assert.equal(f.raw("SELECT * FROM raid_item_claims").length,2);
 await assert.rejects(f.ledger.execute("extract",input,f.api,f.native),/Terminal/);
 assert.throws(()=>f.ledger.journalPhase({...identity,nativeSnapshot:"x",phase:"snapshot",evidence:"x"}),/Terminal/);
 assert.equal(f.calls,1);assert.equal(f.finals,0);
 }finally{f.cleanup();}
});

test("lookup absence never means refund/release and lookup failures retain requested revocation",async()=>{
 const f=setup();try{
 const input=extract();f.invalid();await assert.rejects(f.ledger.execute("extract",input,f.api,f.native));
 const row=JSON.parse(String(f.raw("SELECT data FROM raid_operations")[0].data));
 const identity={operationKey:row.key,requestFingerprint:row.fingerprint};
 const body={...identity,operator:"op",reason:"rejected snapshot",evidence:"investigation",
 isolationEvidence:"no active native generation"};
 await assert.rejects(f.ledger.quarantine(body,{...f.api,async byKey(){throw new Error("lookup denied");}}),/lookup denied/);
 assert.equal(f.ledger.journalRead(identity).disposition,"quarantine_requested");
 const result=await f.ledger.quarantine(body,f.api);
 assert.equal(result.financialState,"prepared");assert.equal(result.proof,null);
 assert.equal(result.audit[1].originalKeyLookup.status,"absent_not_authority");
 assert.equal(f.raw("SELECT * FROM raid_item_claims").length,2);assert.equal(f.calls,0);
 }finally{f.cleanup();}
});

test("recovery-only HTTP works money OFF, admin auth isolated, Origin rejected, terminal readable to native",async()=>{
 const f=setup(),token="n".repeat(40),adminToken="a".repeat(40);
 const proof=await f.ledger.execute("cashin",cashin(),f.api,f.native);
 const identity={operationKey:proof.operationKey,requestFingerprint:proof.requestFingerprint};
 const app=createServer(token,catalog,{status(){return {linked:false};},async create(){return {};},async poll(){return {};}},undefined,
   {ledger:f.ledger,api:f.api,adminToken});
 try{
 const headers={authorization:"Bearer "+token},adminHeaders={authorization:"Bearer "+adminToken};
 const payload={...identity,nativeSnapshot:"z".repeat(50000),phase:"snapshot",evidence:"native"};
 assert.equal((await app.inject({method:"POST",url:"/native/recovery",headers,payload})).statusCode,200);
 assert.equal((await app.inject({method:"POST",url:"/native/recovery",headers:{...headers,origin:""},payload})).statusCode,401);
 assert.equal((await app.inject({method:"POST",url:"/cashin",headers,payload:cashin()})).statusCode,503);
 const manual={...identity,operator:"op",reason:"isolated",evidence:"review",isolationEvidence:"operator verified isolation"};
 assert.equal((await app.inject({method:"POST",url:"/admin/reconciliation/quarantine",headers,payload:manual})).statusCode,401);
 assert.equal((await app.inject({method:"POST",url:"/native/recovery",headers:adminHeaders,payload})).statusCode,401);
 assert.equal((await app.inject({method:"POST",url:"/admin/reconciliation/quarantine",headers:{...adminHeaders,origin:""},payload:manual})).statusCode,401);
 assert.equal((await app.inject({method:"POST",url:"/admin/reconciliation/quarantine",headers:adminHeaders,payload:manual})).statusCode,200);
 const read=await app.inject({method:"POST",url:"/native/journal/read",headers,payload:identity});
 assert.equal(read.json().disposition,"quarantine_requested");assert.equal(read.json().journal.phase,"snapshot");
 const ack={...identity,revocationEpoch:read.json().journal.revocationEpoch,session,nativePhase:"quarantined",isolationEvidence:"native fsync and physical isolation verified"};
 assert.equal((await app.inject({method:"POST",url:"/native/quarantine/ack",headers:adminHeaders,payload:ack})).statusCode,401);
 assert.equal((await app.inject({method:"POST",url:"/native/quarantine/ack",headers:{...headers,origin:""},payload:ack})).statusCode,401);
 const acked=await app.inject({method:"POST",url:"/native/quarantine/ack",headers,payload:ack});
 assert.equal(acked.statusCode,200);assert.equal(acked.json().disposition,"quarantined");

 assert.equal((await app.inject({method:"POST",url:"/native/recovery",headers,payload})).statusCode,400);
 assert.equal(f.calls,1);
 }finally{await app.close();f.cleanup();}
});

test("admin requests revocation immediately while finalize is pending",async()=>{
 const f=setup();try{
 const input=extract();let signal!:()=>void,release!:()=>void;
 const entered=new Promise<void>(r=>signal=r), wait=new Promise<void>(r=>release=r);
 const native={...f.native,async finalize(){signal();await wait;return false;}};
 const execution=f.ledger.execute("extract",input,f.api,native);await entered;
 const row=JSON.parse(String(f.raw("SELECT data FROM raid_operations")[0].data));
 let lookups=0;
 const manual=f.ledger.quarantine({operationKey:row.key,requestFingerprint:row.fingerprint,
  operator:"op",reason:"unresolved",evidence:"post-payment",isolationEvidence:"isolated"},
  {...f.api,async byKey(key:string){lookups++;return f.api.byKey(key);}});
 assert.equal(lookups,1);release();
 await assert.rejects(execution,/unresolved/);assert.equal((await manual).disposition,"quarantine_requested");
 assert.equal(lookups,1);assert.equal(f.calls,1);
 }finally{f.cleanup();}
});

test("prepayment evidence cannot attest consumption; zero extraction keeps null canonical transaction",async()=>{
 const f=setup();try{
 const input=cashin();f.invalid();await assert.rejects(f.ledger.execute("cashin",input,f.api,f.native));
 const row=JSON.parse(String(f.raw("SELECT data FROM raid_operations")[0].data));
 const identity={operationKey:row.key,requestFingerprint:row.fingerprint};
 const base={...identity,nativeSnapshot:"before",phase:"snapshot",evidence:"local fsync"};
 f.ledger.journalPhase(base);
 assert.throws(()=>f.ledger.journalPhase({...base,phase:"consumption_intent"}));
 assert.equal(f.ledger.journalRead(identity).journal?.phase,"snapshot");
 assert.equal(f.calls,0);
 }finally{f.cleanup();}
 const z=setup();try{
 const result=await z.ledger.execute("extract",{...extract(),cashCents:0,inventory:[]},z.api,z.native);
 const {state,...proof}=result;
 const base={operationKey:proof.operationKey,requestFingerprint:proof.requestFingerprint,nativeSnapshot:"zero",evidence:"zero"};
 z.ledger.journalPhase({...base,phase:"snapshot"});
 for(const phase of ["consumption_intent","consumed","cleanup_complete"])
   z.ledger.journalPhase({...base,phase,proof});
 assert.equal(z.calls,0);assert.equal(z.ledger.journalRead({operationKey:proof.operationKey,requestFingerprint:proof.requestFingerprint}).journal?.proof?.transactionId,null);
 }finally{z.cleanup();}
});

test("manual receipt mismatch retains requested revocation; admin configuration cannot reuse native secret",async()=>{
 const f=setup();try{
 const proof=await f.ledger.execute("extract",extract(),f.api,f.native);
 const identity={operationKey:proof.operationKey,requestFingerprint:proof.requestFingerprint};
 await assert.rejects(f.ledger.quarantine({...identity,operator:"op",reason:"audit",evidence:"audit",isolationEvidence:"isolated"},
 {...f.api,async byKey(key:string){return {...(await f.api.byKey(key))!,amount_cents:1};}}),/mismatch/);
 assert.equal(f.ledger.journalRead(identity).disposition,"quarantine_requested");
 assert.equal(f.ledger.journalRead(identity).audit[0].kind,"quarantine_requested");
 const links={status(){return {linked:false};},async create(){return {};},async poll(){return {};}};
 assert.throws(()=>createServer("n".repeat(40),catalog,links,undefined,{ledger:f.ledger,api:f.api,adminToken:"n".repeat(40)}),/separate/);
 assert.throws(()=>createServer("n".repeat(40),catalog,links,undefined,{ledger:f.ledger,api:f.api,adminToken:"short"}),/separate/);
 }finally{f.cleanup();}
});

test("native false during async recovery barrier retains paid and repeats finalize same proof without moving money",async()=>{
 const f=setup();try{
 const input=extract();let attempts=0;const seen:unknown[]=[];
 const native={...f.native,async finalize(proof: Parameters<RaidNativeBridge["finalize"]>[0]){
   attempts++;seen.push(proof);return attempts>=3;
 }};
 await assert.rejects(f.ledger.execute("extract",input,f.api,native),/Native completion unresolved/);
 let row=JSON.parse(String(f.raw("SELECT data FROM raid_operations")[0].data));
 assert.equal(row.state,"paid");
 f.reopen();
 await assert.rejects(f.ledger.execute("extract",input,f.api,native),/Native completion unresolved/);
 row=JSON.parse(String(f.raw("SELECT data FROM raid_operations")[0].data));
 assert.equal(row.state,"paid");
 assert.equal((await f.ledger.execute("extract",input,f.api,native)).state,"settled");
 assert.deepEqual(seen[0],seen[1]);assert.deepEqual(seen[1],seen[2]);
 assert.equal(attempts,3);assert.equal(f.calls,1);
 }finally{f.cleanup();}
});

test("revocation request survives restart, native epoch/session ack required and exact ack replay is idempotent",async()=>{
 const f=setup();try{
 const proof=await f.ledger.execute("cashin",cashin(),f.api,f.native);
 const identity={operationKey:proof.operationKey,requestFingerprint:proof.requestFingerprint};
 const manual={...identity,operator:"op",reason:"revoke",evidence:"race",isolationEvidence:"requested only"};
 const requested=await f.ledger.quarantine(manual,f.api),epoch=requested.journal!.revocationEpoch!;
 assert.equal(requested.disposition,"quarantine_requested");f.reopen();
 assert.equal(f.ledger.journalRead(identity).journal?.revocationEpoch,epoch);
 const ack={...identity,revocationEpoch:epoch,session,nativePhase:"quarantined",isolationEvidence:"fsync tombstone; original handle absent; connected actor spectator"};
 assert.throws(()=>f.ledger.quarantineAck({...ack,revocationEpoch:randomUUID()}),/identity/);
 assert.throws(()=>f.ledger.quarantineAck({...ack,session:{...session,pawnGeneration:"replacement"}}),/identity/);
 const terminal=f.ledger.quarantineAck(ack);
 assert.equal(terminal.disposition,"quarantined");
 assert.deepEqual(f.ledger.quarantineAck(ack),terminal);
 assert.throws(()=>f.ledger.quarantineAck({...ack,isolationEvidence:"changed"}),/Conflicting/);
 assert.equal((await f.ledger.quarantine(manual,f.api)).journal?.revocationEpoch,epoch);
 assert.equal(f.calls,1);
 }finally{f.cleanup();}
});

test("delayed positive finalize cannot settle after requested revocation, even before native ack",async()=>{
 const f=setup();try{
 let entered!:()=>void,release!:()=>void;
 const seen=new Promise<void>(r=>entered=r),gate=new Promise<void>(r=>release=r);
 const input=extract();
 const running=f.ledger.execute("extract",input,f.api,{...f.native,async finalize(){entered();await gate;return true;}});
 await seen;
 const row=JSON.parse(String(f.raw("SELECT data FROM raid_operations")[0].data));
 const identity={operationKey:row.key,requestFingerprint:row.fingerprint};
 const pending=await f.ledger.quarantine({...identity,operator:"op",reason:"revoke delayed callback",evidence:"race",isolationEvidence:"pending native"},f.api);
 assert.equal(pending.disposition,"quarantine_requested");
 assert.throws(()=>f.ledger.journalPhase({...identity,nativeSnapshot:"x",phase:"snapshot",evidence:"late"}),/Terminal/);
 release();await assert.rejects(running,/revocation requested/);
 assert.equal(f.ledger.journalRead(identity).financialState,"paid");
 await assert.rejects(f.ledger.execute("extract",input,f.api,f.native),/revocation requested/);
 assert.equal(f.calls,1);
 }finally{f.cleanup();}
});

test("revocation during in-flight payment preserves eventual receipt but never sends finalize",async()=>{
 const f=setup();try{
 let entered!:()=>void,release!:()=>void;
 const seen=new Promise<void>(r=>entered=r),gate=new Promise<void>(r=>release=r);
 const input=extract();
 const api={...f.api,async moveRaidMoney(op:RaidMoney){entered();await gate;return f.api.moveRaidMoney(op);}};
 const running=f.ledger.execute("extract",input,api,f.native);await seen;
 const row=JSON.parse(String(f.raw("SELECT data FROM raid_operations")[0].data));
 const identity={operationKey:row.key,requestFingerprint:row.fingerprint};
 const requested=await f.ledger.quarantine({...identity,operator:"op",reason:"in flight",evidence:"race",isolationEvidence:"await native"},f.api);
 assert.equal(requested.disposition,"quarantine_requested");
 assert.equal(requested.proof,null);
 release();await assert.rejects(running,/revocation requested/);
 const result=f.ledger.journalRead(identity);
 assert.equal(result.financialState,"paid");assert.equal(result.proof?.transactionId,[...f.receipts.values()][0].transaction_id);
 assert.equal(result.disposition,"quarantine_requested");assert.equal(f.finals,0);assert.equal(f.calls,1);
 }finally{f.cleanup();}
});

test("actual .NET SettlementProtocol.Json native dictionary phase wire and anonymous ack accepted exactly",async()=>{
 // Fixture generated by net10 using actual SettlementProtocol.cs, Dictionary<string,object>
 // with proof omitted at snapshot, and native anonymous ack shape. No TS reconstruction.
 const wire=JSON.parse(require("node:fs").readFileSync(join(__dirname,"native-wire.fixture.json"),"utf8"));
 const f=setup();try{
 const input={...wire.request,session};
 const api={...f.api,async moveRaidMoney(op:RaidMoney){
   const receipt=await f.api.moveRaidMoney(op);
   const canonical={...receipt,transaction_id:wire.proof.transactionId};
   f.receipts.set(op.key,canonical);return canonical;
 }};
 let exactProof:unknown;
 const native={...f.native,async finalize(proof:unknown){exactProof=proof;return false;}};
 await assert.rejects(f.ledger.execute("cashin",input,api,native));
 const row=JSON.parse(String(f.raw("SELECT data FROM raid_operations")[0].data));
 // Fixture has static app user. Bind fixture user to test's actual durable identity only.
 // The serialized proof field names and all operation/session/amount/transaction bytes are unchanged.
 for(const body of wire.payloads){
   if(body.proof)body.proof.appUserId=appUserId;
   f.ledger.journalPhase(body);
 }
 assert.deepEqual(wire.payloads[1].proof,exactProof);
 const identity={operationKey:row.key,requestFingerprint:row.fingerprint};
 const requested=await f.ledger.quarantine({...identity,operator:"op",reason:"wire test",evidence:"dotnet wire",isolationEvidence:"native handshake required"},f.api);
 wire.ack.revocationEpoch=requested.journal!.revocationEpoch;
 assert.equal(f.ledger.quarantineAck(wire.ack).disposition,"quarantined");
 assert.equal(f.calls,1);
 }finally{f.cleanup();}
});
