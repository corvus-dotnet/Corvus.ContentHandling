# 2. Rebuild on Microsoft.Extensions.DependencyInjection keyed services

Date: 2026-07-04

## Status

Accepted

## Context

Corvus.ContentHandling creates instances of types, and dispatches content to handlers, identified by media-type-like content-type strings (e.g. `application/vnd.corvus.example+handler`), with hierarchical fallback resolution (`a.b.c+suffix` → `a.b+suffix` → `a+suffix`).

Through v4, the central `ContentFactory` was a hand-rolled keyed-service resolver: a `ConcurrentDictionary<string, (Type ImplementingType, bool UsesServices)>` layered over the container. Resolution went content type → `Type` → `IServiceProvider.GetService(type)` (or parameterless-constructor reflection for "simple" types). Because the registry could only resolve by `Type`, every lambda-based handler registration needed a *unique wrapper type*, which the library minted at runtime with Roslyn (`ContentHandlerGenerator`), pulling `Microsoft.CodeAnalysis.CSharp` into the dependency graph of every consumer.

.NET 8 introduced keyed services in `Microsoft.Extensions.DependencyInjection`: registrations and resolution by *(service type, key)* pair (`AddKeyedSingleton/Scoped/Transient`, `GetKeyedService`, `IKeyedServiceProvider`).

## Decision

v5 is a clean rebuild on keyed services:

- Content is registered as a keyed service whose service type is the concrete implementing type and whose key is the content-type string.
- A small singleton `IContentRegistry` records content-type metadata — required because keyed services cannot enumerate keys — and hosts the hierarchical fallback algorithm (`ContentRegistry.TryResolve`), which returns the *resolved* key for the subsequent keyed lookup.
- Serializer-constructed content (`AddSerializedContent<T>`) is registry-only: no descriptor is added, so container validation (`ValidateOnBuild`) is unaffected by types designed for constructor-based deserialization.
- Lambda handlers become keyed *instances* of a handful of closed generic adapter types. Because keyed service identity is the (service type, key) pair, no unique wrapper types are needed — the runtime Roslyn code generation and the `Microsoft.CodeAnalysis.CSharp` dependency are gone entirely.
- The dispatcher surface collapses from ~40 overloads to 8 async-first (`ValueTask`) methods with an optional `TContext` type parameter replacing the 0–3 loose parameters; it is registered as a transient open generic, so scoped handlers resolve from the correct scope (a latent defect of the v4 singleton dispatcher).
- The JSON layer keeps the v4 wire format (`contentType` discriminator, `{ "contentType": ..., "payload": ... }` envelopes). System.Text.Json's built-in polymorphism (`JsonPolymorphismOptions`) was evaluated and rejected: it has no per-key fallback for unknown discriminators (incompatible with hierarchical fallback), it conflicts with the real `ContentType` CLR property that serializes as `contentType`, and it only writes the discriminator when the declared type is the polymorphic base. A custom converter remains, modernized: the v4 reflection-based populate hack is replaced by overriding `JsonTypeInfo.CreateObject` on a fresh contract.

## Consequences

- Registration is strictly build-time: keyed descriptors cannot be added after `BuildServiceProvider()`. (The v4 `ContentFactory` singleton could technically be mutated post-build; no known consumer relied on this.)
- Registering the same implementing type under two content types yields two keyed singletons (two instances); `AddContentAlias` restores instance sharing explicitly.
- The public API is not source-compatible with v4; the feature set is preserved and the JSON wire format is unchanged, so serialized documents round-trip across versions.
- The library targets net10.0 only.
