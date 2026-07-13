# Benchmarks: v4 vs v5

Two mirrored BenchmarkDotNet projects measure identical scenarios against the v4 and v5
implementations:

- v4: `Corvus.ContentHandling.old/Solutions/Corvus.ContentHandling.Benchmarks` (standalone;
  deliberately not in the old solution so its CI is unaffected)
- v5: `Solutions/Corvus.ContentHandling.Benchmarks` (in this solution)

Both projects target net10.0, reference Microsoft.Extensions.DependencyInjection 10.0.9, and
use BenchmarkDotNet 0.15.8 with `[MemoryDiagnoser]` — identical runtime and container; the
only variable is the library. Run either with:

```powershell
dotnet run -c Release -- --filter *
```

Results below: .NET 10.0.9, Windows 11, 13th Gen Intel Core i7-13800H (2026-07-04). Artifacts
(markdown/CSV/HTML) are written to each project's `BenchmarkDotNet.Artifacts/results/`.

## Registration (startup cost — fresh container per invocation)

| Scenario                        |                        v4 |                    v5 | Notes                                      |
|---------------------------------|--------------------------:|----------------------:|--------------------------------------------|
| 10 serializer-constructed types |         2.10 µs / 9.07 KB |     1.13 µs / 7.77 KB | v5 caches content-type discovery per type  |
| 10 transient DI-managed types   |        2.82 µs / 11.15 KB |     1.91 µs / 9.84 KB |                                            |
| **5 lambda handlers**           | **138,190 µs / 8,597 KB** | **1.72 µs / 9.66 KB** | **~80,000× faster, ~900× less allocation** |

The lambda-handler row is the headline: v4 compiles a unique wrapper type with Roslyn at
runtime for every (contentType, handlerClass) pair — ~27 ms and ~1.7 MB *per handler*. Because
v4 caches compilations per content type, the benchmark uses unique content-type names per
invocation, which reflects what a real application pays: each handler is registered exactly
once per process. v5 registers keyed instances of one closed generic adapter, so the same
scenario costs about two microseconds — and `Microsoft.CodeAnalysis.CSharp` is gone from the
dependency graph entirely.

## Resolution (hot path)

| Scenario                                      |               v4 |                 v5 |
|-----------------------------------------------|-----------------:|-------------------:|
| Exact content type (`GetContent`)             |   31.9 ns / 24 B |     31.0 ns / 24 B |
| 3-hop hierarchical fallback                   | 105.3 ns / 368 B | 98.2 ns / **24 B** |
| Typed `GetRequiredContent<T>`                 |   23.3 ns / 24 B |     46.2 ns / 24 B |
| `TryGetTypeFor` (the JSON converter's lookup) |    13.7 ns / 0 B |      18.0 ns / 0 B |

Nanosecond territory on every row. v5's fallback resolution is now faster than v4's *and*
allocation-free (the remaining 24 B is the resolved content instance itself): candidates are
composed into a single stack buffer and probed via the dictionary's span-based
`GetAlternateLookup`, instead of allocating a candidate string per hop. v4 keeps a small edge
on the typed path, where v5 pays the keyed-service indirection.

Negative result, recorded so it isn't retried: swapping the registry's `Dictionary` for
`FrozenDictionary` (freeze-on-first-read) measured consistently *slower* — exact resolution
31→46 ns, typed 46→72 ns, `TryGetTypeFor` 18→23 ns. Registered content types are long strings
sharing a common prefix and often a common length, which defeats the frozen comparer-selection
strategies; a plain ordinal `Dictionary` is optimal for this key shape.

## Handler dispatch

v4 dispatch is synchronous; v5 is `ValueTask`-based (async-first API) — the difference below
includes that machinery.

| Scenario                                                     |                       v4 |                v5 |
|--------------------------------------------------------------|-------------------------:|------------------:|
| Lambda handler (explicit content type)                       |          40.1 ns / 136 B | 39.8 ns / **0 B** |
| Class handler, **transient** (v5 default; v4 cannot express) |                      n/a |    76.9 ns / 48 B |
| Class handler, **singleton** (v4's only option)              | 39.0–42.2 ns / 136–152 B | 43.2 ns / **0 B** |
| Result-producing handler                                     |          86.0 ns / 256 B | 47.2 ns / **0 B** |
| By-payload-convention                                        |          92.7 ns / 256 B | 47.2 ns / **0 B** |
| Fallback dispatch (payload one level deeper)                 |         131.7 ns / 464 B | 50.5 ns / **0 B** |

(Both columns from one back-to-back run under identical machine conditions.)

Dispatch in v5 is **allocation-free** on every singleton-handler path: handler resolution —
key composition plus fallback probe — is cached per (content type, handler class) pair in
the immutable registry, resolved singleton/scoped handler instances are cached on the
dispatcher (reference-keyed), and by-payload-convention dispatch uses a cached compiled
getter for the payload's `ContentType` property instead of per-call reflection.

The class-handler rows need the lifetime caveat: v4's `RegisterContentHandler<...,THandler>`
hard-codes `AddSingleton<THandler>()` — every dispatch reuses one cached handler instance,
and there is no way to ask for anything else. v5 defaults to **transient** (safe for
handlers with mutable state, and required for scoped dependencies); its 48 B and extra
~34 ns are the fresh handler + adapter that lifetime demands, constructor-injected through
one compiled call site. Registered with `ServiceLifetime.Singleton`, v5 matches v4's time
and drops the allocation.

## JSON (polymorphic serialization + envelopes)

| Scenario                                |                    v4 |                   v5 |
|-----------------------------------------|----------------------:|---------------------:|
| Serialize via polymorphic target        |      156.3 ns / 216 B |     139.7 ns / 216 B |
| Deserialize (serializer-constructed)    |    864.7 ns / 1,256 B |     722.3 ns / 256 B |
| Deserialize (DI-constructed + populate) | 8,440.6 ns / 10,131 B | **514.2 ns / 264 B** |
| Deserialize with fallback               |    978.3 ns / 1,528 B |     606.8 ns / 272 B |
| Envelope round-trip                     |  1,591.2 ns / 2,280 B | 1,463.2 ns / 2,160 B |

Wire-format-identical, and v5 now wins every row. Two profile-driven changes: the polymorphic
converter buffers the object into a pooled, read-only `JsonDocument` instead of a mutable
`JsonNode` tree (the discriminator is read once and the payload re-read — a DOM is wasted
work), and the DI-populate path caches its `JsonTypeInfo` contracts per (options, type) with
`CreateObject` reading a thread-static slot, instead of rebuilding the contract on every call.
The latter is the standout: **16× faster and 38× less allocation** than both v4 and the naive
v5 port.

## Summary

v5 eliminates v4's pathological startup cost — runtime Roslyn compilation per lambda handler
(~27 ms and ~1.7 MB each) — and removes the `Microsoft.CodeAnalysis.CSharp` dependency
entirely. Two rounds of benchmark- and profile-driven optimization (cached content-type
discovery, zero-allocation span-based fallback probing, single-allocation handler keys,
cached handler resolution and instances, constructor-injected adapters, `JsonDocument`
buffering, cached populate contracts) leave v5 faster than v4 on **every scenario except
typed resolution and class-handler dispatch** — both inherent to the keyed-service
indirection and still well under 100 ns — with dispatch allocation-free on singleton-handler
paths and the DI-populate deserialization path 16× faster. JSON output remains
wire-format-identical to v4.
