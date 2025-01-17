// <copyright file="PolymorphicTargetConverter.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Json.Internal
{
    using System;
    using System.Collections.Concurrent;
    using System.Reflection;
    using System.Text.Json;
    using System.Text.Json.Nodes;
    using System.Text.Json.Serialization;
    using System.Text.Json.Serialization.Metadata;

    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// A JSON converter enabling objects which conform to the content-type polymorphism pattern to
    /// be deserialized to a property of some interface or base type.
    /// </summary>
    /// <typeparam name="TTarget">
    /// Target for deserialization. Either an interface or a base of some types for which
    /// polymorphic deserialization is required.
    /// </typeparam>
    /// <remarks>
    /// <para>
    /// This provides support for deserialization for content registered using the
    /// <see cref="ContentFactory"/> mechanism, in which the concrete type deserialized is
    /// determined by looking at the content type in the source JSON.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// [<![CDATA[
    /// public class MyContent : SomeBase
    /// {
    ///     public const string RegisteredContentType = "application/vnd.corvus.example.mycontent"
    ///     public string SomeProperty { get; set; }
    ///     public string ContentType => RegisteredContentType;
    /// }
    /// public class MyContent2 : SomeBase
    /// {
    ///     public const string RegisteredContentType = "application/vnd.corvus.example.mycontent2"
    ///     public int DifferentProperty { get; set; }
    ///     public string ContentType => RegisteredContentType;
    /// }
    ///
    /// // By registering SomeBase as polymorphic, any registered derived types can be deserialized
    /// // to properties of type SomeBase
    /// contentFactory.RegisterPolymorphicContentTarget<SomeBase>();
    /// // We register the concrete content types as normal (just as we would even if we didn't
    /// // want polymorphic behaviour).
    /// contentFactory.RegisterContent<MyContent>();
    /// contentFactory.RegisterContent<MyContent2>();
    /// ]]>
    /// </code>
    /// </example>
    internal class PolymorphicTargetConverter<TTarget> : JsonConverter<TTarget>
        where TTarget : class
    {
        private static readonly ConcurrentDictionary<Type, Action<TTarget, JsonNode, JsonSerializerOptions>> DeserializeIntoInstanceCache = new();
        private static readonly DefaultJsonTypeInfoResolver JsonTypeInfoResolver = new();
        private static readonly Lazy<MethodInfo> InPlaceDeserializerGenericMethodInfo = new(() =>
        {
            Action<TTarget, JsonObject, JsonSerializerOptions> m = DeserializeIntoExistingInstanceCore<TTarget>;
            return m.Method.GetGenericMethodDefinition();
        });

        private readonly IServiceProvider serviceProvider;

        /// <summary>
        /// Creates a <see cref="PolymorphicTargetConverter{TTarget}"/>.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        public PolymorphicTargetConverter(
            IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        /// <inheritdoc/>
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(TTarget);
        }

        /// <inheritdoc/>
        public override TTarget? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }

            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException($"Expected object, was {reader.TokenType}");
            }

            var jo = (JsonObject)JsonNode.Parse(ref reader)!;

            if (!jo.TryGetPropertyValue("contentType", out JsonNode? contentTypeNode) || contentTypeNode is null)
            {
                throw new JsonException("Object must have contentType property");
            }

            string contentTypeName = contentTypeNode.GetValue<string>();
            if (!this.serviceProvider.TryGetTypeFor(contentTypeName, out Type? typeToCreate, out bool usesServices))
            {
                throw new InvalidOperationException($"The content for type {contentTypeName} has not been registered with the ContentFactory.");
            }

            // With polymorphic deserialization, we can't deserialize directly to the target
            // type. E.g., if we write this:
            //
            //  services.RegisterPolymorphicContent<MyBaseType>();
            //  services.RegisterContent<Derived>();
            //
            // and we have a property like this:
            //
            //  public MyBaseType TargetProp { get; set; }
            //
            // we can deserialize an instance of Derived into TargetProp, but not an instance
            // of MyBaseType. If we attempted that, we'd end up in an infinite loop here.
            // because this converter is in the middle of deserializing a MyBaseType, so when
            // we defer to System.Text.Json, it's going to notice that we registered a converter
            // for MyBaseType (this converter!) and it will just call us back again. There is no
            // way to tell it "actually, we want to fall back to your default handling please".
            // This limitation arises from using a JsonConverter as a type discriminator - this
            // isn't really what converters are designed for. They're really for when you don't
            // want the built-in deserialization behaviour at all for a particular type. The
            // expectation is that if you're supplying a converter, you want to deserialize the
            // data yourself.
            // The old Newtonsoft implementation did actually allow polymorphic target types to
            // be deserialized, because we were able to work around the infinite recurseion. It
            // set a thread-local flag to detect this kind of recursion, and just declined the
            // opportunity to deserialize on the second attempt. But System.Text.Json doesn't offer
            // a way to do that. Whereas Newtonsoft asks us "is this yours?" for each individual
            // instance, System.Text.Json asks us once per type and remembers the result. So once
            // it knows we're the converter for MyBaseType, it will always use us without asking
            // again.
            if (typeToCreate == typeof(TTarget))
            {
                throw new NotSupportedException($"When using polymorphic contentType-driven deserialization, the concrete type must be different from the target type. (Here, both are {typeToCreate.Name}.) Consider using an interface or abstract base class as the target type.");
            }

            if (usesServices)
            {
                // Some content type implementation types depend on services, and must therefore be
                // constructed through DI. The downside of this is that constructor-based property
                // initialization is not available, which makes clean support for nullable
                // references a pain. It is up to consuming code to decide whether the convenience
                // of being able to deserialize directly into a type that also receive services
                // from DI outweights the pain.
                // Note that Newtonsoft.Json had built-in support for deserializing into an existing
                // instance (the JsonSerializer.Populate method). Although .NET 8.0 made changes to
                // address some of the scenarios this supported:
                //  https://github.com/dotnet/runtime/issues/78556
                // it's not a direct equivalent to what you Newtonsoft.Json offered: it seems to
                // let you deserialize into existing collections in properties, but there still
                // doesn't seem to be a way to say "I already have this object, please just deserialize
                // into it."
                // So we have to do it ourselves by first constructing the content via the service,
                // provider so it gets initialized by DI as required, then jumping through some
                // hoops to get System.Text.Json to deserialize into that.
                var result = (TTarget)this.serviceProvider.GetRequiredContent(contentTypeName);
                return DeserializeIntoInstance(typeToCreate, result, jo, options);
            }
            else
            {
                // This content type's implementing type doesn't depend on anything from DI, so
                // we want to get System.Text.Json to do the work for us.
                // The cast here might look odd, but we can't use the generic version of
                // Deserialize here that would normally avoid this. It's vital that we supply the
                // concrete derived type (typeToCreate) at this point so that System.Text.Json will
                // handle things from here. If we used Deserialize<TTarget>, System.Text.Json would
                // just invoke this converter again recursively.
                return (TTarget?)jo.Deserialize(typeToCreate, options);
            }
        }

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, TTarget value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }

        private static TTarget DeserializeIntoInstance(Type instanceType, TTarget instance, JsonNode json, JsonSerializerOptions options)
        {
            // Since System.Text.Json does not (as of .NET 9.0) have a direct equivalent to Newtonsoft's
            // Populate method, getting System.Text.Json to deserialize into an instance that we
            // have already constructed is a little tricky.
            //
            // This works by getting a JsonTypeInfo<T> for the target concrete type from a
            // DefaultJsonTypeInfoResolver and then adjusting its CreateObject to return our
            // instance instead of constructing a new object.
            //
            // Annoyingly, there's no overload of JsonSerializer.Deserialize that
            // accepts the non-generic base JsonTypeInfo type, so we are required to invoke
            // the generic JsonSerializer.Deserialize<T>(JsonNode, JsonTypeInfo<T>) method.
            // T has to be the concrete type being instantiated. (We can't use TTarget, because
            // that's the base or interface type for which we are enabling polymorphism, and
            // System.Text.Json would just call our converter again, leading to infinite
            // recursion.)
            //
            // C# doesn't provide a mechanism for invoking a generic method such that the type
            // argument is determined by the runtime type of some object, so we have to use
            // reflection. We maintain a cache of delegates for all the instances of this generic
            // method we already made so that we only need to use reflection for target types not
            // previously seen. This means we avoid reflection on the hot path once we're up and
            // running.
            Action<TTarget, JsonNode, JsonSerializerOptions> populateExistingInstanceUsingReflection =
                DeserializeIntoInstanceCache.GetOrAdd(
                    instanceType,
                    static t =>
                    {
                        MethodInfo mi = InPlaceDeserializerGenericMethodInfo.Value.MakeGenericMethod(t);
                        Delegate actualDelegate = mi.CreateDelegate(
                            typeof(Action<TTarget, JsonNode, JsonSerializerOptions>));
                        return (Action<TTarget, JsonNode, JsonSerializerOptions>)actualDelegate;
                    });

            populateExistingInstanceUsingReflection(instance, json, options);
            return instance;
        }

        private static void DeserializeIntoExistingInstanceCore<T>(
            TTarget instance, JsonNode json, JsonSerializerOptions options)
            where T : TTarget
        {
            // Note: we need to get hold of a new JsonTypeInfo<T> each time because we need its
            // CreateObject factory method to return the specific instance being populated.
            // So this isn't a step we can cache per type.
            // (GetTypeInfo returns a new instance each time you call it, so there should be no
            // global side effects of setting CreateObject.)
            var jsonTypeInfo = (JsonTypeInfo<T>)JsonTypeInfoResolver.GetTypeInfo(typeof(T), options);
            jsonTypeInfo.CreateObject = () => (T)instance;

            // We don't need to return anything. The result returned by Deserialize here is the
            // existing instance, so the caller already has it. (We could return it, but it would
            // make the reflection code slightly more complex, and it's hard enough to follow as
            // it is.)
            _ = json.Deserialize(jsonTypeInfo);
        }
    }
}