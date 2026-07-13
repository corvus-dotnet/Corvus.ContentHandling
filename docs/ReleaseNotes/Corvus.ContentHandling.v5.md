# Release notes for Corvus.ContentHandling v5.0

## v5 is a rebuild on Microsoft.Extensions.DependencyInjection keyed services

v5 reimplements the library on the keyed-services capability introduced in .NET 8 (`AddKeyedSingleton/Scoped/Transient`, `GetKeyedService`). Content types are now keyed service registrations (keyed by the content-type string) plus a small metadata registry (`IContentRegistry`) that provides content-type enumeration and hierarchical fallback resolution.

The feature set carries over: content registration with singleton/scoped/transient lifetimes, convention-based content-type discovery, hierarchical content-type fallback resolution, handler dispatch, content-type enumeration by suffix, polymorphic `contentType`-discriminated System.Text.Json serialization, and `ContentEnvelope`.

**The JSON wire format is unchanged** — the `contentType` discriminator property and the `{ "contentType": ..., "payload": ... }` envelope shape are identical to v4, so documents serialized by v4 deserialize with v5 and vice versa.

## Breaking changes

- **New API, not source-compatible with v4.** `ContentFactory` and the `RegisterContent`/ `Register*Content` extensions are replaced by `services.AddContentHandling(content => ...)` with a `ContentHandlingBuilder` (`AddContent<T>`, `AddSingletonContent<T>`, `AddScopedContent<T>`, `AddTransientContent<T>`, `AddContentInstance<T>`, `AddSerializedContent<T>`, `AddContentAlias`, `AddContentHandler...`). Resolution extensions (`GetContent`, `GetRequiredContent`, `GetAllContent`, `GetAllContentTypes`, `TryGetTypeFor`) remain on `IServiceProvider`.
- **net10.0 only.** Earlier target frameworks are not supported.
- **Registration is strictly build-time.** Keyed services cannot be added after `BuildServiceProvider()`. The v4 `ContentFactory` could technically be mutated after the container was built; that is no longer possible.
- **Handler dispatch is async-first.** The ~40 `DispatchPayloadToHandler` overloads are replaced by 8 `DispatchAsync`/`DispatchWithResultAsync` methods returning `ValueTask`, with an optional `TContext` type parameter replacing the 0–3 loose parameters (use a tuple for multiple values). The sixteen handler interfaces collapse to four (`IContentHandler<TPayload>`, `IContentHandler<TPayload, TContext>`, and the two `IContentHandlerWithResult` variants), all `ValueTask`-based.
- **Same type under two content types now yields two singleton instances.** In v4 the two registrations shared one unkeyed container entry; keyed registrations are distinct. Use `AddContentAlias(contentType, targetContentType)` to share an instance across content types.
- **`ContentEnvelope.Match` is now fluent.** The 30 fixed-arity `Match`/`MatchAsync` overloads are replaced by `envelope.Match().When<T>(...).Else(...).Execute()` and `envelope.MatchAsync().When<T>(...).ExecuteAsync()`.
- **Serializing the polymorphic target type itself now throws `NotSupportedException`** on write as well as read (v4 recursed infinitely on write). Use an interface or abstract base as the polymorphic target.
- **DI-constructed content must be transient to deserialize.** Polymorphic deserialization of `FromServices` content populates the instance resolved from the container, so a singleton or scoped registration would hand every deserialization the same repeatedly overwritten object; the converter now rejects those lifetimes with `NotSupportedException`. Singleton and scoped content remain fully supported for resolution and dispatch.

## Improvements

- **No more runtime code generation.** v4 compiled a unique wrapper type per lambda handler registration with Roslyn at runtime, making every consumer depend on `Microsoft.CodeAnalysis.CSharp`. Keyed services resolve by (service type, key), so lambda handlers are now plain keyed instances of a few closed generic adapters. The `Microsoft.CodeAnalysis.CSharp` dependency is gone.
- **Content-type discovery via attribute.** `[ContentType("...")]` is the primary convention; the v4 static `RegisteredContentType` field convention still works, and the instance `ContentType` property is still honoured for object-level discovery (and remains part of the JSON wire format).
- **Scoped handlers now resolve from the correct scope.** The dispatcher is a transient open generic rather than a root singleton.
- **Duplicate registrations fail fast** with `InvalidOperationException` at configure time, for handlers as well as content (v4's `RegisterTransientContent<T>()` could silently skip registration when `T` was already in the container).
- **Factory registrations are honoured.** (A v4 overload accepted an implementation factory and ignored it.)
- **DI-constructed deserialization is simpler and faster**: the reflection/`MakeGenericMethod` populate mechanism is replaced by overriding `JsonTypeInfo.CreateObject` on a fresh contract.