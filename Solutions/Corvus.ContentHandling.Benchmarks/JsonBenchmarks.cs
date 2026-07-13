// <copyright file="JsonBenchmarks.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Benchmarks
{
    using System.Text.Json;
    using BenchmarkDotNet.Attributes;
    using Corvus.ContentHandling.Json;
    using Corvus.Json.Serialization;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Polymorphic contentType-discriminated serialization and content envelopes.
    /// </summary>
    [MemoryDiagnoser]
    public class JsonBenchmarks
    {
        private const string SerializedContentA =
            """{"contentType":"application/vnd.corvus.benchmark.json.contenta","someValue":"hello","count":42}""";

        private const string SerializedDiInitialized =
            """{"contentType":"application/vnd.corvus.benchmark.json.diinitialized","someValue":"hello"}""";

        private const string SerializedWithUnregisteredSubtype =
            """{"contentType":"application/vnd.corvus.benchmark.json.contenta.v2.special","someValue":"hello","count":42}""";

        private ServiceProvider provider = null!;
        private JsonSerializerOptions options = null!;
        private BenchmarkContentA value = null!;

        [GlobalSetup]
        public void Setup()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IBenchmarkService, NullBenchmarkService>();
            services.AddContentTypeBasedJsonSerializationSupport();
            services.AddPolymorphicContentTarget<IBenchmarkContent>();
            services.AddContentHandling(c => c
                .AddSerializedContent<BenchmarkContentA>()
                .AddTransientContent<DiInitializedBenchmarkContent>());
            this.provider = services.BuildServiceProvider();
            this.options = this.provider.GetRequiredService<IJsonSerializerOptionsProvider>().Instance;
            this.value = new BenchmarkContentA { SomeValue = "hello", Count = 42 };
        }

        [GlobalCleanup]
        public void Cleanup()
        {
            this.provider.Dispose();
        }

        [Benchmark]
        public string SerializePolymorphic()
        {
            return JsonSerializer.Serialize<IBenchmarkContent>(this.value, this.options);
        }

        [Benchmark]
        public IBenchmarkContent? DeserializePolymorphic()
        {
            return JsonSerializer.Deserialize<IBenchmarkContent>(SerializedContentA, this.options);
        }

        [Benchmark]
        public IBenchmarkContent? DeserializeDiInitialized()
        {
            return JsonSerializer.Deserialize<IBenchmarkContent>(SerializedDiInitialized, this.options);
        }

        [Benchmark]
        public IBenchmarkContent? DeserializeWithFallback()
        {
            return JsonSerializer.Deserialize<IBenchmarkContent>(SerializedWithUnregisteredSubtype, this.options);
        }

        [Benchmark]
        public BenchmarkContentA EnvelopeRoundTrip()
        {
            var envelope = ContentEnvelope.FromPayload(this.value, this.options);
            string json = JsonSerializer.Serialize(envelope, this.options);
            var roundTripped = JsonSerializer.Deserialize<ContentEnvelope>(json, this.options)!;
            return roundTripped.GetContents<BenchmarkContentA>();
        }
    }
}
