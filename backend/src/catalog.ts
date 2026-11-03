import { readFileSync } from "node:fs";
import { z } from "zod";
const entry = z.object({
  id: z.string().regex(/^[a-z0-9_-]{1,48}$/),
  entity: z.string().regex(/^[a-z0-9_]{1,64}$/),
  priceDollars: z.number().positive().finite(),
  kind: z.string().regex(/^[a-z0-9_-]{1,32}$/),
}).strict();
export type CatalogItem = { id: string; entity: string; priceCents: number; kind: string };
export type Catalog = ReadonlyMap<string, CatalogItem>;
export function parseCatalog(input: unknown): Catalog {
  const result = new Map<string, CatalogItem>();
  for (const row of z.array(entry).min(1).parse(input)) {
    const cents = Math.round(row.priceDollars * 100);
    if (!Number.isSafeInteger(cents) || cents <= 0 || Math.abs(cents / 100 - row.priceDollars) > 1e-9)
      throw new Error("Catalog prices must be safe integer cents");
    if (result.has(row.id)) throw new Error("Duplicate catalog ID");
    result.set(row.id, Object.freeze({ id: row.id, entity: row.entity, kind: row.kind, priceCents: cents }));
  }
  return result;
}
export function loadCatalog(path: string): Catalog { return parseCatalog(JSON.parse(readFileSync(path, "utf8"))); }
