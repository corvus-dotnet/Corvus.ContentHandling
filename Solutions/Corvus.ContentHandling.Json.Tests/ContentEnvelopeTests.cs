// <copyright file="ContentEnvelopeTests.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Json.Tests
{
    using System.Collections.Generic;
    using System.Text.Json;
    using System.Text.Json.Nodes;
    using System.Threading.Tasks;
    using Corvus.ContentHandling;
    using Corvus.ContentHandling.Json;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NSubstitute;
    using Shouldly;

    [TestClass]
    public class ContentEnvelopeTests
    {
        [TestMethod]
        public void Envelope_SerializesToTheV4WireFormat()
        {
            using ServiceProvider provider = JsonTestContainer.Build();
            JsonSerializerOptions options = provider.GetOptions();

            var envelope = ContentEnvelope.FromPayload(new InterfaceContent { SomeValue = "enveloped" }, options);
            string json = JsonSerializer.Serialize(envelope, options);

            JsonNode? expected = JsonNode.Parse(
                """
                {
                    "contentType": "application/vnd.corvus.jsontest.interfacecontent",
                    "payload": { "contentType": "application/vnd.corvus.jsontest.interfacecontent", "someValue": "enveloped" }
                }
                """);
            global::System.Text.Json.JsonDiffPatch.JsonDiffPatcher.DeepEquals(expected, JsonNode.Parse(json))
                .ShouldBeTrue($"Actual JSON: {json}");
        }

        [TestMethod]
        public void Envelope_DeserializesFromTheV4WireFormat_RegardlessOfPropertyOrder()
        {
            using ServiceProvider provider = JsonTestContainer.Build();
            JsonSerializerOptions options = provider.GetOptions();

            // Note "payload" preceding "contentType" — the reader is order-tolerant.
            var envelope = JsonSerializer.Deserialize<ContentEnvelope>(
                """
                {
                    "payload": { "someValue": "from-old-library", "contentType": "application/vnd.corvus.jsontest.interfacecontent" },
                    "contentType": "application/vnd.corvus.jsontest.interfacecontent"
                }
                """,
                options);

            envelope!.PayloadContentType.ShouldBe("application/vnd.corvus.jsontest.interfacecontent");
            envelope.GetContents<InterfaceContent>().SomeValue.ShouldBe("from-old-library");
        }

        [TestMethod]
        public void GetContents_DeserializesPolymorphicallyThroughTheTargetInterface()
        {
            using ServiceProvider provider = JsonTestContainer.Build();
            JsonSerializerOptions options = provider.GetOptions();

            var envelope = ContentEnvelope.FromPayload(new InterfaceContent { SomeValue = "poly" }, options);

            envelope.GetContents<ISomeContentInterface>().ShouldBeOfType<InterfaceContent>().SomeValue.ShouldBe("poly");
        }

        [TestMethod]
        public void SetPayload_DerivesTheContentTypeFromThePayloadType()
        {
            using ServiceProvider provider = JsonTestContainer.Build();

            var envelope = new ContentEnvelope(provider.GetOptions());
            envelope.SetPayload(new InterfaceContent { SomeValue = "set" });

            envelope.PayloadContentType.ShouldBe(InterfaceContent.RegisteredContentType);
            envelope.TryGetPayload(out InterfaceContent payload).ShouldBeTrue();
            payload.SomeValue.ShouldBe("set");
        }

        [TestMethod]
        public void EnvelopeRoundTrip_PreservesThePayload()
        {
            using ServiceProvider provider = JsonTestContainer.Build();
            JsonSerializerOptions options = provider.GetOptions();

            var original = ContentEnvelope.FromPayload(new ContentWithEnum { EnumValue = SomeEnum.Second }, options);
            string json = JsonSerializer.Serialize(original, options);
            var roundTripped = JsonSerializer.Deserialize<ContentEnvelope>(json, options);

            roundTripped!.PayloadContentType.ShouldBe(ContentWithEnum.RegisteredContentType);
            roundTripped.GetContents<ContentWithEnum>().EnumValue.ShouldBe(SomeEnum.Second);
        }

        [TestMethod]
        public void Match_InvokesTheCaseForThePayloadContentType()
        {
            using ServiceProvider provider = JsonTestContainer.Build();
            var envelope = ContentEnvelope.FromPayload(new InterfaceContent { SomeValue = "matched" }, provider.GetOptions());

            var handled = new List<string?>();
            bool matched = envelope.Match()
                .When<ContentWithEnum>(c => handled.Add("wrong"))
                .When<InterfaceContent>(c => handled.Add(c.SomeValue))
                .Else(_ => handled.Add("else"))
                .Execute();

            matched.ShouldBeTrue();
            handled.ShouldBe(["matched"]);
        }

        [TestMethod]
        public void Match_InvokesElseWhenNoCaseMatches()
        {
            using ServiceProvider provider = JsonTestContainer.Build();
            var envelope = ContentEnvelope.FromPayload(new InterfaceContent { SomeValue = "x" }, provider.GetOptions());

            var handled = new List<string>();
            bool matched = envelope.Match()
                .When<ContentWithEnum>(_ => handled.Add("wrong"))
                .Else(e => handled.Add($"else:{e.PayloadContentType}"))
                .Execute();

            matched.ShouldBeFalse();
            handled.ShouldBe([$"else:{InterfaceContent.RegisteredContentType}"]);
        }

        [TestMethod]
        public async Task MatchAsync_InvokesTheCaseForThePayloadContentType()
        {
            using ServiceProvider provider = JsonTestContainer.Build();
            var envelope = ContentEnvelope.FromPayload(new InterfaceContent { SomeValue = "async" }, provider.GetOptions());

            var handled = new List<string?>();
            bool matched = await envelope.MatchAsync()
                .When<InterfaceContent>(async c =>
                {
                    await Task.Yield();
                    handled.Add(c.SomeValue);
                })
                .ExecuteAsync();

            matched.ShouldBeTrue();
            handled.ShouldBe(["async"]);
        }

        [TestMethod]
        public async Task DelegateEnvelopeHandler_ReceivesTheUnwrappedPayload()
        {
            var handled = new List<string?>();

            using ServiceProvider provider = JsonTestContainer.Build(configureContent: c => c
                .AddContentEnvelopeHandler<InterfaceContent>("audit", payload => handled.Add(payload.SomeValue)));

            var envelope = ContentEnvelope.FromPayload(new InterfaceContent { SomeValue = "unwrapped" }, provider.GetOptions());
            await envelope.DispatchToHandlerAsync(provider.GetRequiredService<IContentDispatcher<ContentEnvelope>>(), "audit");

            handled.ShouldBe(["unwrapped"]);
        }

        [TestMethod]
        public async Task ClassEnvelopeHandler_ReceivesTheUnwrappedPayloadAndItsDependencies()
        {
            ISampleService service = Substitute.For<ISampleService>();

            using ServiceProvider provider = JsonTestContainer.Build(
                configureServices: s => s.AddSingleton(service),
                configureContent: c => c.AddContentEnvelopeHandler<InterfaceContent, InterfaceContentHandler>("audit"));

            var envelope = ContentEnvelope.FromPayload(new InterfaceContent { SomeValue = "class-handled" }, provider.GetOptions());
            await envelope.DispatchToHandlerAsync(provider.GetRequiredService<IContentDispatcher<ContentEnvelope>>(), "audit");

            service.Received(1).Describe();
        }

        public class InterfaceContentHandler : IContentHandler<InterfaceContent>
        {
            private readonly ISampleService service;

            public InterfaceContentHandler(ISampleService service)
            {
                this.service = service;
            }

            public ValueTask HandleAsync(InterfaceContent payload, System.Threading.CancellationToken cancellationToken = default)
            {
                this.service.Describe();
                return ValueTask.CompletedTask;
            }
        }
    }
}
