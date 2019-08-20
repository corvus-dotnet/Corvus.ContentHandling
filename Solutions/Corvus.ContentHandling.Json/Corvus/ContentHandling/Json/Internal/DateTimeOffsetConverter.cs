// <copyright file="DateTimeOffsetConverter.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Json.Internal
{
    using System;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// A standard json converter for <see cref="DateTimeOffset"/>.
    /// </summary>
    public class DateTimeOffsetConverter : JsonConverter
    {
        /// <inheritdoc/>
        public override bool CanConvert(Type objectType)
        {
            return typeof(DateTimeOffset) == objectType || typeof(DateTimeOffset?) == objectType;
        }

        /// <inheritdoc/>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            if (reader.TokenType == JsonToken.Date)
            {
                return (DateTimeOffset)reader.ReadAsDateTimeOffset();
            }

            var value = JObject.Load(reader);

            if (value != null)
            {
                return (DateTimeOffset)value["dateTimeOffset"];
            }

            reader.Read();
            return null;
        }

        /// <inheritdoc/>
        /// <remarks>
        /// <para>
        /// Note that this will write the <see cref="DateTimeOffset"/> as a complex object containing both an ISO Date Time string with timezone, and a unix time long.
        /// </para>
        /// <code>
        /// ![<![CDATA[ { "dateTimeOffset": "2009-06-15T13:45:30.0000000-07:00", "unixTime": 1245098730000]]>
        /// </code>
        /// </remarks>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is DateTimeOffset dto)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("dateTimeOffset");
                writer.WriteRawValue($"\"{dto.ToString("O")}\"");
                writer.WritePropertyName("unixTime");
                writer.WriteValue(dto.ToUnixTimeMilliseconds());
                writer.WriteEndObject();
            }
            else
            {
                writer.WriteNull();
            }
        }
    }
}
