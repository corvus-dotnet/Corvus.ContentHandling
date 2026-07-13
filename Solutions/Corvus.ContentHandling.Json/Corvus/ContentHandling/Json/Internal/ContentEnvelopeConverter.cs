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
    /// The JSON converter for <see cref="ContentEnvelope"/>, producing the
    /// <c>{ "contentType": ..., "payload": ... }</c> wire format.
    /// </summary>
    public class ContentEnvelopeConverter : JsonConverter<ContentEnvelope>
    {
        /// <inheritdoc/>
        public override bool CanConvert(Type objectType)
        {
            return typeof(ContentEnvelope) == objectType;
        }

        /// <inheritdoc/>
        public override ContentEnvelope? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (JsonNode.Parse(ref reader) is not JsonObject value)
            {
                throw new JsonException($"A {nameof(ContentEnvelope)} must be a JSON object.");
            }

            if (value[ContentTypeJson.ContentTypePropertyName] is not JsonValue contentTypeValue
                || contentTypeValue.GetValueKind() != JsonValueKind.String)
            {
                throw new JsonException($"A {nameof(ContentEnvelope)} must have a string '{ContentTypeJson.ContentTypePropertyName}' property.");
            }

            if (value[ContentTypeJson.PayloadPropertyName] is not JsonNode payload)
            {
                throw new JsonException($"A {nameof(ContentEnvelope)} must have a non-null '{ContentTypeJson.PayloadPropertyName}' property.");
            }

            return ContentEnvelope.FromJson(payload, options, contentTypeValue.GetValue<string>());
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
                writer.WritePropertyName(ContentTypeJson.ContentTypePropertyName);
                writer.WriteStringValue(value.PayloadContentType);
                writer.WritePropertyName(ContentTypeJson.PayloadPropertyName);
                value.SerializedPayload.WriteTo(writer);
                writer.WriteEndObject();
            }
        }
    }
}
