// <copyright file="ContractResolutionExtensions.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Json
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    /// <summary>
    /// Extension methods that assist with contract resolution.
    /// </summary>
    public static class ContractResolutionExtensions
    {
        /// <summary>
        /// Gets the predicted json member name.
        /// </summary>
        /// <param name="memberInfo">The member for which to get the predicted name.</param>
        /// <param name="serializerOptions">The <see cref="JsonSerializerOptions"/> that apply to the context.</param>
        /// <returns>The predicted member name.</returns>
        public static string GetPredictedMemberName(this MemberInfo memberInfo, JsonSerializerOptions serializerOptions)
        {
            ArgumentNullException.ThrowIfNull(memberInfo);
            ArgumentNullException.ThrowIfNull(serializerOptions);

            return memberInfo.GetCustomAttributes(typeof(JsonPropertyNameAttribute), true).FirstOrDefault() is JsonPropertyNameAttribute jsonPropertyNameAttribute
                ? jsonPropertyNameAttribute.Name
                : serializerOptions.PropertyNamingPolicy is JsonNamingPolicy np ? np.ConvertName(memberInfo.Name) : memberInfo.Name;
        }
    }
}