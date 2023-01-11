// <copyright file="ContentEnvelopeConverter.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Json.Internal
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Nodes;
    using System.Text.Json.Serialization;
    using System.Threading;

    using Corvus.Json.Serialization;

    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// A standard json converter for <see cref="ContentEnvelope"/>.
    /// </summary>
    public class ContentEnvelopeConverter : JsonConverter<object>
    {
        private const string SerializedPayloadTag = "payload";
        private const string PayloadContentTypeTag = "contentType";
        private readonly IServiceProvider serviceProvider;
        private readonly Lazy<IJsonSerializerOptionsProvider> jsonSerializerSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentEnvelopeConverter"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider for the context.</param>
        public ContentEnvelopeConverter(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            this.jsonSerializerSettings = new Lazy<IJsonSerializerOptionsProvider>(() => this.serviceProvider.GetRequiredService<IJsonSerializerOptionsProvider>(), LazyThreadSafetyMode.ExecutionAndPublication);
        }

        /// <inheritdoc/>
        public override bool CanConvert(Type objectType)
        {
            return typeof(ContentEnvelope) == objectType;
        }

        /// <inheritdoc/>
        public override object? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = (JsonObject)JsonNode.Parse(ref reader)!;
            return ContentEnvelope.FromJson(
                value[SerializedPayloadTag]!,
                options,
                value[PayloadContentTypeTag]!.GetValue<string>());
        }

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
        {
            ArgumentNullException.ThrowIfNull(writer);

            if (value is null)
            {
                writer.WriteNullValue();
            }

            if (value is ContentEnvelope env)
            {
                writer.WriteStartObject();
                writer.WritePropertyName(PayloadContentTypeTag);
                writer.WriteStringValue(env.PayloadContentType);
                writer.WritePropertyName(SerializedPayloadTag);
                env.SerializedPayload.WriteTo(writer);
                writer.WriteEndObject();
            }
        }
    }
}