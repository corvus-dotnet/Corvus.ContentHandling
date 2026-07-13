// <copyright file="HandlerDispatchTests.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Corvus.ContentHandling;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NSubstitute;
    using Shouldly;

    [TestClass]
    public class HandlerDispatchTests
    {
        [TestMethod]
        public async Task SynchronousDelegateHandler_ReceivesThePayload()
        {
            var handled = new List<string?>();

            using ServiceProvider provider = Build(c => c.AddContentHandler<SamplePayload, OrderCreated>(
                "created",
                (OrderCreated payload) => handled.Add(payload.OrderId)));

            IContentDispatcher<SamplePayload> dispatcher = provider.GetRequiredService<IContentDispatcher<SamplePayload>>();
            await dispatcher.DispatchAsync(new OrderCreated { OrderId = "42" }, OrderCreated.RegisteredContentType, "created");

            handled.ShouldBe(["42"]);
        }

        [TestMethod]
        public async Task AsynchronousDelegateHandler_ReceivesThePayload()
        {
            var handled = new List<string?>();

            using ServiceProvider provider = Build(c => c.AddContentHandler<SamplePayload, OrderCreated>(
                "created",
                async (OrderCreated payload, System.Threading.CancellationToken cancellationToken) =>
                {
                    await Task.Yield();
                    handled.Add(payload.OrderId);
                }));

            IContentDispatcher<SamplePayload> dispatcher = provider.GetRequiredService<IContentDispatcher<SamplePayload>>();
            await dispatcher.DispatchAsync(new OrderCreated { OrderId = "async" }, OrderCreated.RegisteredContentType, "created");

            handled.ShouldBe(["async"]);
        }

        [TestMethod]
        public async Task DispatchByPayloadConvention_DiscoversTheContentTypeFromThePayload()
        {
            var handled = new List<string?>();

            using ServiceProvider provider = Build(c => c.AddContentHandler<SamplePayload, OrderCreated>(
                "created",
                (OrderCreated payload) => handled.Add(payload.OrderId)));

            IContentDispatcher<SamplePayload> dispatcher = provider.GetRequiredService<IContentDispatcher<SamplePayload>>();
            await dispatcher.DispatchAsync(new OrderCreated { OrderId = "by-convention" }, "created");

            handled.ShouldBe(["by-convention"]);
        }

        [TestMethod]
        public async Task TwoDelegateHandlers_UnderDifferentContentTypes_DoNotCollide()
        {
            // In v4 each of these needed a unique runtime-Roslyn-generated wrapper type;
            // with keyed services they are two instances of one closed generic type.
            var handled = new List<string>();

            using ServiceProvider provider = Build(c => c
                .AddContentHandler<SamplePayload, OrderCreated>("audit", (OrderCreated p) => handled.Add($"created:{p.OrderId}"))
                .AddContentHandler<SamplePayload, OrderCancelled>("audit", (OrderCancelled p) => handled.Add($"cancelled:{p.OrderId}")));

            IContentDispatcher<SamplePayload> dispatcher = provider.GetRequiredService<IContentDispatcher<SamplePayload>>();
            await dispatcher.DispatchAsync(new OrderCreated { OrderId = "1" }, "audit");
            await dispatcher.DispatchAsync(new OrderCancelled { OrderId = "2" }, "audit");

            handled.ShouldBe(["created:1", "cancelled:2"]);
        }

        [TestMethod]
        public async Task ClassHandler_ReceivesConstructorInjectedDependencies()
        {
            ISampleDependency dependency = Substitute.For<ISampleDependency>();

            using ServiceProvider provider = new ServiceCollection()
                .AddSingleton(dependency)
                .AddContentHandling(c => c.AddContentHandler<SamplePayload, OrderCreated, OrderCreatedHandler>("created"))
                .BuildServiceProvider();

            IContentDispatcher<SamplePayload> dispatcher = provider.GetRequiredService<IContentDispatcher<SamplePayload>>();
            await dispatcher.DispatchAsync(new OrderCreated { OrderId = "di" }, "created");

            dependency.Received(1).Record("handled:di");
        }

        [TestMethod]
        public async Task ContextHandler_ReceivesTheContext()
        {
            var handled = new List<string>();

            using ServiceProvider provider = Build(c => c.AddContentHandler<SamplePayload, OrderCreated, (string User, int Attempt)>(
                OrderCreated.RegisteredContentType,
                "created",
                (payload, context, _) =>
                {
                    handled.Add($"{payload.OrderId}:{context.User}:{context.Attempt}");
                    return ValueTask.CompletedTask;
                }));

            IContentDispatcher<SamplePayload> dispatcher = provider.GetRequiredService<IContentDispatcher<SamplePayload>>();
            await dispatcher.DispatchAsync(new OrderCreated { OrderId = "ctx" }, ("alice", 2), "created");

            handled.ShouldBe(["ctx:alice:2"]);
        }

        [TestMethod]
        public async Task ResultHandler_ProducesAResult()
        {
            using ServiceProvider provider = Build(c => c.AddContentHandlerWithResult<SamplePayload, OrderCreated, string>(
                OrderCreated.RegisteredContentType,
                "describe",
                (OrderCreated payload) => $"order {payload.OrderId}"));

            IContentDispatcher<SamplePayload> dispatcher = provider.GetRequiredService<IContentDispatcher<SamplePayload>>();
            string result = await dispatcher.DispatchWithResultAsync<string>(new OrderCreated { OrderId = "7" }, "describe");

            result.ShouldBe("order 7");
        }

        [TestMethod]
        public async Task Dispatch_FallsBackToTheParentContentTypeHandler()
        {
            // A handler registered for "...order.created" also handles the more specific
            // "...order.created.priority" payload via hierarchical fallback.
            var handled = new List<string?>();

            using ServiceProvider provider = Build(c => c.AddContentHandler<SamplePayload, OrderCreated>(
                OrderCreated.RegisteredContentType,
                "created",
                (OrderCreated payload) => handled.Add(payload.OrderId)));

            IContentDispatcher<SamplePayload> dispatcher = provider.GetRequiredService<IContentDispatcher<SamplePayload>>();
            await dispatcher.DispatchAsync(new OrderCreatedPriority { OrderId = "prio" }, "created");

            handled.ShouldBe(["prio"]);
        }

        [TestMethod]
        public async Task UnknownHandler_ThrowsInvalidOperationException()
        {
            using ServiceProvider provider = Build(_ => { });

            IContentDispatcher<SamplePayload> dispatcher = provider.GetRequiredService<IContentDispatcher<SamplePayload>>();

            InvalidOperationException exception = await Should.ThrowAsync<InvalidOperationException>(
                async () => await dispatcher.DispatchAsync(new OrderCreated(), "nonexistent"));

            exception.Message.ShouldContain("No content handler");
        }

        [TestMethod]
        public void DuplicateHandlerRegistration_ThrowsAtConfigureTime()
        {
            var services = new ServiceCollection();

            Should.Throw<InvalidOperationException>(() => services.AddContentHandling(c => c
                .AddContentHandler<SamplePayload, OrderCreated>("created", (OrderCreated _) => { })
                .AddContentHandler<SamplePayload, OrderCreated>("created", (OrderCreated _) => { })));
        }

        private static ServiceProvider Build(Action<ContentHandlingBuilder> configure)
        {
            return new ServiceCollection().AddContentHandling(configure).BuildServiceProvider();
        }
    }
}
