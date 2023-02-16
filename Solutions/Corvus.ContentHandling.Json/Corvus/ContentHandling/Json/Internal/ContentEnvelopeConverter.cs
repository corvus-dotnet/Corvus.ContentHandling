// <copyright file="ContentEnvelopeConverter.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Json.Internal
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Nodes;
    using System.Text.Json.Serialization;

    /// <summary>
    /// A standard json converter for <see cref="ContentEnvelope"/>.
    /// </summary>
    public class ContentEnvelopeConverter : JsonConverter<ContentEnvelope>
    {
        private const string SerializedPayloadTag = "payload";
        private const string PayloadContentTypeTag = "contentType";

        /// <inheritdoc/>
        public override bool CanConvert(Type objectType)
        {
            return typeof(ContentEnvelope) == objectType;
        }

        /// <inheritdoc/>
        public override ContentEnvelope? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = (JsonObject)JsonNode.Parse(ref reader)!;
            return ContentEnvelope.FromJson(
                value[SerializedPayloadTag]!,
                options,
                value[PayloadContentTypeTag]!.GetValue<string>());
        }

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, ContentEnvelope value, JsonSerializerOptions options)
        {
            ArgumentNullException.ThrowIfNull(writer);

            if (value is null)
            {
                writer.WriteNullValue();
            }
            else
            {
                writer.WriteStartObject();
                writer.WritePropertyName(PayloadContentTypeTag);
                writer.WriteStringValue(value.PayloadContentType);
                writer.WritePropertyName(SerializedPayloadTag);
                value.SerializedPayload.WriteTo(writer);
                writer.WriteEndObject();
            }
        }
    }
}