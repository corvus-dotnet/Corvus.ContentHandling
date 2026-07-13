// <copyright file="RegistrationBenchmarks.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Benchmarks
{
    using System;
    using System.Threading;
    using BenchmarkDotNet.Attributes;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Registration/startup cost. Each benchmark builds a fresh service collection and
    /// provider — the "application startup" scenario.
    /// </summary>
    [MemoryDiagnoser]
    public class RegistrationBenchmarks
    {
        private static int uniqueSeed;

        [Benchmark]
        public IServiceProvider RegisterTenContentTypes()
        {
            var services = new ServiceCollection();
            services.AddContentHandling(c => c
                .AddSerializedContent<ContentType01>()
                .AddSerializedContent<ContentType02>()
                .AddSerializedContent<ContentType03>()
                .AddSerializedContent<ContentType04>()
                .AddSerializedContent<ContentType05>()
                .AddSerializedContent<ContentType06>()
                .AddSerializedContent<ContentType07>()
                .AddSerializedContent<ContentType08>()
                .AddSerializedContent<ContentType09>()
                .AddSerializedContent<ContentType10>());
            return services.BuildServiceProvider();
        }

        [Benchmark]
        public IServiceProvider RegisterTenTransientContentTypes()
        {
            var services = new ServiceCollection();
            services.AddContentHandling(c => c
                .AddTransientContent<ContentType01>()
                .AddTransientContent<ContentType02>()
                .AddTransientContent<ContentType03>()
                .AddTransientContent<ContentType04>()
                .AddTransientContent<ContentType05>()
                .AddTransientContent<ContentType06>()
                .AddTransientContent<ContentType07>()
                .AddTransientContent<ContentType08>()
                .AddTransientContent<ContentType09>()
                .AddTransientContent<ContentType10>());
            return services.BuildServiceProvider();
        }

        /// <summary>
        /// Registers five delegate-based content handlers. v4 compiles a unique wrapper type
        /// with Roslyn at runtime for every (contentType, handlerClass) pair; v5 registers
        /// keyed instances of one closed generic adapter. Unique content-type names per
        /// invocation match the v4 project's scheme (where they defeat its compilation cache,
        /// reflecting the cost a real application pays registering each handler once).
        /// </summary>
        [Benchmark]
        public IServiceProvider RegisterFiveLambdaHandlers()
        {
            int seed = Interlocked.Increment(ref uniqueSeed);
            var services = new ServiceCollection();
            services.AddContentHandling(c =>
            {
                for (int i = 0; i < 5; i++)
                {
                    c.AddContentHandler<BenchmarkPayload, BenchmarkPayload>(
                        $"application/vnd.corvus.benchmark.unique{seed}.h{i}",
                        "bench",
                        (BenchmarkPayload payload) => { });
                }
            });
            return services.BuildServiceProvider();
        }
    }
}
