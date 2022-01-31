// <copyright file="ContractResolutionExtensions.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Json
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    /// <summary>
    /// Extension methods that assist with contract resolution.
    /// </summary>
    public static class ContractResolutionExtensions
    {
        /// <summary>
        /// Gets the predicted json member name.
        /// </summary>
        /// <param name="memberInfo">The member for which to get the predicted name.</param>
        /// <param name="serializerSettings">The <see cref="JsonSerializerSettings"/> that apply to the context.</param>
        /// <returns>The predicted member name.</returns>
        public static string GetPredictedMemberName(this MemberInfo memberInfo, JsonSerializerSettings serializerSettings)
        {
            if (memberInfo is null)
            {
                throw new ArgumentNullException(nameof(memberInfo));
            }

            if (serializerSettings is null)
            {
                throw new ArgumentNullException(nameof(serializerSettings));
            }

            return memberInfo.GetCustomAttributes(typeof(JsonPropertyAttribute), true).FirstOrDefault() is JsonPropertyAttribute jsonPropertyAttribute
                ? jsonPropertyAttribute.PropertyName
                : serializerSettings.ContractResolver is CamelCasePropertyNamesContractResolver cccr ? cccr.GetResolvedPropertyName(memberInfo.Name) : memberInfo.Name;
        }
    }
}