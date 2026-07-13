// <copyright file="KeyEnumerationTests.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Shouldly;

    [TestClass]
    public class KeyEnumerationTests
    {
        [TestMethod]
        public void GetAllContentTypes_ReturnsContentTypesEndingWithTheSuffix()
        {
            using ServiceProvider provider = Build();

            List<string> contentTypes = [.. provider.GetAllContentTypes("+view")];

            contentTypes.ShouldBe(
                ["application/vnd.corvus.test.one+view", "application/vnd.corvus.test.two+view"],
                ignoreOrder: true);
        }

        [TestMethod]
        public void GetAllContent_ResolvesAnInstancePerMatchingContentType()
        {
            using ServiceProvider provider = Build();

            List<object> instances = [.. provider.GetAllContent<object>("+view")];

            instances.Count.ShouldBe(2);
            instances.ShouldContain(i => i is ExplicitContent);
            instances.ShouldContain(i => i is OtherExplicitContent);
        }

        [TestMethod]
        public void GetAllContentTypes_ReturnsEmptyWhenNothingMatches()
        {
            using ServiceProvider provider = Build();

            provider.GetAllContentTypes("+nomatch").ShouldBeEmpty();
        }

        private static ServiceProvider Build()
        {
            return new ServiceCollection()
                .AddContentHandling(c => c
                    .AddTransientContent<ExplicitContent>("application/vnd.corvus.test.one+view")
                    .AddTransientContent<OtherExplicitContent>("application/vnd.corvus.test.two+view")
                    .AddTransientContent<AttributedContent>("application/vnd.corvus.test.three+other"))
                .BuildServiceProvider();
        }
    }
}
