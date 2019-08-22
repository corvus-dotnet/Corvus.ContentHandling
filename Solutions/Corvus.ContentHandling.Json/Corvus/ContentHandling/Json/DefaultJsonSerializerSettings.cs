// <copyright file="DefaultJsonSerializerSettings.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Json
{
    using System.Collections.Generic;
    using System.Globalization;
    using Newtonsoft.Json;

    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultJsonSerializerSettings"/> class.
    /// </summary>
    public class DefaultJsonSerializerSettings : IDefaultJsonSerializerSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultJsonSerializerSettings"/> class.
        /// </summary>
        /// <param name="converters">The list of JsonConverters to add.</param>
        /// <remarks>
        /// You should not modify these settings directly. They are shared by all users.
        /// </remarks>
        public DefaultJsonSerializerSettings(IEnumerable<JsonConverter> converters)
        {
            this.Instance = new JsonSerializerSettings
            {
                ContractResolver = StandardContractResolver.Instance,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Error,
                NullValueHandling = NullValueHandling.Include,
                ObjectCreationHandling = ObjectCreationHandling.Auto,
                PreserveReferencesHandling = PreserveReferencesHandling.None,
                ConstructorHandling = ConstructorHandling.Default,
                TypeNameHandling = TypeNameHandling.None,
                MetadataPropertyHandling = MetadataPropertyHandling.Default,
                Formatting = Formatting.None,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind,
                DateParseHandling = DateParseHandling.DateTimeOffset,
                FloatParseHandling = FloatParseHandling.Double,
                FloatFormatHandling = FloatFormatHandling.String,
                StringEscapeHandling = StringEscapeHandling.Default,
                CheckAdditionalContent = false,
                DateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK",
                Converters = new List<JsonConverter>(converters),
                ReferenceResolverProvider = null,
                Context = default,
                Culture = CultureInfo.InvariantCulture,
                MaxDepth = 4096,
                DefaultValueHandling = DefaultValueHandling.Include,
            };
        }

        /// <summary>
        /// Gets the instance of JsonSerializerSettings to use as the default.
        /// </summary>
        public JsonSerializerSettings Instance { get; }
    }
}