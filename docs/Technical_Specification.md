# Corvus.ContentHandling Technical Specification

## Overview

Corvus.ContentHandling is a .NET library providing a content-type-based factory pattern and polymorphic message handling system. This specification documents the complete architecture of the library.

## Architecture Overview

### Design Philosophy
- **Content-Type Driven**: All types are identified by content-type strings (e.g., "application/vnd.corvus.example")
- **Hierarchical Content Types**: Support for content type inheritance through dot notation (e.g., "application/vnd.corvus.type.subtype")
- **Fallback Resolution**: Automatic fallback to parent content types when specific types aren't registered
- **Polymorphic Dispatch**: Base type handlers can process derived type payloads
- **Dual Pattern Support**: Both object-oriented (interface) and functional (delegate) handler patterns
- **DI Integration**: Deep integration with Microsoft.Extensions.DependencyInjection

### Core Libraries
1. **Corvus.ContentHandling**: Core factory and handler dispatch system
2. **Corvus.ContentHandling.Json**: JSON serialization support with ContentEnvelope
3. **Test Projects**: BDD specs using ReqnRoll (formerly SpecFlow)

## Core Components

### Corvus.ContentHandling Project

#### Core Registry and Types

**ContentFactory**
- **Location**: `Corvus/ContentHandling/ContentFactory.cs`
- **Purpose**: Central registry for content types and their implementations
- **Key Members**:
  - `ContentTypes`: ConcurrentDictionary mapping content-type strings to RegisteredContentType
  - `GetContentType()`: Extract content type from types with RegisteredContentType field
  - `TryGetContentType()`: Type resolution with service dependency information
  - `AddTypeRequiringServices()`: Register types needing DI
  - `AddSimpleDeserializableType()`: Register types for direct deserialization
  - Internal `Services` property: Exposes IServiceCollection for registration
  - Instance property fallback: Checks "ContentType" instance property before static field
  - `GetRegisteredContentTypes()`: Internal method returning all content type mappings

**MediaType**
- **Location**: `Corvus/ContentHandling/MediaType.cs`
- **Purpose**: Immutable struct representing media types with optional syntax suffix
- **Format**: `typeAndSubtype+structuredSyntaxSuffix`
- **Key Features**:
  - Parent type resolution via `GetParent()` - removes last segment after final dot
  - Preserves structured syntax suffix during parent resolution
  - Explicit string conversions (both directions)
  - `None` singleton for null object pattern
  - Hierarchical content type support

#### Handler Interfaces
- **Location**: `Corvus/ContentHandling/` (root of ContentHandling folder)

**Synchronous Handlers**:
- `IContentHandler<TPayloadType>`: Basic handler with `void Handle(TPayloadType)`
- `IContentHandler<TPayloadType, T1>`: Handler with one parameter
- `IContentHandler<TPayloadType, T1, T2>`: Handler with two parameters
- `IContentHandler<TPayloadType, T1, T2, T3>`: Handler with three parameters

**Asynchronous Handlers**:
- `IAsyncContentHandler<TPayloadType>`: Returns `Task`
- `IAsyncContentHandler<TPayloadType, T1>`: With one parameter
- `IAsyncContentHandler<TPayloadType, T1, T2>`: With two parameters
- `IAsyncContentHandler<TPayloadType, T1, T2, T3>`: With three parameters

**Result-Returning Handlers**:
- `IContentHandlerWithResult<TPayloadType, TResult>`: Returns `TResult`
- `IContentHandlerWithResult<TPayloadType, T1, TResult>`: With one parameter
- `IContentHandlerWithResult<TPayloadType, T1, T2, TResult>`: With two parameters
- `IContentHandlerWithResult<TPayloadType, T1, T2, T3, TResult>`: With three parameters

**Async Result-Returning Handlers**:
- `IAsyncContentHandlerWithResult<TPayloadType, TResult>`: Returns `Task<TResult>`
- `IAsyncContentHandlerWithResult<TPayloadType, T1, TResult>`: With one parameter
- `IAsyncContentHandlerWithResult<TPayloadType, T1, T2, TResult>`: With two parameters
- `IAsyncContentHandlerWithResult<TPayloadType, T1, T2, T3, TResult>`: With three parameters

**Dispatcher Interface**:
- `IContentHandlerDispatcher<TPayloadBaseType>`: Interface for dispatching to handlers

#### Internal Implementation Components
- **Location**: `Corvus/ContentHandling/Internal/`

**ContentHandlerDispatcher**
- **File**: `ContentHandlerDispatcher{TPayloadBase}.cs`
- **Purpose**: Routes payloads to registered handlers based on content type
- **Key Methods**:
  - `DispatchPayloadToHandler()`: Multiple overloads for sync/async with 0-3 parameters
  - Uses `GetRequiredContent<T>()` extension for handler resolution
  - Content type resolution via `ContentHandlerUtilities.GetHandlerContentType()`
  - Automatic content type detection when not explicitly provided

**ContentHandlerGenerator**
- **File**: `ContentHandlerGenerator.cs`
- **Purpose**: Runtime code generation for unique handler types using Roslyn
- **Features**: Template-based code generation, assembly caching, AssemblyLoadContext integration

**ContentHandlerUtilities**
- **File**: `ContentHandlerUtilities.cs`
- **Purpose**: Simple utility for handler content type formatting
- **Method**: `GetHandlerContentType()` - combines payload and handler class

**Function Adapter Implementations**:
- `ContentHandlerWithAction<TPayloadBase, TPayload>`: Wraps `Action<TPayload>`
- `ContentHandlerWithAction<TPayloadBase, TPayload, T1>`: With one parameter
- `ContentHandlerWithAction<TPayloadBase, TPayload, T1, T2>`: With two parameters
- `ContentHandlerWithAction<TPayloadBase, TPayload, T1, T2, T3>`: With three parameters
- `AsyncContentHandlerWithAction<...>`: Async variants wrapping `Func<..., Task>`
- `ContentHandlerWithResultAndAction<...>`: Result-returning variants
- `AsyncContentHandlerWithResultAndAction<...>`: Async result-returning variants

**Class Wrapper Implementations**:
- `ContentHandlerWithClass<TPayloadBase, TPayload, THandler>`: Wraps handler classes
- `ContentHandlerWithClass<TPayloadBase, TPayload, THandler, T1>`: With one parameter
- `ContentHandlerWithClass<TPayloadBase, TPayload, THandler, T1, T2>`: With two parameters
- `ContentHandlerWithClass<TPayloadBase, TPayload, THandler, T1, T2, T3>`: With three parameters
- `AsyncContentHandlerWithClass<...>`: Async variants
- `ContentHandlerWithResultAndClass<...>`: Result-returning variants
- `AsyncContentHandlerWithResultAndClass<...>`: Async result-returning variants

**Internals Configuration**:
- **File**: `Internals.cs`
- **Content**: `[assembly: InternalsVisibleTo("Corvus.ContentHandling.Json")]`

#### Extension Methods
- **Location**: `Corvus/ContentHandling/` (root of ContentHandling folder)

**ContentFactoryExtensions.cs**
- **Lines**: 674
- **Purpose**: Registration methods for content types
- **Key Methods**:
  - `RegisterContent<T>()`: Simple type registration
  - `RegisterSingletonContent<T>()`: DI-integrated singleton
  - `RegisterTransientContent<T>()`: DI-integrated transient
  - `RegisterScopedContent<T>()`: DI-integrated scoped
- **Features**: Content type strings, factory functions, instances, duplicate prevention

**ContentHandlerContentFactoryExtensions.cs**
- **Lines**: 1,277
- **Purpose**: Handler registration methods
- **Registration Patterns**:
  - Action-based: `RegisterContentHandler<TBase, TPayload>(Action<TPayload>, handlerClass)`
  - Class-based: `RegisterContentHandler<TBase, TPayload, THandler>(handlerClass)`
  - Factory-based: With `Func<IServiceProvider, THandler>`
- **Variants**: All combinations of sync/async, with/without results, 0-3 parameters

**ContentHandlerWithResultContentFactoryExtensions.cs**
- **Lines**: 1,329
- **Purpose**: Result-returning handler registration
- **Pattern**: Same as ContentHandlerContentFactoryExtensions but for result-returning handlers

#### Utility Extensions
- **Location**: `Corvus/ContentHandling/`

**MediaTypeDictionaryExtensions.cs**
- **Purpose**: Recursive lookup in MediaType-keyed dictionaries
- **Methods**:
  - `GetRecursive<TKey, TValue>()`: Throws if not found
  - `TryGetRecursive<TKey, TValue>()`: Returns bool
- **Implementation**: Uses `MediaType.GetParent()` for hierarchy traversal

#### Resources
- **Location**: `Corvus/ContentHandling/`

**Resources.Designer.cs** and **Resources.resx**
- **Purpose**: Auto-generated resource file for localized error messages

#### Service Collection Integration
- **Location**: `Microsoft/Extensions/DependencyInjection/`

**ContentFactoryServiceCollectionExtensions.cs**
- **Purpose**: Service collection extension for ContentFactory registration
- **Key Method**: `AddContent(Action<ContentFactory>? configure)`
  - Checks for existing ContentFactory registration
  - Creates new instance if not found
  - Registers as singleton
  - Also registers generic `IContentHandlerDispatcher<>`
  - Invokes configuration callback

**ContentFactoryServiceProviderExtensions.cs**
- **Purpose**: Service provider extensions for content retrieval
- **Key Methods**:
  - `GetContent<T>()` and `GetRequiredContent<T>()`: Type resolution with fallback
  - `GetAllContent<T>()`: Retrieve all types with specific suffix
- **Features**: Hierarchical fallback resolution, handles both DI and simple types

## Content Type Fallback Mechanism

### How Fallback Works
1. **Initial Lookup**: System first attempts exact match for requested content type
2. **Parent Resolution**: If not found, removes the last segment after the final dot
3. **Recursive Fallback**: Continues until match found or no more parent types exist
4. **Suffix Preservation**: Structured syntax suffixes (+json, +xml) are preserved during fallback

### Implementation
The fallback algorithm in `TryGetTypeFor` method:
1. Try exact content type match
2. If not found, extract structured syntax suffix
3. Remove last segment after final dot
4. Recursively try parent with preserved suffix
5. Stop when no more dots or match found

### Example
Request: `application/vnd.corvus.customer.premium.v2`
1. Try: `application/vnd.corvus.customer.premium.v2` (not found)
2. Try: `application/vnd.corvus.customer.premium` (not found)
3. Try: `application/vnd.corvus.customer` (found!)

### Corvus.ContentHandling.Json Project

#### Core JSON Components

**ContentEnvelope**
- **Location**: `Corvus/ContentHandling/Json/ContentEnvelope.cs`
- **Lines**: 1,618
- **Purpose**: Container for heterogeneous message types
- **Key Features**:
  - Wraps any payload with content type discriminator
  - `Match<T1, T2...>()` methods for type-safe dispatch (up to 5 types)
  - Integration with ContentFactory for type resolution
  - JSON serialization via JsonNode
  - Payload access methods:
    - `SetPayload<T>`: Serializes to JsonNode with content type
    - `TryGetPayload<T>`: Deserializes from JsonNode
    - `GetPayload<T>`: Throws if wrong type

#### Internal JSON Components
- **Location**: `Corvus/ContentHandling/Json/Internal/`

**ContentEnvelopeConverter**
- **File**: `ContentEnvelopeConverter.cs`
- **Purpose**: System.Text.Json converter for ContentEnvelope
- **Serialization Format**:
  ```json
  {
    "contentType": "application/vnd.example",
    "payload": { /* serialized content */ }
  }
  ```
- **Implementation**:
  - Read: Parses to JsonObject, extracts contentType and payload
  - Write: Outputs object with contentType string and payload JsonNode
  - Uses `ContentEnvelope.FromJson()` for deserialization
  - Direct `JsonNode.WriteTo()` for efficient payload serialization

**PolymorphicTargetConverter**
- **File**: `PolymorphicTargetConverter.cs`
- **Purpose**: Handles polymorphic deserialization with content type discrimination
- **Key Constraint**: Concrete type MUST differ from target type to prevent infinite recursion
- **Features**:
  - DI Support: Constructs instance via service provider with custom JsonTypeInfo manipulation
  - Non-DI Path: Direct deserialization for simple types
  - Caching: Reflection delegates cached in ConcurrentDictionary
  - Clear exception messages for constraint violations

#### ContentEnvelope Handler Wrappers
- **Location**: `Corvus/ContentHandling/Json/Internal/ContentEnvelopeHandler/`
- **Count**: 16 wrapper classes
- **Purpose**: Adapt ContentEnvelope handlers to automatically extract contents
- **Pattern**: Calls `envelope.GetContents<TContents>()` before forwarding to wrapped handler
- **Files**:
  - `ContentEnvelopeHandlerWithClass{TContents,THandler}.cs`
  - `ContentEnvelopeHandlerWithClass{TContents,THandler,T1}.cs`
  - `ContentEnvelopeHandlerWithClass{TContents,THandler,T1,T2}.cs`
  - `ContentEnvelopeHandlerWithClass{TContents,THandler,T1,T2,T3}.cs`
  - `AsyncContentEnvelopeHandlerWithClass{...}` (4 async variants)
  - `ContentEnvelopeHandlerWithResultAndClass{...}` (4 result variants)
  - `AsyncContentEnvelopeHandlerWithResultAndClass{...}` (4 async result variants)

#### JSON Extension Methods
- **Location**: `Corvus/ContentHandling/`

**JsonContentFactoryExtensions.cs**
- **Purpose**: Extension methods for JSON-specific registration
- **Key Method**: `RegisterPolymorphicContentTarget<T>()`
  - Registers `PolymorphicTargetConverter<T>` as singleton
  - For types that don't depend on services
  - Returns ContentFactory for method chaining

**ContentEnvelopeContentFactoryExtensions.cs**
- **Lines**: 577
- **Purpose**: Extension methods for ContentEnvelope handler registration

**ContentEnvelopeWithResultContentFactoryExtensions.cs**
- **Lines**: 574
- **Purpose**: Same pattern for result-returning envelope handlers

**ContractResolutionExtensions.cs**
- **Purpose**: Helper extensions for JSON contract resolution
- **Key Method**: `GetPredictedMemberName(MemberInfo, JsonSerializerOptions)`
  - Checks for JsonPropertyNameAttribute first
  - Falls back to PropertyNamingPolicy if no attribute
  - Returns unchanged member name if no naming policy

#### Service Collection Integration
- **Location**: `Microsoft/Extensions/DependencyInjection/`

**ContentHandlingJsonServiceCollectionExtensions.cs**
- **Purpose**: DI registration for JSON serialization support
- **Key Method**: `AddContentTypeBasedSerializationSupport()`
  - Calls `AddJsonSerializerOptionsProvider()` from Corvus.Json.Serialization
  - Registers ContentFactory singleton if not already registered
  - Registers ContentEnvelopeConverter as JsonConverter
  - Checks for existing registrations to avoid duplicates
- **Note**: No longer accepts callback for content registration - must call `services.AddContent()` separately

## Internal Implementation Details

### Runtime Code Generation
**Location**: `Internal/ContentHandlerGenerator.cs`

Dynamically compiles unique handler types at runtime using Roslyn.

**Key Features**:
- Creates handler types with unique GUIDs to avoid type conflicts
- Caches compiled assemblies in ConcurrentDictionary
- Loads assemblies into same AssemblyLoadContext
- Generates code for all handler variants
- Template-based code generation with string templates

**Compilation Process**:
1. Parse template with CSharpSyntaxTree
2. Create compilation with references
3. Emit to MemoryStream
4. Load into same AssemblyLoadContext

### ContentHandlerUtilities
**Location**: `Internal/ContentHandlerUtilities.cs`

Simple utility class with single static method.

**GetHandlerContentType**: Combines payload content type with handler class
- Format: `{payloadContentType}+{handlerClass.ToLowerInvariant()}`

### Resources
**Location**: `Resources.Designer.cs` and `Resources.resx`

Auto-generated resource file for localization of error messages.

### Internal Visibility
**Location**: `Internals.cs`

Single line: `[assembly: InternalsVisibleTo("Corvus.ContentHandling.Json")]`

Allows JSON library access to internal types.

## Utility Extensions

### MediaTypeDictionaryExtensions
**Location**: `MediaTypeDictionaryExtensions.cs`

Recursive lookup in MediaType-keyed dictionaries.

**Methods**:
- `GetRecursive<TKey, TValue>()`: Throws if not found
- `TryGetRecursive<TKey, TValue>()`: Returns bool

Uses `MediaType.GetParent()` to walk up hierarchy.

## Design Patterns

### Factory Pattern
- `ContentFactory` as central factory
- Type registration with content-type keys
- Service lifetime management (singleton/transient/scoped)

### Strategy Pattern
- Handler selection based on content type
- Runtime strategy selection via dispatcher

### Adapter Pattern
- Function-to-interface adapters
- Enables functional programming style

### Chain of Responsibility
- Content type hierarchy with parent resolution
- Fallback handler support

### Visitor Pattern
- `ContentEnvelope.Match()` methods
- Type-safe visitor dispatch

## Type System Architecture

### Generic Type Relationships
```
IContentHandler<TPayloadBase>
  ↑
ContentHandlerWithClass<TPayloadBase, TPayload, THandler>
  where TPayload : TPayloadBase
  where THandler : IContentHandler<TPayload>
```

### Content Type Resolution
1. Static field: `const string RegisteredContentType`
2. Instance property: `string ContentType { get; }`
3. Manual specification in registration

### Handler Class Naming
- Format: `{contentType}+{handlerClass}`
- Example: `"application/vnd.corvus.button+renderer"`

## Test Infrastructure

### Project Structure

#### Corvus.ContentHandling.Specs
- **Target Frameworks**: net8.0, net9.0
- **Test Framework**: NUnit with ReqnRoll
- **Dependencies**: Corvus.Testing.ReqnRoll.NUnit 4.0.3
- **Purpose**: Tests core ContentHandling functionality without JSON

#### Corvus.ContentHandling.Json.Specs
- **Target Frameworks**: net8.0, net9.0
- **Additional Dependencies**:
  - SystemTextJson.JsonDiffPatch 2.0.0 (for DeepEquals in tests)
  - System.Text.Json 8.0.5 (explicit version to address vulnerability)
- **Purpose**: Tests JSON serialization and polymorphic features

Both projects use package lock files for CI repeatability.

### Test Driver Types
**Location**: `Driver/` directory

**ContentType Classes** (ContentType1-6):
- Have RegisteredContentType const field
- Subtype variants demonstrate hierarchical content types
- Example: ContentType1.subtype1 extends ContentType1

**ExplicitType Classes** (ExplicitType1-6):
- No content type field
- Used to test explicit content type registration
- Subtype variants for fallback testing

**Helper Classes**:
- `Registration.cs`: Helper for test registration scenarios
- `TypeMap.cs`: String to Type mapping for test scenarios
- `RegistrationKind.cs`: Enum for DI lifetime selection

### Feature Files and Step Definitions

#### Corvus.ContentHandling.Specs

**RegisterContent.feature**
- **Location**: `Solutions/Corvus.ContentHandling.Specs/Features/RegisterContent.feature`
- **Purpose**: Tests content type registration and fallback mechanisms
- **Key Scenarios**:
  - Types with explicit content type registration
  - Types with implicit content type (from RegisteredContentType field)
  - Content type fallback from subtypes to parent types
  - Different DI lifetimes (Singleton, Transient, Scoped)

**RegisterContentSteps.cs**
- **Location**: `Solutions/Corvus.ContentHandling.Specs/Steps/RegisterContentSteps.cs`
- **Step Definitions**:
  - `Given I have registered the following types`: Table-driven registration
  - `When I get the following content`: Content retrieval by type
  - `Then the results should be of types`: Type validation
- **Uses**: Registration helper and TypeMap for flexible test scenarios

#### Corvus.ContentHandling.Json.Specs

**DeserializingContent.feature**
- **Location**: `Solutions/Corvus.ContentHandling.Json.Specs/DeserializingContent.feature`
- **Purpose**: Tests polymorphic deserialization scenarios
- **Key Scenarios**:
  - Interface targets (ISomeContentInterface)
  - Base class targets (SomeContentBase)
  - Abstract base targets (SomeContentAbstractBase)
  - Nested polymorphic objects
  - POC (Plain Old CLR) objects without content type
  - DI-initialized objects

**DeserializingContentSteps.cs**
- **Location**: `Solutions/Corvus.ContentHandling.Json.Specs/DeserializingContentSteps.cs`
- **Step Definitions**:
  - `When I deserialize the json object '(.*)' to the common interface as '(.*)'`
  - `When I deserialize the json object '(.*)' to the common base as '(.*)'`
  - `When I deserialize the json object '(.*)' to the common abstract base as '(.*)'`
  - `When I deserialize the json object '(.*)' as a poc object with dictionary as '(.*)'`
- **Uses**: IJsonSerializerOptionsProvider from service provider

**SerializingContent.feature**
- **Location**: `Solutions/Corvus.ContentHandling.Json.Specs/SerializingContent.feature`
- **Purpose**: Tests serialization of polymorphic content types
- **Key Scenarios**:
  - Interface-based polymorphism with/without child objects
  - Abstract base polymorphism with/without child objects
  - Concrete base polymorphism with/without child objects
  - POC objects with enums (serialized as integers)
  - Null child object handling

**SerializingContentSteps.cs**
- **Location**: `Solutions/Corvus.ContentHandling.Json.Specs/SerializingContentSteps.cs`
- **Step Definitions**:
  - `Given I have an instance of a content object called '(.*)' with content type '(.*)'`
  - `Given I have an instance of a content object called '(.*)' with content type '(.*)' available as a child object`
  - `When I serialize the content object called '(.*)'`
  - `Then the serialized result should be a json object '(.*)'`
- **Uses**: JsonDiffPatch.DeepEquals for comparison

### JSON Test Samples
**Location**: `Solutions/Corvus.ContentHandling.Json.Specs/Samples/`

Test types demonstrating various polymorphic serialization scenarios:
- **Interfaces**: ISomeContentInterface with ContentType property
- **Base Classes**: SomeContentBase (abstract) and SomeContentAbstractBase
- **Content Types**: Classes with RegisteredContentType const field
- **POC Objects**: Plain types without content type
- **DI Types**: SomeContentRequiringDiInitialization with constructor injection
- **Nested Polymorphism**: Types containing other polymorphic properties

All implement IEquatable for test assertions and follow "application/vnd.corvus.*" convention.

### Test Container Setup

**ContentHandlingContainerBindings**
- **Location**: `Solutions/Corvus.ContentHandling.Json.Specs/ContentHandlingContainerBindings.cs`
- **Purpose**: ReqnRoll test container setup for JSON specs
- **Features**:
  - BeforeFeature hook configures services
  - Calls `AddContentTypeBasedSerializationSupport()`
  - Registers sample content via `AddSampleContent()`
  - Tests duplicate registration guards

**SerializationSampleContentServiceCollectionExtensions**
- **Location**: `Solutions/Corvus.ContentHandling.Json.Specs/Samples/SerializationSampleContentServiceCollectionExtensions.cs`
- **Purpose**: Registers all JSON test sample types
- **Features**:
  - Registers polymorphic targets for interfaces and base classes
  - Registers concrete content types
  - Includes both DI and non-DI types

**SerializationSampleContentServiceCollectionExtensions**:
- `AddSampleContent()` extension registers all test types
- Registers polymorphic targets for interfaces and base classes
- Includes both DI and non-DI types

## Key Behaviors and Constraints

### Polymorphic Deserialization
- **System.Text.Json Limitation**: Target type MUST differ from concrete type
- **Workaround**: Complex JsonTypeInfo manipulation for DI-constructed types
- **Impact**: Requires interfaces/abstract bases for polymorphism

### DI-Aware Content Types
**Two-Path Deserialization**:
1. DI types: Constructed via service provider, then populated
2. Non-DI types: Standard System.Text.Json deserialization

**Trade-offs**: DI convenience vs nullable reference support

### ContentEnvelope Match Behavior
- No fallback - Match methods use exact content type comparison
- Sequential evaluation - First match wins
- Type-safe payload extraction via internal TryGetPayload

### JSON Serialization Behaviors
- **Enum Serialization**: Serializes as integers by default
- **Null Handling**: Null properties omitted from serialized JSON
- **Property Naming**: Uses camelCase via PropertyNamingPolicy
- **Nested Polymorphism**: Fully supported

### External Dependencies
- **Corvus.Json.Serialization**: Provides IJsonSerializerOptionsProvider
- **Microsoft.CodeAnalysis.CSharp**: For runtime code generation
- **System.Text.Json**: Core serialization framework

## Library Metrics

### Code Statistics
- **Core Library**: ~5,000 lines
- **JSON Library**: ~3,000 lines
- **Test Projects**: ~2,000 lines
- **Interfaces**: 16 handler interface variants
- **Extension Methods**: ~200 registration methods

### Complexity Hotspots
1. `ContentHandlerContentFactoryExtensions.cs`: 1,278 lines of similar patterns
2. `ContentEnvelope.cs`: 1,619 lines with repetitive Match methods
3. Handler wrapper classes: 16 similar implementations per library

## Refactoring Recommendations

### Simplification Opportunities
- **Handler Registration**: 1,278 lines of similar registration methods could use source generators
- **Match Methods**: Repetitive pattern up to 5 types could be generalized
- **Generic Constraints**: Inconsistent `notnull` constraints across similar types
- Replace runtime code generation with source generators
- Consolidate Match method implementations
- Standardize naming conventions

### Performance Optimizations
- **Type Caching**: Content type extraction uses reflection on every call
- **Handler Resolution**: String concatenation for handler content types
- **Concurrent Dictionary**: Consider frozen collections for read-heavy scenarios
- Implement type caching for content type resolution
- Optimize handler lookup with frozen dictionaries
- Add benchmarks for critical paths
- Consider removing runtime compilation overhead

### API Consistency
- **Naming**: Mix of "ContentType" and "PayloadContentType"
- **Parameter Order**: Inconsistent ordering of contentType vs handlerClass
- **Null Handling**: Mix of nullable reference types and null checks

### Modernization Opportunities
- Adopt nullable reference types consistently
- Use pattern matching for type dispatch
- Consider ValueTask for async operations
- Evaluate System.Text.Json v9 features for polymorphism
- Source generators for registration boilerplate
- Span<T> for performance-critical paths

### Migration Considerations

**Breaking Changes to Consider**:
1. Consolidate handler registration methods using generic constraints
2. Standardize content type property names
3. Remove legacy overloads

**Backward Compatibility**:
1. Maintain content type string format
2. Keep core interfaces stable
3. Preserve DI integration patterns

### Documentation Improvements
- Create architecture diagrams including code generation flow
- Add quick-start guides covering all scenarios
- Document migration from v3 to v4
- Document all limitations and workarounds
- XML comments are comprehensive but have some repetition
- Good inline examples in IContentHandlerDispatcher
- Could benefit from sequence diagrams

### Testing Enhancements
- Test organization with separate feature files works well
- Sample types provide good coverage but could be simplified
- Consider adding performance benchmarks