// <copyright file="ResolutionBenchmarks.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Benchmarks
{
    using System;
    using BenchmarkDotNet.Attributes;
    using Corvus.ContentHandling;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Content resolution by content type, including the hierarchical fallback path.
    /// </summary>
    [MemoryDiagnoser]
    public class ResolutionBenchmarks
    {
        private ServiceProvider provider = null!;

        [GlobalSetup]
        public void Setup()
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
                .AddTransientContent<ContentType10>());
            this.provider = services.BuildServiceProvider();
        }

        [GlobalCleanup]
        public void Cleanup()
        {
            this.provider.Dispose();
        }

        [Benchmark]
        public object? ResolveExact()
        {
            return this.provider.GetContent(ContentType01.RegisteredContentType);
        }

        [Benchmark]
        public object? ResolveWithFallback()
        {
            // Three fallback hops: .a.b.c -> .a.b -> .a -> content01
            return this.provider.GetContent("application/vnd.corvus.benchmark.content01.a.b.c");
        }

        [Benchmark]
        public ContentType10 ResolveTyped()
        {
            return this.provider.GetRequiredContent<ContentType10>(ContentType10.RegisteredContentType);
        }

        [Benchmark]
        public bool TryGetTypeFor()
        {
            return this.provider.TryGetTypeFor(ContentType01.RegisteredContentType, out Type? _);
        }
    }
}
