import { randomUUID } from "node:crypto";
import { z } from "zod";
import type { RaidNativeBridge, RaidRequest, SettlementProof } from "./raid-ledger";
import type { RaidOperation } from "./raid-protocol";
export type NativeJob = {
  id: string; method: "freeze" | "finalize"; operationKey: string;
  operation: RaidOperation; request: RaidRequest; proof?: SettlementProof;
};
type Waiting = { job: NativeJob; resolve: (accepted: boolean) => void; timer: ReturnType<typeof setTimeout> };
// Single configured server/native consumer only. No native listener and no
// native command execution endpoint. Poll/reply share confidential backend auth.
export class NativeHttpBridge implements RaidNativeBridge {
  private waiting: Waiting[] = [];
  private leased: Waiting | null = null;
  constructor(private timeoutMs = 10_000, private maxQueue = 32) {
    if (!Number.isInteger(timeoutMs) || timeoutMs < 1 || timeoutMs > 30_000 ||
        !Number.isInteger(maxQueue) || maxQueue < 1 || maxQueue > 128) throw new Error("Invalid callback bounds");
  }
  private enqueue(job: Omit<NativeJob, "id">): Promise<boolean> {
    if (this.waiting.length + (this.leased ? 1 : 0) >= this.maxQueue) return Promise.resolve(false);
    return new Promise(resolve => {
      const entry: Waiting = {
        // Clone plain data; never retain controller/entity handles or mutable snapshots.
        job: structuredClone({ ...job, id: randomUUID() }), resolve,
        timer: setTimeout(() => {
          if (this.leased === entry) this.leased = null;
          this.waiting = this.waiting.filter(item => item !== entry);
          resolve(false); // Unknown native outcome. Durable ledger decides recovery.
        }, this.timeoutMs),
      };
      this.waiting.push(entry);
    });
  }
  next(input: unknown): { job: NativeJob | null } {
    z.object({}).strict().parse(input);
    // One lease at a time; repeated poll never distributes another callback.
    if (this.leased) return { job: null };
    this.leased = this.waiting.shift() ?? null;
    return { job: this.leased ? structuredClone(this.leased.job) : null };
  }
  reply(input: unknown) {
    const value = z.object({ id: z.string().uuid(), accepted: z.boolean() }).strict().parse(input);
    if (!this.leased || this.leased.job.id !== value.id) throw new Error("Stale or unknown native callback");
    const entry = this.leased; this.leased = null;
    clearTimeout(entry.timer); entry.resolve(value.accepted);
    return { accepted: true };
  }
  validateAndFreeze(operationKey: string, operation: RaidOperation, request: RaidRequest) {
    return this.enqueue({ method: "freeze", operationKey, operation, request });
  }
  finalize(proof: SettlementProof, request: RaidRequest) {
    return this.enqueue({ method: "finalize", operationKey: proof.operationKey, operation: proof.operation, proof, request });
  }
  close() {
    for (const entry of [...this.waiting, ...(this.leased ? [this.leased] : [])]) {
      clearTimeout(entry.timer); entry.resolve(false);
    }
    this.waiting = []; this.leased = null;
  }
}
