import type { CatalogItem } from "./catalog";
import type { GameAdapter, PlayerSession } from "./outbox";

// The C# bridge must implement this interface on the installed CSSharp build.
// Only plain strings cross the async boundary, never native event/controller handles.
export interface GameThreadBridge {
  // This promise resolves only after one synchronous callback completes on the game thread.
  // Rejection, cancellation or a scheduling timeout is ambiguous, not known-no-delivery.
  runOnGameThread<T>(callback: () => T): Promise<T>;
  currentAuthorizedSession(steamId64: string): PlayerSession | null;
  eligibleForItem(steamId64: string, item: CatalogItem): boolean;
  // Called in the SAME callback as identity and eligibility checks.
  // Return delivered only after verified item creation. Any uncertain native result is unknown.
  giveItem(steamId64: string, item: CatalogItem): "delivered" | "unknown";
}
export function gameAdapter(bridge: GameThreadBridge): GameAdapter {
  return {
    async deliverOnGameThread(expected, item) {
      try {
        return await bridge.runOnGameThread(() => {
          const current = bridge.currentAuthorizedSession(expected.steamId64);
          if (!current || current.steamId64 !== expected.steamId64 ||
              current.connectionId !== expected.connectionId ||
              current.pawnGeneration !== expected.pawnGeneration ||
              current.mapGeneration !== expected.mapGeneration ||
              !bridge.eligibleForItem(expected.steamId64, item)) return "not_delivered";
          return bridge.giveItem(expected.steamId64, item);
        });
      } catch {
        return "unknown";
      }
    },
  };
}
