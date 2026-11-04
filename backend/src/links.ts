import { DatabaseSync } from "node:sqlite";
import { z } from "zod";
import { ApiError, type PendingLink, type SsgClient } from "./client";
export const steamSchema = z.string().regex(/^[0-9]{17}$/);
type LinkApi = Pick<SsgClient, "createLink" | "pollLink">;
export class LinkStore {
  private db: DatabaseSync;
  constructor(path: string, private serverId: string, private api: LinkApi) {
    this.db = new DatabaseSync(path);
    this.db.exec(`PRAGMA journal_mode=WAL; PRAGMA synchronous=FULL; PRAGMA busy_timeout=5000;
      CREATE TABLE IF NOT EXISTS links (steam TEXT PRIMARY KEY, data TEXT NOT NULL, next_poll INTEGER NOT NULL) STRICT;
      CREATE TABLE IF NOT EXISTS bindings (steam TEXT PRIMARY KEY, app_user TEXT NOT NULL UNIQUE, grant_id TEXT NOT NULL) STRICT;`);
  }
  close() { this.db.close(); }
  status(steam: string) {
    steamSchema.parse(steam);
    return { linked: !!this.db.prepare("SELECT steam FROM bindings WHERE steam=?").get(steam) };
  }
  async create(steam: string) {
    steamSchema.parse(steam);
    if (this.db.prepare("SELECT steam FROM bindings WHERE steam=?").get(steam)) throw new Error("Already linked; operator unlink required");
    const old = this.db.prepare("SELECT data FROM links WHERE steam=?").get(steam);
    if (old) {
      const link = JSON.parse(String(old.data)) as PendingLink;
      if (Date.parse(link.expires_at) > Date.now()) return this.publicLink(link);
    }
    const link = await this.api.createLink(steam, this.serverId);
    this.db.prepare("INSERT INTO links VALUES (?,?,?) ON CONFLICT(steam) DO UPDATE SET data=excluded.data,next_poll=excluded.next_poll WHERE links.data = ?")
      .run(steam, JSON.stringify(link), Date.now() + link.interval_seconds * 1000, old ? String(old.data) : "");
    const stored = this.db.prepare("SELECT data FROM links WHERE steam=?").get(steam)!;
    return this.publicLink(JSON.parse(String(stored.data)) as PendingLink);
  }
  private publicLink(link: PendingLink) {
    const { device_token: _secret, ...publicData } = link; return publicData;
  }
  async poll(steam: string) {
    steamSchema.parse(steam);
    const row = this.db.prepare("SELECT data,next_poll FROM links WHERE steam=?").get(steam);
    if (!row) throw new Error("No pending link");
    const link = JSON.parse(String(row.data)) as PendingLink;
    if (Date.parse(link.expires_at) <= Date.now()) return { state: "expired" };
    const now = Date.now();
    if (Number(row.next_poll) > now) throw new ApiError(429, "slow_down", Math.ceil((Number(row.next_poll)-now)/1000));
    // Claim before awaiting network, including across workers. Failed calls retain the delay.
    const claim = this.db.prepare("UPDATE links SET next_poll=? WHERE steam=? AND next_poll=?")
      .run(now + link.interval_seconds*1000, steam, row.next_poll);
    if (claim.changes !== 1) throw new ApiError(429, "slow_down", link.interval_seconds);
    try {
      const result = await this.api.pollLink(link);
      if (result.state === "approved") {
        if (!result.granted_scopes.includes("balance:debit")) throw new ApiError(403, "insufficient_scope");
        this.db.exec("BEGIN IMMEDIATE");
        try {
          const bound = this.db.prepare("SELECT app_user,grant_id FROM bindings WHERE steam=?").get(steam);
          if (bound && (bound.app_user !== result.app_user_id || bound.grant_id !== result.grant_id)) throw new Error("Binding conflict");
          if (!bound) this.db.prepare("INSERT INTO bindings VALUES (?,?,?)").run(steam,result.app_user_id,result.grant_id);
          this.db.exec("COMMIT");
        } catch(e) { this.db.exec("ROLLBACK"); throw e; }
      }
      return { state: result.state };
    } catch(e) {
      if (e instanceof ApiError && e.retryAfterSeconds)
        this.db.prepare("UPDATE links SET next_poll=MAX(next_poll,?) WHERE steam=?").run(Date.now()+e.retryAfterSeconds*1000,steam);
      throw e;
    }
  }
}
