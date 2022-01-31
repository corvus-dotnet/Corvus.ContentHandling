// <copyright file="TypeMap.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.Extensions.Specs.Driver
{
    using System;

    public static class TypeMap
    {
        public static Type GetTypeFor(string typeName)
        {
            return typeName switch
            {
                "ExplicitType1"         => typeof(ExplicitType1),
                "ExplicitType1Subtype1" => typeof(ExplicitType1Subtype1),
                "ExplicitType2"         => typeof(ExplicitType2),
                "ExplicitType3"         => typeof(ExplicitType3),
                "ExplicitType3Subtype1" => typeof(ExplicitType3Subtype1),
                "ExplicitType4"         => typeof(ExplicitType4),
                "ExplicitType5"         => typeof(ExplicitType5),
                "ExplicitType5Subtype1" => typeof(ExplicitType5Subtype1),
                "ExplicitType6"         => typeof(ExplicitType6),
                "ContentType1"          => typeof(ContentType1),
                "ContentType1Subtype1"  => typeof(ContentType1Subtype1),
                "ContentType2"          => typeof(ContentType2),
                "ContentType3"          => typeof(ContentType3),
                "ContentType3Subtype1"  => typeof(ContentType3Subtype1),
                "ContentType4"          => typeof(ContentType4),
                "ContentType5"          => typeof(ContentType5),
                "ContentType5Subtype1"  => typeof(ContentType5Subtype1),
                "ContentType6"          => typeof(ContentType6),
                _                       => throw new ArgumentException(nameof(typeName)),
            };
        }
    }
}