// <copyright file="MediaTypeTests.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Tests
{
    using Corvus.ContentHandling;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Shouldly;

    [TestClass]
    public class MediaTypeTests
    {
        [TestMethod]
        public void DefaultMediaType_BehavesAsNone()
        {
            // default(MediaType) bypasses the constructor; it must still honor the type's
            // contract rather than surfacing null strings.
            MediaType mediaType = default;

            mediaType.ShouldBe(MediaType.None);
            mediaType.ToString().ShouldBe(string.Empty);
            mediaType.TypeAndSubtype.ShouldBe(string.Empty);
            mediaType.StructuredSyntaxSuffix.ShouldBe(string.Empty);
            mediaType.GetParent().ShouldBe(MediaType.None);
            ((string)mediaType).ShouldBe(string.Empty);
        }

        [TestMethod]
        public void GetParent_RemovesTheLastSubtypeSegment()
        {
            var mediaType = new MediaType("application/vnd.corvus.a.b", "json");

            MediaType parent = mediaType.GetParent();

            parent.TypeAndSubtype.ShouldBe("application/vnd.corvus.a");
            parent.StructuredSyntaxSuffix.ShouldBe("json");
        }

        [TestMethod]
        public void RoundTrip_ThroughStringCasts_Preserves()
        {
            var mediaType = (MediaType)"application/vnd.corvus.a+json";

            mediaType.TypeAndSubtype.ShouldBe("application/vnd.corvus.a");
            mediaType.StructuredSyntaxSuffix.ShouldBe("json");
            ((string)mediaType).ShouldBe("application/vnd.corvus.a+json");
        }
    }
}
