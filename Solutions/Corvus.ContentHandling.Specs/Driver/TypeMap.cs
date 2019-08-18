namespace Corvus.Extensions.Specs.Driver
{
    using System;

    public static class TypeMap
    {
        public static Type GetTypeFor(string typeName)
        {
            switch (typeName)
            {
                case "ExplicitType1":
                    return typeof(ExplicitType1);
                case "ExplicitType1Subtype1":
                    return typeof(ExplicitType1Subtype1);
                case "ExplicitType2":
                    return typeof(ExplicitType2);
                case "ExplicitType3":
                    return typeof(ExplicitType3);
                case "ExplicitType3Subtype1":
                    return typeof(ExplicitType3Subtype1);
                case "ExplicitType4":
                    return typeof(ExplicitType4);
                case "ExplicitType5":
                    return typeof(ExplicitType5);
                case "ExplicitType5Subtype1":
                    return typeof(ExplicitType5Subtype1);
                case "ExplicitType6":
                    return typeof(ExplicitType6);
                case "ContentType1":
                    return typeof(ContentType1);
                case "ContentType1Subtype1":
                    return typeof(ContentType1Subtype1);
                case "ContentType2":
                    return typeof(ContentType2);
                case "ContentType3":
                    return typeof(ContentType3);
                case "ContentType3Subtype1":
                    return typeof(ContentType3Subtype1);
                case "ContentType4":
                    return typeof(ContentType4);
                case "ContentType5":
                    return typeof(ContentType5);
                case "ContentType5Subtype1":
                    return typeof(ContentType5Subtype1);
                case "ContentType6":
                    return typeof(ContentType6);
                default:
                    throw new ArgumentException(nameof(typeName));
            }
        }
    }
}
