// <copyright file="ContentEnvelopeConverter.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Json.Internal
{
    using System;
    using System.Threading;
    using Corvus.Extensions.Json;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// A standard json converter for <see cref="ContentEnvelope"/>.
    /// </summary>
    public class ContentEnvelopeConverter : JsonConverter
    {
        private const string SerializedPayloadTag = "payload";
        private const string PayloadContentTypeTag = "contentType";
        private readonly IServiceProvider serviceProvider;
        private readonly Lazy<IJsonSerializerSettingsProvider> jsonSerializerSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentEnvelopeConverter"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider for the context.</param>
        public ContentEnvelopeConverter(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            this.jsonSerializerSettings = new Lazy<IJsonSerializerSettingsProvider>(() => this.serviceProvider.GetService<IJsonSerializerSettingsProvider>(), LazyThreadSafetyMode.ExecutionAndPublication);
        }

        /// <inheritdoc/>
        public override bool CanConvert(Type objectType)
        {
            return typeof(ContentEnvelope) == objectType;
        }

        /// <inheritdoc/>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader is null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            var value = JToken.ReadFrom(reader) as JObject;
            return ContentEnvelope.FromJson(value[SerializedPayloadTag], (string)value[PayloadContentTypeTag], this.jsonSerializerSettings.Value?.Instance);
        }

        /// <inheritdoc/>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            if (value is null)
            {
                writer.WriteNull();
            }

            if (value is ContentEnvelope env)
            {
                writer.WriteStartObject();
                writer.WritePropertyName(SerializedPayloadTag);
                writer.WriteValue(env.SerializedPayload);
                writer.WritePropertyName(PayloadContentTypeTag);
                writer.WriteValue(env.PayloadContentType);
                writer.WriteEndObject();
            }
        }
    }
}
