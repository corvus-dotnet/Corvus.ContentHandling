// <copyright file="PolymorphicSerializationTests.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Json.Tests
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Nodes;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Shouldly;

    [TestClass]
    public class PolymorphicSerializationTests
    {
        [TestMethod]
        public void ConcreteContent_SerializesWithTheContentTypeDiscriminator()
        {
            using ServiceProvider provider = JsonTestContainer.Build();
            JsonSerializerOptions options = provider.GetOptions();

            string json = JsonSerializer.Serialize(new InterfaceContent { SomeValue = "hello" }, options);

            AssertJsonEquals(
                """{"contentType":"application/vnd.corvus.jsontest.interfacecontent","someValue":"hello"}""",
                json);
        }

        [TestMethod]
        public void InterfaceTypedValue_SerializesAsItsRuntimeType()
        {
            using ServiceProvider provider = JsonTestContainer.Build();
            JsonSerializerOptions options = provider.GetOptions();

            ISomeContentInterface content = new InterfaceContent { SomeValue = "poly" };
            string json = JsonSerializer.Serialize(content, options);

            AssertJsonEquals(
                """{"contentType":"application/vnd.corvus.jsontest.interfacecontent","someValue":"poly"}""",
                json);
        }

        [TestMethod]
        public void NestedPolymorphicChild_SerializesWithItsOwnDiscriminator()
        {
            using ServiceProvider provider = JsonTestContainer.Build();
            JsonSerializerOptions options = provider.GetOptions();

            var content = new InterfaceContentWithChild
            {
                SomeValue = "parent",
                Child = new InterfaceContent { SomeValue = "child" },
            };

            string json = JsonSerializer.Serialize(content, options);

            AssertJsonEquals(
                """
                {
                    "contentType": "application/vnd.corvus.jsontest.interfacecontentwithchild",
                    "someValue": "parent",
                    "child": { "contentType": "application/vnd.corvus.jsontest.interfacecontent", "someValue": "child" }
                }
                """,
                json);
        }

        [TestMethod]
        public void NullPolymorphicChild_IsOmittedWhenWriting()
        {
            // The Corvus.Json.Serialization options ignore nulls when writing (matching the
            // old library's behaviour); explicit nulls are still read correctly on the way in.
            using ServiceProvider provider = JsonTestContainer.Build();
            JsonSerializerOptions options = provider.GetOptions();

            string json = JsonSerializer.Serialize(new InterfaceContentWithChild { SomeValue = "parent" }, options);

            AssertJsonEquals(
                """
                {
                    "contentType": "application/vnd.corvus.jsontest.interfacecontentwithchild",
                    "someValue": "parent"
                }
                """,
                json);
        }

        [TestMethod]
        public void EnumValue_SerializesAsANumber()
        {
            using ServiceProvider provider = JsonTestContainer.Build();
            JsonSerializerOptions options = provider.GetOptions();

            string json = JsonSerializer.Serialize(new ContentWithEnum { EnumValue = SomeEnum.Third }, options);

            AssertJsonEquals(
                """{"contentType":"application/vnd.corvus.jsontest.contentwithenum","enumValue":2}""",
                json);
        }

        [TestMethod]
        public void SerializingTheTargetTypeItself_ThrowsNotSupported()
        {
            using ServiceProvider provider = JsonTestContainer.Build(
                configureContent: c => c.AddSerializedContent<SomeContentBase>());
            JsonSerializerOptions options = provider.GetOptions();

            // SomeContentBase is itself the polymorphic target, so serializing an instance
            // whose runtime type IS the target would recurse into the converter forever;
            // the converter rejects it explicitly.
            Should.Throw<NotSupportedException>(
                () => JsonSerializer.Serialize(new SomeContentBase { BaseValue = "x" }, options));
        }

        [TestMethod]
        public void RoundTrip_PreservesValuesThroughTheInterfaceTarget()
        {
            using ServiceProvider provider = JsonTestContainer.Build();
            JsonSerializerOptions options = provider.GetOptions();

            ISomeContentInterface original = new InterfaceContent { SomeValue = "round-trip" };
            string json = JsonSerializer.Serialize(original, options);
            var deserialized = (InterfaceContent)JsonSerializer.Deserialize<ISomeContentInterface>(json, options)!;

            deserialized.SomeValue.ShouldBe("round-trip");
        }

        private static void AssertJsonEquals(string expected, string actual)
        {
            JsonNode? expectedNode = JsonNode.Parse(expected);
            JsonNode? actualNode = JsonNode.Parse(actual);
            global::System.Text.Json.JsonDiffPatch.JsonDiffPatcher.DeepEquals(expectedNode, actualNode)
                .ShouldBeTrue($"Expected JSON:\n{expectedNode}\n\nActual JSON:\n{actualNode}");
        }
    }
}
