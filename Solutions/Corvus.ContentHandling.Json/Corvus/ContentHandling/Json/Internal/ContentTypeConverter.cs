// <copyright file="ContentTypeConverter.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Json.Internal
{
    using System;
    using System.Reflection;
    using System.Threading;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// A type converter for objects which conform to the content-type polymorphism pattern.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This interface is provides support for custom serialization for content registered using the
    /// <see cref="ContentFactory"/> mechanism.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// [<![CDATA[
    /// public class MyContent
    /// {
    ///     public const string RegisteredContentType = "application/vnd.corvus.example.mycontent"
    ///     public string SomeProperty { get; set; }
    ///     public string ContentType { get { return RegisteredContentType; } }
    /// }
    ///
    /// // Register the content
    /// serviceCollection.RegisterTransientContent<MyContent>();
    /// ]]>
    /// </code>
    /// </example>
    public class ContentTypeConverter : CustomCreationConverter<object>
    {
        private readonly IServiceProvider serviceProvider;
        private readonly ThreadLocal<Type> skipType = new ThreadLocal<Type>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentTypeConverter"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        public ContentTypeConverter(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        /// <inheritdoc/>
        public override bool CanRead => true;

        /// <inheritdoc/>
        public override bool CanWrite => false;

        /// <inheritdoc/>
        public override bool CanConvert(Type objectType)
        {
            if (this.skipType.IsValueCreated && objectType == this.skipType.Value)
            {
                this.skipType.Value = null;
                return false;
            }

            PropertyInfo member = objectType.GetTypeInfo().GetProperty("ContentType", BindingFlags.Public | BindingFlags.Instance);

            return member != null;
        }

        /// <inheritdoc/>
        public override object Create(Type objectType)
        {
            // Do not create the object.
            return null;
        }

        /// <inheritdoc/>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader is null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            if (serializer is null)
            {
                throw new ArgumentNullException(nameof(serializer));
            }

            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            reader.MaxDepth = 4096;
            var jo = JObject.Load(reader);

            string contentType = (string)jo["contentType"];

            if (!this.serviceProvider.TryGetTypeFor(contentType, out Type type, out bool usesServices))
            {
                throw new InvalidOperationException($"The content for type {contentType} has not been registered with the ContentFactory.");
            }

            JsonReader contentReader = jo.CreateReader();
            contentReader.Culture = reader.Culture;
            contentReader.CloseInput = reader.CloseInput;
            contentReader.DateFormatString = reader.DateFormatString;
            contentReader.DateParseHandling = reader.DateParseHandling;
            contentReader.DateTimeZoneHandling = reader.DateTimeZoneHandling;
            contentReader.FloatParseHandling = reader.FloatParseHandling;
            contentReader.MaxDepth = reader.MaxDepth;
            contentReader.SupportMultipleContent = reader.SupportMultipleContent;

            if (usesServices)
            {
                // Some content type implementation types depend on services, and must therefore be
                // constructed through DI. The downside of this is that constructor-based property
                // initialization is not available, which makes clean support for nullable
                // references a pain. It is up to consuming code to decide whether the convenience
                // of being able to deserialize directly into a type that also receive services
                // from DI outweights the pain.
                object result = this.serviceProvider.GetContent(contentType);
                serializer.Populate(contentReader, result);
                return result;
            }
            else
            {
                // This content type's implementing type doesn't depend on anything from DI, so
                // we want to get JSON.NET to do the work for us. However, since we're just asking
                // it again to do the thing it was already doing, it'll invoke our type converter
                // a second time, leading to infinite recursion. And we can't just build a separate
                // JSON settings object with this type converter removed, because we need it to
                // remain in place to be able to handle any nested objects—we want recursion in
                // those cases. To avoid recursion for the object we're on right now, we want our
                // CanConvert method to return false on the call that we know is about to come in.
                // So we set a thread-local value tracking the fact that we're expecting a second
                // call for this type that we should ignore. Our CanConvert detects that call,
                // resets this thread-local value and then returns false, ensuring that nested
                // conversions can still occur.
                try
                {
                    this.skipType.Value = type;
                    return serializer.Deserialize(contentReader, type);
                }
                finally
                {
                    // Clear this just in case—cleanup semantics around long-lived ThreadLocal<T>
                    // instances are a little unclear.
                    this.skipType.Value = null;
                }
            }
        }

        /// <inheritdoc/>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotSupportedException();
        }
    }
}
