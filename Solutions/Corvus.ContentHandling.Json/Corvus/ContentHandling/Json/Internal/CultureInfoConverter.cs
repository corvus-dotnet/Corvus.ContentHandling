// <copyright file="CultureInfoConverter.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Json.Internal
{
    using System;
    using System.Globalization;
    using Newtonsoft.Json;

    /// <summary>
    /// A standard json converter for <see cref="CultureInfo"/>.
    /// </summary>
    public class CultureInfoConverter : JsonConverter
    {
        /// <inheritdoc/>
        public override bool CanConvert(Type objectType)
        {
            return typeof(CultureInfo) == objectType;
        }

        /// <inheritdoc/>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            string value = (string)reader.Value;
            if (value != null && value != "null")
            {
                return new CultureInfo(value);
            }

            return null;
        }

        /// <inheritdoc/>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var ci = (CultureInfo)value;
            writer.WriteValue(ci.Name);
        }
    }
}
