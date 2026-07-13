// <copyright file="LifetimeTests.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Tests
{
    using Corvus.ContentHandling;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Shouldly;

    [TestClass]
    public class LifetimeTests
    {
        [TestMethod]
        public void SingletonContent_ResolvesTheSameInstance()
        {
            using ServiceProvider provider = Build(c => c.AddSingletonContent<ExplicitContent>("application/vnd.corvus.test.lifetime"));

            object? first = provider.GetContent("application/vnd.corvus.test.lifetime");
            object? second = provider.GetContent("application/vnd.corvus.test.lifetime");

            first.ShouldBeSameAs(second);
        }

        [TestMethod]
        public void TransientContent_ResolvesDistinctInstances()
        {
            using ServiceProvider provider = Build(c => c.AddTransientContent<ExplicitContent>("application/vnd.corvus.test.lifetime"));

            object? first = provider.GetContent("application/vnd.corvus.test.lifetime");
            object? second = provider.GetContent("application/vnd.corvus.test.lifetime");

            first.ShouldNotBeSameAs(second);
        }

        [TestMethod]
        public void ScopedContent_IsSharedWithinAScope_AndDistinctAcrossScopes()
        {
            using ServiceProvider provider = Build(c => c.AddScopedContent<ExplicitContent>("application/vnd.corvus.test.lifetime"));

            using IServiceScope scope1 = provider.CreateScope();
            using IServiceScope scope2 = provider.CreateScope();

            object? scope1First = scope1.ServiceProvider.GetContent("application/vnd.corvus.test.lifetime");
            object? scope1Second = scope1.ServiceProvider.GetContent("application/vnd.corvus.test.lifetime");
            object? scope2First = scope2.ServiceProvider.GetContent("application/vnd.corvus.test.lifetime");

            scope1First.ShouldBeSameAs(scope1Second);
            scope1First.ShouldNotBeSameAs(scope2First);
        }

        [TestMethod]
        public void SameType_UnderTwoContentTypes_YieldsTwoSingletonInstances()
        {
            using ServiceProvider provider = Build(c => c
                .AddSingletonContent<ExplicitContent>("application/vnd.corvus.test.one")
                .AddSingletonContent<ExplicitContent>("application/vnd.corvus.test.two"));

            object? one = provider.GetContent("application/vnd.corvus.test.one");
            object? two = provider.GetContent("application/vnd.corvus.test.two");

            one.ShouldNotBeSameAs(two);
        }

        [TestMethod]
        public void ContentAlias_SharesTheTargetSingletonInstance()
        {
            using ServiceProvider provider = Build(c => c
                .AddSingletonContent<ExplicitContent>("application/vnd.corvus.test.target")
                .AddContentAlias("application/vnd.corvus.test.alias", "application/vnd.corvus.test.target"));

            object? target = provider.GetContent("application/vnd.corvus.test.target");
            object? alias = provider.GetContent("application/vnd.corvus.test.alias");

            alias.ShouldBeSameAs(target);
        }

        [TestMethod]
        public void ValidateOnBuild_PassesForAllRegistrationKinds()
        {
            var services = new ServiceCollection();
            services.AddContentHandling(c => c
                .AddSingletonContent<ExplicitContent>("application/vnd.corvus.test.singleton")
                .AddScopedContent<OtherExplicitContent>("application/vnd.corvus.test.scoped")
                .AddSerializedContent<AttributedContent>("application/vnd.corvus.test.serialized")
                .AddSerializedContent<ContentWithoutParameterlessCtor>("application/vnd.corvus.test.noctor"));

            using ServiceProvider provider = services.BuildServiceProvider(new ServiceProviderOptions
            {
                ValidateOnBuild = true,
                ValidateScopes = true,
            });

            provider.ShouldNotBeNull();
        }

        private static ServiceProvider Build(System.Action<ContentHandlingBuilder> configure)
        {
            return new ServiceCollection().AddContentHandling(configure).BuildServiceProvider();
        }
    }
}
