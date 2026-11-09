# Semantic inventory

Inventory.cs.txt is an offline Roslyn inventory script, not plugin code. It resolves external method invocations, explicit/implicit constructors and member accesses against the current DeltaOps compilation. Requires local SDK Roslyn assemblies and exact reference assemblies. Paths in this audit instance are local reproducibility inputs, not deployment paths.

Output includes inherited legacy paths as well as new additions, so it cannot establish all calls are new. It is not a complete C# operation audit: implicit operators, indexers, deconstruction and compiler-generated calls are not yet enumerated. Each listed occurrence remains pending contract review until linked to evidence. Compilation success is binding/signature evidence only.
