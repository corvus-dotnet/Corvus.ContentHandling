// <copyright file="FallbackResolutionTests.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Tests
{
    using System;
    using Corvus.ContentHandling;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Shouldly;

    /// <summary>
    /// Tests for hierarchical content-type fallback resolution — the key feature: an
    /// unregistered content type falls back to progressively more general parents by removing
    /// trailing dot segments, preserving any "+suffix".
    /// </summary>
    [TestClass]
    public class FallbackResolutionTests
    {
        [TestMethod]
        public void ExactMatch_ResolvesTheRegisteredType()
        {
            IContentRegistry registry = BuildRegistry(c => c.AddTransientContent<ExplicitContent>("application/vnd.corvus.test.a.b"));

            registry.TryResolve("application/vnd.corvus.test.a.b", out ContentRegistration? registration).ShouldBeTrue();
            registration!.ImplementingType.ShouldBe(typeof(ExplicitContent));
            registration.ContentType.ShouldBe("application/vnd.corvus.test.a.b");
        }

        [TestMethod]
        [DataRow("application/vnd.corvus.test.a.b.c")]
        [DataRow("application/vnd.corvus.test.a.b.c.d")]
        [DataRow("application/vnd.corvus.test.a.b.c.d.e")]
        public void UnregisteredSubtype_FallsBackToRegisteredParent(string requested)
        {
            IContentRegistry registry = BuildRegistry(c => c.AddTransientContent<ExplicitContent>("application/vnd.corvus.test.a.b"));

            registry.TryResolve(requested, out ContentRegistration? registration).ShouldBeTrue();
            registration!.ImplementingType.ShouldBe(typeof(ExplicitContent));
        }

        [TestMethod]
        public void Fallback_ReturnsTheResolvedKey_NotTheRequestedKey()
        {
            IContentRegistry registry = BuildRegistry(c => c.AddTransientContent<ExplicitContent>("application/vnd.corvus.test.a"));

            registry.TryResolve("application/vnd.corvus.test.a.b.c", out ContentRegistration? registration).ShouldBeTrue();

            // The resolved key is what keyed-service resolution must use.
            registration!.ContentType.ShouldBe("application/vnd.corvus.test.a");
        }

        [TestMethod]
        public void Fallback_PreservesThePlusSuffix()
        {
            IContentRegistry registry = BuildRegistry(c => c.AddTransientContent<ExplicitContent>("application/vnd.corvus.test.a.b+handler"));

            registry.TryResolve("application/vnd.corvus.test.a.b.c.d+handler", out ContentRegistration? registration).ShouldBeTrue();
            registration!.ContentType.ShouldBe("application/vnd.corvus.test.a.b+handler");
        }

        [TestMethod]
        public void Fallback_DoesNotBleedAcrossSuffixes()
        {
            // A suffixed request must not fall back to an unsuffixed registration (or vice versa).
            IContentRegistry registry = BuildRegistry(c =>
                c.AddTransientContent<ExplicitContent>("application/vnd.corvus.test.a.b"));

            registry.TryResolve("application/vnd.corvus.test.a.b.c+handler", out _).ShouldBeFalse();
        }

        [TestMethod]
        public void Fallback_PrefersTheMostSpecificRegistration()
        {
            IContentRegistry registry = BuildRegistry(c => c
                .AddTransientContent<ExplicitContent>("application/vnd.corvus.test.a")
                .AddTransientContent<OtherExplicitContent>("application/vnd.corvus.test.a.b"));

            registry.TryResolve("application/vnd.corvus.test.a.b.c", out ContentRegistration? registration).ShouldBeTrue();
            registration!.ImplementingType.ShouldBe(typeof(OtherExplicitContent));
        }

        [TestMethod]
        public void Fallback_AppliesToSerializedContent()
        {
            IContentRegistry registry = BuildRegistry(c => c.AddSerializedContent<ExplicitContent>("application/vnd.corvus.test.a"));

            registry.TryResolve("application/vnd.corvus.test.a.b", out ContentRegistration? registration).ShouldBeTrue();
            registration!.Construction.ShouldBe(ContentConstruction.BySerializer);
        }

        [TestMethod]
        public void UnregisteredContentType_DoesNotResolve()
        {
            IContentRegistry registry = BuildRegistry(c => c.AddTransientContent<ExplicitContent>("application/vnd.corvus.test.a"));

            registry.TryResolve("application/vnd.other.unrelated", out ContentRegistration? registration).ShouldBeFalse();
            registration.ShouldBeNull();
        }

        [TestMethod]
        public void GetContent_UsesFallback_EndToEnd()
        {
            ServiceProvider provider = BuildProvider(c => c.AddTransientContent<ExplicitContent>("application/vnd.corvus.test.a.b"));

            object? content = provider.GetContent("application/vnd.corvus.test.a.b.c.d");

            content.ShouldBeOfType<ExplicitContent>();
        }

        [TestMethod]
        public void GetRequiredContent_ThrowsForUnresolvableContentType()
        {
            ServiceProvider provider = BuildProvider(c => c.AddTransientContent<ExplicitContent>("application/vnd.corvus.test.a"));

            Should.Throw<InvalidOperationException>(() => provider.GetRequiredContent("application/vnd.other.unrelated"));
        }

        [TestMethod]
        public void TryGetTypeFor_UsesFallback()
        {
            ServiceProvider provider = BuildProvider(c => c.AddSerializedContent<ExplicitContent>("application/vnd.corvus.test.a"));

            provider.TryGetTypeFor("application/vnd.corvus.test.a.b.c", out Type? implementingType, out ContentConstruction construction).ShouldBeTrue();
            implementingType.ShouldBe(typeof(ExplicitContent));
            construction.ShouldBe(ContentConstruction.BySerializer);
        }

        private static IContentRegistry BuildRegistry(Action<ContentHandlingBuilder> configure)
        {
            return BuildProvider(configure).GetRequiredService<IContentRegistry>();
        }

        private static ServiceProvider BuildProvider(Action<ContentHandlingBuilder> configure)
        {
            return new ServiceCollection()
                .AddContentHandling(configure)
                .BuildServiceProvider();
        }
    }
}
