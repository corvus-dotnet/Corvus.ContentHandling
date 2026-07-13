// <copyright file="RegistrationTests.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Tests
{
    using System;
    using System.Linq;
    using Corvus.ContentHandling;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Shouldly;

    [TestClass]
    public class RegistrationTests
    {
        [TestMethod]
        [DataRow(ServiceLifetime.Singleton)]
        [DataRow(ServiceLifetime.Scoped)]
        [DataRow(ServiceLifetime.Transient)]
        public void ExplicitContentType_ResolvesForEveryLifetime(ServiceLifetime lifetime)
        {
            using ServiceProvider provider = new ServiceCollection()
                .AddContentHandling(c => c.AddContent<ExplicitContent>("application/vnd.corvus.test.explicit", lifetime))
                .BuildServiceProvider();

            using IServiceScope scope = provider.CreateScope();
            object? content = scope.ServiceProvider.GetContent("application/vnd.corvus.test.explicit");

            content.ShouldBeOfType<ExplicitContent>();
        }

        [TestMethod]
        public void ContentType_IsDiscoveredFromAttribute()
        {
            using ServiceProvider provider = new ServiceCollection()
                .AddContentHandling(c => c.AddTransientContent<AttributedContent>())
                .BuildServiceProvider();

            provider.GetContent(AttributedContent.RegisteredContentType).ShouldBeOfType<AttributedContent>();
        }

        [TestMethod]
        public void ContentType_IsDiscoveredFromLegacyStaticField()
        {
            using ServiceProvider provider = new ServiceCollection()
                .AddContentHandling(c => c.AddTransientContent<LegacyFieldContent>())
                .BuildServiceProvider();

            provider.GetContent(LegacyFieldContent.RegisteredContentType).ShouldBeOfType<LegacyFieldContent>();
        }

        [TestMethod]
        public void TypeWithoutContentType_ThrowsOnConventionRegistration()
        {
            var services = new ServiceCollection();

            Should.Throw<InvalidOperationException>(
                () => services.AddContentHandling(c => c.AddTransientContent<ExplicitContent>()));
        }

        [TestMethod]
        public void DuplicateContentType_ThrowsAtConfigureTime()
        {
            var services = new ServiceCollection();

            Should.Throw<InvalidOperationException>(
                () => services.AddContentHandling(c => c
                    .AddTransientContent<ExplicitContent>("application/vnd.corvus.test.duplicate")
                    .AddTransientContent<OtherExplicitContent>("application/vnd.corvus.test.duplicate")));
        }

        [TestMethod]
        public void MultipleAddContentHandlingCalls_ShareOneRegistry()
        {
            using ServiceProvider provider = new ServiceCollection()
                .AddContentHandling(c => c.AddTransientContent<ExplicitContent>("application/vnd.corvus.test.first"))
                .AddContentHandling(c => c.AddTransientContent<OtherExplicitContent>("application/vnd.corvus.test.second"))
                .BuildServiceProvider();

            provider.GetContent("application/vnd.corvus.test.first").ShouldBeOfType<ExplicitContent>();
            provider.GetContent("application/vnd.corvus.test.second").ShouldBeOfType<OtherExplicitContent>();
            provider.GetRequiredService<IContentRegistry>().ContentTypes.Count.ShouldBe(2);
        }

        [TestMethod]
        public void FactoryRegistration_UsesTheFactory()
        {
            using ServiceProvider provider = new ServiceCollection()
                .AddContentHandling(c => c.AddContent(
                    "application/vnd.corvus.test.factory",
                    _ => new ExplicitContent { Value = "from-factory" },
                    ServiceLifetime.Transient))
                .BuildServiceProvider();

            var content = provider.GetContent<ExplicitContent>("application/vnd.corvus.test.factory");

            content!.Value.ShouldBe("from-factory");
        }

        [TestMethod]
        public void InstanceRegistration_ResolvesTheInstance()
        {
            var instance = new ExplicitContent { Value = "the-instance" };

            using ServiceProvider provider = new ServiceCollection()
                .AddContentHandling(c => c.AddContentInstance("application/vnd.corvus.test.instance", instance))
                .BuildServiceProvider();

            provider.GetContent("application/vnd.corvus.test.instance").ShouldBeSameAs(instance);
        }

        [TestMethod]
        public void ContentWithDependency_IsConstructedThroughTheContainer()
        {
            ISampleDependency dependency = NSubstitute.Substitute.For<ISampleDependency>();

            using ServiceProvider provider = new ServiceCollection()
                .AddSingleton(dependency)
                .AddContentHandling(c => c.AddTransientContent<ContentWithDependency>("application/vnd.corvus.test.withdependency"))
                .BuildServiceProvider();

            var content = provider.GetContent<ContentWithDependency>("application/vnd.corvus.test.withdependency");

            content!.Dependency.ShouldBeSameAs(dependency);
        }

        [TestMethod]
        public void GetRequiredContent_ThrowsDistinctErrorForWrongType()
        {
            using ServiceProvider provider = new ServiceCollection()
                .AddContentHandling(c => c.AddTransientContent<ExplicitContent>("application/vnd.corvus.test.explicit"))
                .BuildServiceProvider();

            InvalidOperationException exception = Should.Throw<InvalidOperationException>(
                () => provider.GetRequiredContent<OtherExplicitContent>("application/vnd.corvus.test.explicit"));

            exception.Message.ShouldContain("not of type");
        }

        [TestMethod]
        public void GetContentOfT_DerivesContentTypeFromT()
        {
            using ServiceProvider provider = new ServiceCollection()
                .AddContentHandling(c => c.AddTransientContent<AttributedContent>())
                .BuildServiceProvider();

            provider.GetContent<AttributedContent>().ShouldNotBeNull();
        }

        [TestMethod]
        public void Registration_IsRecordedInTheRegistry()
        {
            using ServiceProvider provider = new ServiceCollection()
                .AddContentHandling(c => c.AddSingletonContent<ExplicitContent>("application/vnd.corvus.test.explicit"))
                .BuildServiceProvider();

            IContentRegistry registry = provider.GetRequiredService<IContentRegistry>();
            registry.TryGet("application/vnd.corvus.test.explicit", out ContentRegistration? registration).ShouldBeTrue();
            registration!.Lifetime.ShouldBe(ServiceLifetime.Singleton);
            registration.Construction.ShouldBe(ContentConstruction.FromServices);
            registry.ContentTypes.Single().ShouldBe("application/vnd.corvus.test.explicit");
        }
    }
}
