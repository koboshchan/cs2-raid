# PriceTest

Experimental test plugin for CounterStrikeSharp 1.0.379. The stable ordinal item list in PriceTestPlugin.cs assigns prices $1 through $45. Every startup logs the mapping; !prices shows it.

The plugin spawns temporary weapon entities, writes CCSWeaponBaseVData.Price and logs readback, then removes the entities. Armor and defuse kit do not expose weapon VData and use the purchase correction path only.

A same-tick pre-buy account snapshot and item_purchase event aggregate the intended total, then correct account balance next frame. Purchases without a matching buy command snapshot are logged and left alone rather than guessing a refund. Engine affordability checks and client UI can still use original prices. This is a test, not production economy logic; disable refunds and unrelated economy plugins during testing.

Build from root with `docker build --target pricetest-artifacts --output type=local,dest=out .`. Install out/plugin/PriceTest.dll and PriceTest.deps.json in addons/counterstrikesharp/plugins/PriceTest.

Server VData readback is not evidence of client buy menu display changes. A real connected client must inspect prices and record its before/after cash. Client display has not yet been verified.
