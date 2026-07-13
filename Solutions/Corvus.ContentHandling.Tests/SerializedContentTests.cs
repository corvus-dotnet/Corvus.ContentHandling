// <copyright file="SerializedContentTests.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Tests
{
    using System;
    using Corvus.ContentHandling;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Shouldly;

    [TestClass]
    public class SerializedContentTests
    {
        [TestMethod]
        public void SerializedContent_IsConstructedViaParameterlessCtor()
        {
            using ServiceProvider provider = Build();

            object? content = provider.GetContent("application/vnd.corvus.test.serialized");

            content.ShouldBeOfType<ExplicitContent>();
        }

        [TestMethod]
        public void SerializedContent_YieldsANewInstancePerResolution()
        {
            using ServiceProvider provider = Build();

            provider.GetContent("application/vnd.corvus.test.serialized")
                .ShouldNotBeSameAs(provider.GetContent("application/vnd.corvus.test.serialized"));
        }

        [TestMethod]
        public void SerializedContent_AddsNoKeyedServiceToTheContainer()
        {
            using ServiceProvider provider = Build();

            provider.GetKeyedService<ExplicitContent>("application/vnd.corvus.test.serialized").ShouldBeNull();
        }

        [TestMethod]
        public void SerializedContent_WithoutParameterlessCtor_ThrowsOnDirectResolution()
        {
            using ServiceProvider provider = new ServiceCollection()
                .AddContentHandling(c => c.AddSerializedContent<ContentWithoutParameterlessCtor>("application/vnd.corvus.test.noctor"))
                .BuildServiceProvider();

            InvalidOperationException exception = Should.Throw<InvalidOperationException>(
                () => provider.GetRequiredContent("application/vnd.corvus.test.noctor"));

            exception.Message.ShouldContain("parameterless constructor");
        }

        private static ServiceProvider Build()
        {
            return new ServiceCollection()
                .AddContentHandling(c => c.AddSerializedContent<ExplicitContent>("application/vnd.corvus.test.serialized"))
                .BuildServiceProvider();
        }
    }
}
