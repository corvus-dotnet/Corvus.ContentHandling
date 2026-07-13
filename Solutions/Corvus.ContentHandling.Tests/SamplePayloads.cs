// <copyright file="SamplePayloads.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Tests
{
    using System.Threading;
    using System.Threading.Tasks;
    using Corvus.ContentHandling;

    public abstract class SamplePayload
    {
        public abstract string ContentType { get; }
    }

    public class OrderCreated : SamplePayload
    {
        public const string RegisteredContentType = "application/vnd.corvus.test.order.created";

        public override string ContentType => RegisteredContentType;

        public string? OrderId { get; set; }
    }

    public class OrderCancelled : SamplePayload
    {
        public const string RegisteredContentType = "application/vnd.corvus.test.order.cancelled";

        public override string ContentType => RegisteredContentType;

        public string? OrderId { get; set; }
    }

    public class OrderCreatedPriority : OrderCreated
    {
        public new const string RegisteredContentType = "application/vnd.corvus.test.order.created.priority";

        public override string ContentType => RegisteredContentType;
    }

    public class OrderCreatedHandler : IContentHandler<OrderCreated>
    {
        private readonly ISampleDependency dependency;

        public OrderCreatedHandler(ISampleDependency dependency)
        {
            this.dependency = dependency;
        }

        public ValueTask HandleAsync(OrderCreated payload, CancellationToken cancellationToken = default)
        {
            this.dependency.Record($"handled:{payload.OrderId}");
            return ValueTask.CompletedTask;
        }
    }
}
