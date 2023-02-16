# Release notes for Corvus.ContentHandling v4

## V4.0

Uses `System.Text.Json`. (Earlier versions used `Newtonsoft.Json`.) Targets .NET 6.0.

There are no changes to the core `Corvus.ContentHandling` library because that does not depend on any particular JSON serialization technology. (In fact it is not specific to JSON at all.) The changes are all in `Corvus.ContentHandling.Json`.

Breaking change:
* Serialization and deserialization for content type pattern no longer supported for `Newtonsoft.Json` (that's the main point of this version, so use v3.0 if you still need that)
* .NET 6.0 minimum (.NET 7.0 for some features)
* Nullable reference types now supported throughout
* The `ContentEnvelope` no longer supplies default JSON serialization settings
* Polymorphic deserialization driven by content type requires different registration
* Dependency injection during deserialization not available prior to .NET 7.0
* Fixed a typo: a few methods called `DispatchPayloadToHanderAsync` now have the missing `l` reinstated to match all the other overloads

More detail:

### `ContentEnvelope` and default settings

You have always been  able to construct a `ContentEnvelope` without supplying either content or JSON serialization settings. Part of the reason for this is that the type offers a `SetPayload` method enabling the JSON data to be supplied later. An unfortunate feature of the previous versions of `Corvus.ContentHandling.Json` is that it tolerated the failure to supply JSON serialization settings—it would supply default settings if you didn't bring it any. This was almost always a mistake, because it meant that it wouldn't have access to any of the converters or settings you wanted to use.

Since most of the things you might want to do with a `ContentEnvelope` involve working with the JSON it contains, in practice you almost always want to plug in suitable serialization settings. The fact that it used not to require this was a source of subtle bugs.

So as of v4.0, the `ContentEnvelope` will not permit any operations whose behaviour is affected by serialization configuration unless you have supplied `JsonSerializerOptions` (which are the `System.Text.Json` equivalent of the old `JsonSerializerSettings`). It still allows you to construct an instance without supplying this, because common use patterns include constructing the thing and then supplying settings later. But if you use any of the operations whose behaviour is affected by these options (`TryGetPayload<T>`, `GetContents<T>`, and the overload of `SetPayload<T>` that doesn't have an options argument) without having previously supplied a `JsonSerializerOptions`, you will get an `InvalidOperationException`.

Code that really wants the old behaviour, in which the serializer behaviour is simply the defaults, just needs to be explicit about that, by supplying a suitable `JsonSerializerOptions` instance during or after construction. Since failure to supply options was likely to be a mistake, is is better that they always be supplied explicitly in scenarios where behaviour depends on them.

### Polymorphic deserialization changes

With v3, any types that you had registered were available for polymorphic deserialization to any compatible target type, but due to limitations in the current `System.Text.Json`, we can't offer quite that level of flexibility. (It was always a bit of a hack before, and the slightly unpleasant trick we were using to make it work doesn't work on `System.Text.Json`.)

It is now necessary to register the target types into which some set of content-type-discriminated concrete types might be deserializer. For example, if you have this interface:

```csharp
public interface ICommon
{
    string Name { get; }
}
```

and a couple of implementations conforming to the content factory pattern:

```csharp
public class C1 : ICommon
{
    public const string RegisteredContentType = "application/vnd.corvus.test.c1";
    public string ContentType => RegisteredContentType;

    public string Name => "Foo"
}

public class C2 : ICommon
{
    public const string RegisteredContentType = "application/vnd.corvus.test.c2";
    public string ContentType => RegisteredContentType;

    public string Name => "Bar"
}
```

all you needed to do was register both content types:

```csharp
contentFactory.RegisterContent<C1>();
contentFactory.RegisterContent<C2>();
```

If you then have a class like this:

```csharp
public class Data
{
    public ICommon Dynamic { get; set; }
}
```

and  source data like this:

```json
{
    "contentType": "application/vnd.corvus.test.c2"
}
```

deserializing that data would correctly work out that it needed to construct an instance of the `C2` class to populate the `Data.Dynamic` property. (It works that out from the `contentType`.)

As of `Corvus.ContentHandling.Json` v4.0, you need to do one more thing: you have to state which target types require polymorphic deserialization. So in this example, if we want the `Data.Dynamic` property to get this `contentType`-driven type selection, we need to enable that for serialization targets of type `ICommon`:

```csharp
contentFactory.RegisterPolymorphicContentTarget<ICommon>();
contentFactory.RegisterContent<C1>();
contentFactory.RegisterContent<C2>();
```

The basic reason for this is that we're using a custom JSON converter to enable this behaviour, and that's not really what they are designed to do. (This was also true on `Newtonsoft.Json`, but we were able to use a slightly more comprehensive albeit rather smelly workaround.) Custom converters are meant for scenarios where we want to completely replace the built-in deserialization behaviour. But what we actually want to do here is use the built-in behaviour, just with the ability to select types at runtime.

Prior to .NET 7, there was no support for polymorphism, so a hack based on custom converters is the only available option if we want to support .NET 6.0. And due to some performance optimizations that `System.Text.Json` performs that `Newtonsoft.Json` did not, there are some constraints on what can be achieved, leading to the requirement to state explicitly which target types require polymorphism.

There is an additional constraint: a polymorphic target type cannot itself be deserialized. Consider this hierarchy:

```csharp
public class Base
{
    public const string RegisteredContentType = "application/vnd.corvus.test.b";
    public string ContentType => RegisteredContentType;
}
public class Derived1 : Base
{
    public const string RegisteredContentType = "application/vnd.corvus.test.d1";
    public string ContentType => RegisteredContentType;
}
public class Derived2 : Base
{
    public const string RegisteredContentType = "application/vnd.corvus.test.d2";
    public string ContentType => RegisteredContentType;
}
```

We can register these for polymorphic use:

```csharp
contentFactory.RegisterPolymorphicContentTarget<Base>();
contentFactory.RegisterContent<Derived1>();
contentFactory.RegisterContent<Derived2>();
```

But what we can't do is attempt to deserialize an instance of the base type. With v3, we could have had this source data:

```json
{
    "contentType": "application/vnd.corvus.test.b"
}
```

and have that deserialize to an instance of `Base`. But with v4, that's not possible. The JSON converter approach we've used to enable polymorphism on .NET 6.0 would end up crashing in infinite recursion. (The smelly hack referred to above in v3 was a nasty workaround to this. The same trick doesn't work in `System.Text.Json`.)

If your targets for polymorphic deserialization are either interfaces or abstract base classes, this isn't a limitation in practice. (And if you just want to deserialize directly to a known type without polymorphism, there's also no problem.)

In principle, we might be able to go back to the old model when targeting .NET 7.0, but arguably the old behaviour was inappropriately global: it became possible for types to pop out in places you weren't expecting (e.g., any `object` target). If this turns out to be important, we could revisit reinstating support for this on .NET 7.0; I don't think it's possible on .NET 6.0.


## Injecting dependencies during content-type-driven deserialization

`Corvus.ContentHandling.Json` has long supported dependency-injection-based initialization of content. This has sometimes been used to enable serializable types to have access to JSON serialization settings. In practice this is a somewhat problematic approach—types that are essentially DTOs shouldn't need dependencies. And it was not a good solution to the serialization settings problem, because DTOs shouldn't be dictating to an application how they should be serialized—this is the tail wagging the dog.

It turns out that in .NET 6.0, `System.Text.Json` doesn't have any way to deserialize data into an existing instance. (`Newtonsoft.Json` offers a `Populate` method for this style of deserialization, but there's no `System.Text.Json` equivalent.) .NET 7.0 doesn't directly offer such a mechanism either, but the changes they have made as part of the type discrimination support happen to have opened up a way of doing it. (The new `DefaultJsonTypeInfoResolver` gives us a way to obtain a new `JsonTypeInfo<T>` for any type, and it lets us plug in an object creation factory method, at which point we can just supply the object we already had to hand.) So we are able to support this feature on .NET 7.0 or later.