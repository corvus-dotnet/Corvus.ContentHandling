# Use of System.Text.Json

## Status

Accepted

## Context

Since the General Availability of the `System.Text.Json` high-performance, low-allocation JSON parsing and writing library as part of the standard libraries on all LTS versions of the dotnet framework (including net47, netstandard2.0, netstandard2.1 and netcore3.1), various MS-supplied libraries (including the new Cosmos DB SDK and AspNetCore) have migrated to its use.

We have already created Corvus.Extensions.System.Text.Json with support for our standard low-level JSON patterns and this considers adding System.Text.Json supprt to Corvus.ContentHandling (which would then allow us to propagate that use through the rest of the Corvus, Menes and Marain IP.)

Unfortunately, `System.Text.Json` as currently implemented has no support for the dynamic creation of polymorphic types which is what is required for the ContentHandling use case. The existing examples require a custom Converter for any type participating in polymorphic conversion, and have no equivalent to Newtonsoft's `JsonSerializer.Populate()` method allowing you to use the default code means of deserialization having discovered and created an instance of a type yourself. This is a showstopper. However, a short spike revealed that it would be fairly easy to add in an efficient manner, and there are, in fact, comments that point to the fact that this is considered for a future addition to the codebase.

The other feature required for type-discriminator-based instantiation is a "read ahead" to find the property concerned. This could easily be achieved by buffering into a `JsonElement` and disposing of it, but it is clear from inspecting the code that a buffered read-ahead for this could be more efficiently implemented "inside the box" as opposed to the current "read it into a `JsonDocument` and then re-parse" approach.

## Decision

Do not attempt to implement this or spend time creating a PR for `System.Text.Json`.

## Consequences

We will likely need to invest in wrappers around the Newtonsoft.Json implementation like the one we have for the Cosmos DB library. I imagine support for Newtonsoft in core libraries will remain for at least another version but this is a guess. The aspnetcore team in particular have a history of making large breaking changes (that they sometimes have to reverse) which could preclude a jump to e.g. 4.0 but if we retain our "version behind" policy with that technology, this should not affect us too badly.