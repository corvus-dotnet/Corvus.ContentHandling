// <copyright file="ConverterEdgeCaseTests.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Json.Tests
{
    using System;
    using System.Text.Json;
    using Corvus.ContentHandling;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Shouldly;

    /// <summary>
    /// Regression tests for converter edge cases: malformed envelopes must fail with
    /// <see cref="JsonException"/> (not crash), non-transient DI-constructed content must be
    /// rejected rather than silently shared, and content-type discovery for envelopes must
    /// see the payload's runtime type.
    /// </summary>
    [TestClass]
    public class ConverterEdgeCaseTests
    {
        [TestMethod]
        public void Envelope_WithoutContentType_ThrowsJsonException()
        {
            using ServiceProvider provider = JsonTestContainer.Build();

            Should.Throw<JsonException>(() =>
                JsonSerializer.Deserialize<Json.ContentEnvelope>("""{"payload":{"someValue":"x"}}""", provider.GetOptions()));
        }

        [TestMethod]
        public void Envelope_WithNullPayload_ThrowsJsonException()
        {
            using ServiceProvider provider = JsonTestContainer.Build();

            Should.Throw<JsonException>(() =>
                JsonSerializer.Deserialize<Json.ContentEnvelope>("""{"contentType":"application/vnd.corvus.jsontest.interfacecontent","payload":null}""", provider.GetOptions()));
        }

        [TestMethod]
        public void Envelope_WithMissingPayload_ThrowsJsonException()
        {
            using ServiceProvider provider = JsonTestContainer.Build();

            Should.Throw<JsonException>(() =>
                JsonSerializer.Deserialize<Json.ContentEnvelope>("""{"contentType":"application/vnd.corvus.jsontest.interfacecontent"}""", provider.GetOptions()));
        }

        [TestMethod]
        public void Envelope_FromNonObjectJson_ThrowsJsonException()
        {
            using ServiceProvider provider = JsonTestContainer.Build();

            Should.Throw<JsonException>(() =>
                JsonSerializer.Deserialize<Json.ContentEnvelope>("\"not an envelope\"", provider.GetOptions()));
        }

        [TestMethod]
        public void EnvelopeFromJson_WithoutContentTypeProperty_ThrowsArgumentException()
        {
            Should.Throw<ArgumentException>(() =>
                Json.ContentEnvelope.FromJson("""{"payload":{"someValue":"x"}}"""));
        }

        [TestMethod]
        public void EnvelopeFromPayload_HeldThroughInterfaceVariable_DerivesContentTypeFromInstance()
        {
            using ServiceProvider provider = JsonTestContainer.Build();

            // The static type parameter is the interface, which declares no content type of
            // its own; discovery must see the payload's runtime type.
            ISomeContentInterface payload = new InterfaceContent { SomeValue = "hello" };
            var envelope = Json.ContentEnvelope.FromPayload(payload, provider.GetOptions());

            envelope.PayloadContentType.ShouldBe(InterfaceContent.RegisteredContentType);
        }

        [TestMethod]
        public void DeserializingSingletonDiConstructedContent_ThrowsNotSupported()
        {
            const string singletonContentType = "application/vnd.corvus.jsontest.singletondicontent";
            using ServiceProvider provider = JsonTestContainer.Build(
                configureServices: services => services.AddSingleton<ISampleService, StubService>(),
                configureContent: content => content.AddSingletonContent<DiInitializedContent>(singletonContentType));

            // Deserialization populates the resolved instance, so a singleton registration
            // would hand every caller the same repeatedly overwritten object. That must be
            // an explicit error, not silent data corruption.
            string json = $$"""{"contentType":"{{singletonContentType}}","someValue":"x"}""";
            NotSupportedException exception = Should.Throw<NotSupportedException>(() =>
                JsonSerializer.Deserialize<ISomeContentInterface>(json, provider.GetOptions()));

            exception.Message.ShouldContain("Singleton");
            exception.Message.ShouldContain("AddTransientContent");
        }

        [TestMethod]
        public void DeserializingTransientDiConstructedContent_ReturnsDistinctInstances()
        {
            using ServiceProvider provider = JsonTestContainer.Build(
                configureServices: services => services.AddSingleton<ISampleService, StubService>());

            string json = $$"""{"contentType":"{{DiInitializedContent.RegisteredContentType}}","someValue":"first"}""";
            var first = (DiInitializedContent)JsonSerializer.Deserialize<ISomeContentInterface>(json, provider.GetOptions())!;
            var second = (DiInitializedContent)JsonSerializer.Deserialize<ISomeContentInterface>(
                $$"""{"contentType":"{{DiInitializedContent.RegisteredContentType}}","someValue":"second"}""", provider.GetOptions())!;

            second.ShouldNotBeSameAs(first);
            first.SomeValue.ShouldBe("first");
            second.SomeValue.ShouldBe("second");
        }

        private sealed class StubService : ISampleService
        {
            public string Describe() => "stub";
        }
    }
}
