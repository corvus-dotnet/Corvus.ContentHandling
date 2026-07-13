// <copyright file="DispatchBenchmarks.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Benchmarks
{
    using System.Threading.Tasks;
    using BenchmarkDotNet.Attributes;
    using Corvus.ContentHandling;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Handler dispatch by content type. v5 dispatch is ValueTask-based; the v4 equivalent is
    /// synchronous — that difference is part of the comparison.
    /// </summary>
    [MemoryDiagnoser]
    public class DispatchBenchmarks
    {
        private ServiceProvider provider = null!;
        private IContentDispatcher<BenchmarkPayload> dispatcher = null!;
        private CreatedPayload payload = null!;
        private CreatedSpecialPayload specialPayload = null!;
        private string? sink;

        [GlobalSetup]
        public void Setup()
        {
            var services = new ServiceCollection();
            services.AddContentHandling(c => c
                .AddContentHandler<BenchmarkPayload, CreatedPayload>("lambda", (CreatedPayload p) => this.sink = p.Id)
                .AddContentHandler<BenchmarkPayload, CreatedPayload, CreatedPayloadClassHandler>("class")
                .AddContentHandler<BenchmarkPayload, CreatedPayload, CreatedPayloadClassHandler>(
                    CreatedPayload.RegisteredContentType, "singletonclass", ServiceLifetime.Singleton)
                .AddContentHandlerWithResult<BenchmarkPayload, CreatedPayload, string>(
                    CreatedPayload.RegisteredContentType, "result", (CreatedPayload p) => p.Id ?? string.Empty));
            this.provider = services.BuildServiceProvider();
            this.dispatcher = this.provider.GetRequiredService<IContentDispatcher<BenchmarkPayload>>();
            this.payload = new CreatedPayload { Id = "42" };
            this.specialPayload = new CreatedSpecialPayload { Id = "43" };
        }

        [GlobalCleanup]
        public void Cleanup()
        {
            this.provider.Dispose();
        }

        [Benchmark]
        public async ValueTask<string?> DispatchToLambdaHandler()
        {
            await this.dispatcher.DispatchAsync(this.payload, CreatedPayload.RegisteredContentType, "lambda");
            return this.sink;
        }

        [Benchmark]
        public async ValueTask<string?> DispatchToClassHandler()
        {
            await this.dispatcher.DispatchAsync(this.payload, CreatedPayload.RegisteredContentType, "class");
            return CreatedPayloadClassHandler.LastHandled;
        }

        /// <summary>
        /// The like-for-like comparison with v4, whose class-handler registration hard-codes
        /// singleton lifetime. The default-lifetime row above is transient — it constructs a
        /// fresh handler and adapter per dispatch, a semantic v4 cannot express.
        /// </summary>
        [Benchmark]
        public async ValueTask<string?> DispatchToSingletonClassHandler()
        {
            await this.dispatcher.DispatchAsync(this.payload, CreatedPayload.RegisteredContentType, "singletonclass");
            return CreatedPayloadClassHandler.LastHandled;
        }

        [Benchmark]
        public ValueTask<string> DispatchWithResult()
        {
            return this.dispatcher.DispatchWithResultAsync<string>(this.payload, "result");
        }

        [Benchmark]
        public async ValueTask<string?> DispatchByPayloadConvention()
        {
            await this.dispatcher.DispatchAsync(this.payload, "lambda");
            return this.sink;
        }

        [Benchmark]
        public async ValueTask<string?> DispatchWithFallback()
        {
            // The payload's content type is one level deeper than the handler registration.
            await this.dispatcher.DispatchAsync(this.specialPayload, "lambda");
            return this.sink;
        }
    }
}
