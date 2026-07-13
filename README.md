# Corvus.ContentHandling
[![Build Status](https://github.com/corvus-dotnet/Corvus.ContentHandling/actions/workflows/build.yml/badge.svg)](https://github.com/corvus-dotnet/Corvus.ContentHandling/actions/workflows/build.yml)
[![GitHub license](https://img.shields.io/badge/License-Apache%202-blue.svg)](https://raw.githubusercontent.com/corvus-dotnet/Corvus.ContentHandling/main/LICENSE)
[![IMM](https://imm.endjin.com/api/imm/github/corvus-dotnet/Corvus.ContentHandling/total?cache=false)](https://imm.endjin.com/api/imm/github/corvus-dotnet/Corvus.ContentHandling/total?cache=false)

Creates instances of types, and dispatches content to content handlers, identified by media-type-like content-type strings (e.g. `application/vnd.corvus.example`), with hierarchical fallback resolution. Built on `Microsoft.Extensions.DependencyInjection` [keyed services](https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection#keyed-services).

v5 is a ground-up rebuild of the library on keyed services. It preserves the v4 feature set and JSON wire format, with a new registration API and no runtime code generation. See [the v5 release notes](docs/ReleaseNotes/Corvus.ContentHandling.v5.md) and [ADR 0002](docs/adr/0002-rebuild-on-keyed-services.md) for details. Targets `net10.0`.

## Getting started

Register content, keyed by content type:

```csharp
services.AddContentHandling(content => content
    // Resolved from the container, with a DI lifetime:
    .AddSingletonContent<PdfRenderer>("application/vnd.corvus.renderer.pdf")
    .AddTransientContent<AuditRecord>()          // content type discovered from the type
    // Constructed by the serializer (no service dependencies):
    .AddSerializedContent<OrderCreated>()
    // A pre-built instance under an explicit key:
    .AddContentInstance("application/vnd.corvus.template.invoice", invoiceTemplate));
```

Content types are discovered from a `[ContentType("...")]` attribute, or from the legacy `static string RegisteredContentType` field convention used by earlier versions.

Resolve by content type — with **hierarchical fallback**: if `application/vnd.corvus.a.b.c+suffix` is not registered, `application/vnd.corvus.a.b+suffix` is tried, then `application/vnd.corvus.a+suffix` (the `+suffix` is preserved at every step, so distinct *roles* of one content type — `+mapper`, `+renderer`, `+template` — fall back independently):

```csharp
object? content = serviceProvider.GetContent("application/vnd.corvus.renderer.pdf.v2");
// falls back to the "application/vnd.corvus.renderer.pdf" registration

PdfRenderer renderer = serviceProvider.GetRequiredContent<PdfRenderer>("application/vnd.corvus.renderer.pdf.v2");
```

Register handlers — delegates or classes — and dispatch payloads to them by content type
(fallback applies here too):

```csharp
services.AddContentHandling(content => content
    .AddContentHandler<OrderEvent, OrderCreated>("audit", order => Log(order))
    .AddContentHandler<OrderEvent, OrderCancelled, CancellationHandler>("audit"));

IContentDispatcher<OrderEvent> dispatcher = serviceProvider.GetRequiredService<IContentDispatcher<OrderEvent>>();
await dispatcher.DispatchAsync(orderEvent, "audit");
```

`Corvus.ContentHandling.Json` adds content-type-discriminated polymorphic serialization for System.Text.Json, and `ContentEnvelope` for sending heterogeneous payloads through a single channel:

```csharp
services.AddContentTypeBasedJsonSerializationSupport();
services.AddPolymorphicContentTarget<IOrderEvent>();

// {"contentType":"application/vnd.corvus...","...":...} round-trips via IOrderEvent
IOrderEvent? evt = JsonSerializer.Deserialize<IOrderEvent>(json, options);

// The envelope wire format: { "contentType": ..., "payload": ... }
var envelope = ContentEnvelope.FromPayload(evt, options);
bool handled = envelope.Match()
    .When<OrderCreated>(HandleCreated)
    .When<OrderCancelled>(HandleCancelled)
    .Else(e => HandleUnknown(e))
    .Execute();
```

Content that needs constructor-injected services can also be deserialized polymorphically (the instance is resolved from the container, then populated from the JSON). Such content must be registered with `AddTransientContent` — deserialization populates the resolved instance, so a shared singleton/scoped instance would be overwritten by every payload, and the converter rejects those lifetimes with a clear error.

## Performance: v4 vs v5

Two mirrored BenchmarkDotNet projects measure identical scenarios against v4 and v5 — same runtime (net10.0), same container version, only the library differs. Headlines (13th Gen Intel Core i7-13800H, .NET 10.0.9; full methodology and tables in [the v4 vs v5 comparison](docs/benchmarks-v4-vs-v5.md)):

| Scenario                                |                    v4 |                   v5 |                                            |
|-----------------------------------------|----------------------:|---------------------:|--------------------------------------------|
| Register 5 lambda handlers              | 138,190 µs / 8,597 KB |    1.72 µs / 9.66 KB | **~80,000× faster, ~900× less allocation** |
| 3-hop hierarchical fallback resolution  |      105.3 ns / 368 B |       98.2 ns / 24 B | allocation-free probing                    |
| Lambda-handler dispatch                 |       40.1 ns / 136 B |        39.8 ns / 0 B | **allocation-free**                        |
| Fallback dispatch                       |      131.7 ns / 464 B |        50.5 ns / 0 B | 2.6× faster, allocation-free               |
| Deserialize (serializer-constructed)    |    864.7 ns / 1,256 B |     722.3 ns / 256 B | 5× less allocation                         |
| Deserialize (DI-constructed + populate) | 8,440.6 ns / 10,131 B |     514.2 ns / 264 B | **16× faster, 38× less allocation**        |
| Envelope round-trip                     |  1,591.2 ns / 2,280 B | 1,463.2 ns / 2,160 B | wire-format-identical                      |

The registration row is the structural win: v4 compiled a unique wrapper type with Roslyn at runtime for every lambda handler registration (~27 ms and ~1.7 MB *each*); v5 registers keyed instances of a few closed generic adapters, and `Microsoft.CodeAnalysis.CSharp` is gone from the dependency graph entirely. Dispatch is allocation-free on every singleton-handler path, and the DI-populate deserialization path caches its serialization contracts instead of rebuilding them per call. v4 keeps a small (< 25 ns) edge on exactly two scenarios — typed resolution and transient class-handler dispatch — both inherent to the keyed-service indirection.

```
dotnet run -c Release --project Solutions/Corvus.ContentHandling.Benchmarks -- --filter '*'
```

## Building and testing

```
dotnet build Solutions/Corvus.ContentHandling.slnx
dotnet test --solution Solutions/Corvus.ContentHandling.slnx
```

Requires the .NET 10 SDK. The test projects run on
[Microsoft.Testing.Platform](https://learn.microsoft.com/en-us/dotnet/core/testing/microsoft-testing-platform-intro)
(opted in via `global.json`).

## Licenses

[![GitHub license](https://img.shields.io/badge/License-Apache%202-blue.svg)](https://raw.githubusercontent.com/corvus-dotnet/Corvus.ContentHandling/main/LICENSE)

Corvus.ContentHandling is available under the Apache 2.0 open source license.

## Project Sponsor

This project is sponsored by [endjin](https://endjin.com), a UK based Microsoft Gold Partner for Cloud Platform, Data Platform, Data Analytics, DevOps, and a Power BI Partner.

For more information about our products and services, or for commercial support of this project, please [contact us](https://endjin.com/contact-us). 

We produce two free weekly newsletters; [Azure Weekly](https://azureweekly.info) for all things about the Microsoft Azure Platform, and [Power BI Weekly](https://powerbiweekly.info).

Keep up with everything that's going on at endjin via our [blog](https://blogs.endjin.com/), follow us on [Twitter](https://twitter.com/endjin), or [LinkedIn](https://www.linkedin.com/company/1671851/).

Our other Open Source projects can be found on [GitHub](https://endjin.com/open-source)

## Code of conduct

This project has adopted a code of conduct adapted from the [Contributor Covenant](http://contributor-covenant.org/) to clarify expected behavior in our community. This code of conduct has been [adopted by many other projects](http://contributor-covenant.org/adopters/). For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [&#104;&#101;&#108;&#108;&#111;&#064;&#101;&#110;&#100;&#106;&#105;&#110;&#046;&#099;&#111;&#109;](&#109;&#097;&#105;&#108;&#116;&#111;:&#104;&#101;&#108;&#108;&#111;&#064;&#101;&#110;&#100;&#106;&#105;&#110;&#046;&#099;&#111;&#109;) with any additional questions or comments.

## IP Maturity Matrix (IMM)

The IMM is endjin's IP quality framework.

[![Shared Engineering Standards](https://imm.endjin.com/api/imm/github/corvus-dotnet/Corvus.ContentHandling/rule/74e29f9b-6dca-4161-8fdd-b468a1eb185d?nocache=true)](https://imm.endjin.com/api/imm/github/corvus-dotnet/Corvus.ContentHandling/rule/74e29f9b-6dca-4161-8fdd-b468a1eb185d?cache=false)

[![Coding Standards](https://imm.endjin.com/api/imm/github/corvus-dotnet/Corvus.ContentHandling/rule/f6f6490f-9493-4dc3-a674-15584fa951d8?cache=false)](https://imm.endjin.com/api/imm/github/corvus-dotnet/Corvus.ContentHandling/rule/f6f6490f-9493-4dc3-a674-15584fa951d8?cache=false)

[![Executable Specifications](https://imm.endjin.com/api/imm/github/corvus-dotnet/Corvus.ContentHandling/rule/bb49fb94-6ab5-40c3-a6da-dfd2e9bc4b00?cache=false)](https://imm.endjin.com/api/imm/github/corvus-dotnet/Corvus.ContentHandling/rule/bb49fb94-6ab5-40c3-a6da-dfd2e9bc4b00?cache=false)

[![Code Coverage](https://imm.endjin.com/api/imm/github/corvus-dotnet/Corvus.ContentHandling/rule/0449cadc-0078-4094-b019-520d75cc6cbb?cache=false)](https://imm.endjin.com/api/imm/github/corvus-dotnet/Corvus.ContentHandling/rule/0449cadc-0078-4094-b019-520d75cc6cbb?cache=false)

[![Benchmarks](https://imm.endjin.com/api/imm/github/corvus-dotnet/Corvus.ContentHandling/rule/64ed80dc-d354-45a9-9a56-c32437306afa?cache=false)](https://imm.endjin.com/api/imm/github/corvus-dotnet/Corvus.ContentHandling/rule/64ed80dc-d354-45a9-9a56-c32437306afa?cache=false)

[![Reference Documentation](https://imm.endjin.com/api/imm/github/corvus-dotnet/Corvus.ContentHandling/rule/2a7fc206-d578-41b0-85f6-a28b6b0fec5f?cache=false)](https://imm.endjin.com/api/imm/github/corvus-dotnet/Corvus.ContentHandling/rule/2a7fc206-d578-41b0-85f6-a28b6b0fec5f?cache=false)

[![Design & Implementation Documentation](https://imm.endjin.com/api/imm/github/corvus-dotnet/Corvus.ContentHandling/rule/f026d5a2-ce1a-4e04-af15-5a35792b164b?cache=false)](https://imm.endjin.com/api/imm/github/corvus-dotnet/Corvus.ContentHandling/rule/f026d5a2-ce1a-4e04-af15-5a35792b164b?cache=false)

[![How-to Documentation](https://imm.endjin.com/api/imm/github/corvus-dotnet/Corvus.ContentHandling/rule/145f2e3d-bb05-4ced-989b-7fb218fc6705?cache=false)](https://imm.endjin.com/api/imm/github/corvus-dotnet/Corvus.ContentHandling/rule/145f2e3d-bb05-4ced-989b-7fb218fc6705?cache=false)

[![Date of Last IP Review](https://imm.endjin.com/api/imm/github/corvus-dotnet/Corvus.ContentHandling/rule/da4ed776-0365-4d8a-a297-c4e91a14d646?cache=false)](https://imm.endjin.com/api/imm/github/corvus-dotnet/Corvus.ContentHandling/rule/da4ed776-0365-4d8a-a297-c4e91a14d646?cache=false)

[![Framework Version](https://imm.endjin.com/api/imm/github/corvus-dotnet/Corvus.ContentHandling/rule/6c0402b3-f0e3-4bd7-83fe-04bb6dca7924?cache=false)](https://imm.endjin.com/api/imm/github/corvus-dotnet/Corvus.ContentHandling/rule/6c0402b3-f0e3-4bd7-83fe-04bb6dca7924?cache=false)

[![Associated Work Items](https://imm.endjin.com/api/imm/github/corvus-dotnet/Corvus.ContentHandling/rule/79b8ff50-7378-4f29-b07c-bcd80746bfd4?cache=false)](https://imm.endjin.com/api/imm/github/corvus-dotnet/Corvus.ContentHandling/rule/79b8ff50-7378-4f29-b07c-bcd80746bfd4?cache=false)

[![Source Code Availability](https://imm.endjin.com/api/imm/github/corvus-dotnet/Corvus.ContentHandling/rule/30e1b40b-b27d-4631-b38d-3172426593ca?cache=false)](https://imm.endjin.com/api/imm/github/corvus-dotnet/Corvus.ContentHandling/rule/30e1b40b-b27d-4631-b38d-3172426593ca?cache=false)

[![License](https://imm.endjin.com/api/imm/github/corvus-dotnet/Corvus.ContentHandling/rule/d96b5bdc-62c7-47b6-bcc4-de31127c08b7?cache=false)](https://imm.endjin.com/api/imm/github/corvus-dotnet/Corvus.ContentHandling/rule/d96b5bdc-62c7-47b6-bcc4-de31127c08b7?cache=false)

[![Production Use](https://imm.endjin.com/api/imm/github/corvus-dotnet/Corvus.ContentHandling/rule/87ee2c3e-b17a-4939-b969-2c9c034d05d7?cache=false)](https://imm.endjin.com/api/imm/github/corvus-dotnet/Corvus.ContentHandling/rule/87ee2c3e-b17a-4939-b969-2c9c034d05d7?cache=false)

[![Insights](https://imm.endjin.com/api/imm/github/corvus-dotnet/Corvus.ContentHandling/rule/71a02488-2dc9-4d25-94fa-8c2346169f8b?cache=false)](https://imm.endjin.com/api/imm/github/corvus-dotnet/Corvus.ContentHandling/rule/71a02488-2dc9-4d25-94fa-8c2346169f8b?cache=false)

[![Packaging](https://imm.endjin.com/api/imm/github/corvus-dotnet/Corvus.ContentHandling/rule/547fd9f5-9caf-449f-82d9-4fba9e7ce13a?cache=false)](https://imm.endjin.com/api/imm/github/corvus-dotnet/Corvus.ContentHandling/rule/547fd9f5-9caf-449f-82d9-4fba9e7ce13a?cache=false)

[![Deployment](https://imm.endjin.com/api/imm/github/corvus-dotnet/Corvus.ContentHandling/rule/edea4593-d2dd-485b-bc1b-aaaf18f098f9?cache=false)](https://imm.endjin.com/api/imm/github/corvus-dotnet/Corvus.ContentHandling/rule/edea4593-d2dd-485b-bc1b-aaaf18f098f9?cache=false)

[![OpenChain](https://imm.endjin.com/api/imm/github/corvus-dotnet/Corvus.ContentHandling/rule/66efac1a-662c-40cf-b4ec-8b34c29e9fd7?cache=false)](https://imm.endjin.com/api/imm/github/corvus-dotnet/Corvus.ContentHandling/rule/66efac1a-662c-40cf-b4ec-8b34c29e9fd7?cache=false)
