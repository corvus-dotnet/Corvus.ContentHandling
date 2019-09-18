// <copyright file="ContentTypeConverter.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Json.Internal
{
    using System;
    using System.Reflection;
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

            object result = this.serviceProvider.GetContent(contentType);

            if (result is null)
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

            serializer.Populate(contentReader, result);

            return result;
        }

        /// <inheritdoc/>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotSupportedException();
        }
    }
}
