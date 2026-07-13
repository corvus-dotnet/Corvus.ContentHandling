// <copyright file="PolymorphicDeserializationTests.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Json.Tests
{
    using System;
    using System.Text.Json;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NSubstitute;
    using Shouldly;

    [TestClass]
    public class PolymorphicDeserializationTests
    {
        [TestMethod]
        public void InterfaceTarget_DeserializesToTheConcreteType()
        {
            using ServiceProvider provider = JsonTestContainer.Build();

            ISomeContentInterface? content = JsonSerializer.Deserialize<ISomeContentInterface>(
                """{"contentType":"application/vnd.corvus.jsontest.interfacecontent","someValue":"hello"}""",
                provider.GetOptions());

            content.ShouldBeOfType<InterfaceContent>().SomeValue.ShouldBe("hello");
        }

        [TestMethod]
        public void AbstractBaseTarget_DeserializesToTheConcreteType()
        {
            using ServiceProvider provider = JsonTestContainer.Build();

            SomeContentAbstractBase? content = JsonSerializer.Deserialize<SomeContentAbstractBase>(
                """{"contentType":"application/vnd.corvus.jsontest.abstractbasecontent","someValue":"abstract"}""",
                provider.GetOptions());

            content.ShouldBeOfType<AbstractBaseContent>().SomeValue.ShouldBe("abstract");
        }

        [TestMethod]
        public void ConcreteBaseTarget_DeserializesToTheDerivedType()
        {
            using ServiceProvider provider = JsonTestContainer.Build();

            SomeContentBase? content = JsonSerializer.Deserialize<SomeContentBase>(
                """{"contentType":"application/vnd.corvus.jsontest.base.derived","baseValue":"b","derivedValue":"d"}""",
                provider.GetOptions());

            DerivedFromBaseContent derived = content.ShouldBeOfType<DerivedFromBaseContent>();
            derived.BaseValue.ShouldBe("b");
            derived.DerivedValue.ShouldBe("d");
        }

        [TestMethod]
        public void DiscriminatorDoesNotHaveToBeTheFirstProperty()
        {
            using ServiceProvider provider = JsonTestContainer.Build();

            ISomeContentInterface? content = JsonSerializer.Deserialize<ISomeContentInterface>(
                """{"someValue":"order-insensitive","contentType":"application/vnd.corvus.jsontest.interfacecontent"}""",
                provider.GetOptions());

            content.ShouldBeOfType<InterfaceContent>().SomeValue.ShouldBe("order-insensitive");
        }

        [TestMethod]
        public void NestedPolymorphicChild_Deserializes()
        {
            using ServiceProvider provider = JsonTestContainer.Build();

            ISomeContentInterface? content = JsonSerializer.Deserialize<ISomeContentInterface>(
                """
                {
                    "contentType": "application/vnd.corvus.jsontest.interfacecontentwithchild",
                    "someValue": "parent",
                    "child": { "someValue": "child", "contentType": "application/vnd.corvus.jsontest.interfacecontent" }
                }
                """,
                provider.GetOptions());

            InterfaceContentWithChild parent = content.ShouldBeOfType<InterfaceContentWithChild>();
            parent.SomeValue.ShouldBe("parent");
            parent.Child.ShouldBeOfType<InterfaceContent>().SomeValue.ShouldBe("child");
        }

        [TestMethod]
        public void NullPolymorphicChild_DeserializesAsNull()
        {
            using ServiceProvider provider = JsonTestContainer.Build();

            ISomeContentInterface? content = JsonSerializer.Deserialize<ISomeContentInterface>(
                """{"contentType":"application/vnd.corvus.jsontest.interfacecontentwithchild","someValue":"parent","child":null}""",
                provider.GetOptions());

            content.ShouldBeOfType<InterfaceContentWithChild>().Child.ShouldBeNull();
        }

        [TestMethod]
        public void PocChild_Deserializes()
        {
            using ServiceProvider provider = JsonTestContainer.Build();

            ISomeContentInterface? content = JsonSerializer.Deserialize<ISomeContentInterface>(
                """{"contentType":"application/vnd.corvus.jsontest.interfacecontentwithpocchild","child":{"name":"poc"}}""",
                provider.GetOptions());

            content.ShouldBeOfType<InterfaceContentWithPocChild>().Child!.Name.ShouldBe("poc");
        }

        [TestMethod]
        public void CtorInitializedPocChild_DeserializesThroughItsConstructor()
        {
            using ServiceProvider provider = JsonTestContainer.Build();

            ISomeContentInterface? content = JsonSerializer.Deserialize<ISomeContentInterface>(
                """{"contentType":"application/vnd.corvus.jsontest.interfacecontentwithctorpocchild","child":{"name":"ctor","count":3}}""",
                provider.GetOptions());

            PocObjectCtorInitialized child = content.ShouldBeOfType<InterfaceContentWithCtorInitializedPocChild>().Child!;
            child.Name.ShouldBe("ctor");
            child.Count.ShouldBe(3);
        }

        [TestMethod]
        public void DictionaryContent_Deserializes()
        {
            using ServiceProvider provider = JsonTestContainer.Build();

            ISomeContentInterface? content = JsonSerializer.Deserialize<ISomeContentInterface>(
                """{"contentType":"application/vnd.corvus.jsontest.contentwithdictionary","values":{"a":"1","b":"2"}}""",
                provider.GetOptions());

            ContentWithDictionary withDictionary = content.ShouldBeOfType<ContentWithDictionary>();
            withDictionary.Values!["a"].ShouldBe("1");
            withDictionary.Values["b"].ShouldBe("2");
        }

        [TestMethod]
        public void EnumContent_DeserializesFromANumber()
        {
            using ServiceProvider provider = JsonTestContainer.Build();

            ISomeContentInterface? content = JsonSerializer.Deserialize<ISomeContentInterface>(
                """{"contentType":"application/vnd.corvus.jsontest.contentwithenum","enumValue":1}""",
                provider.GetOptions());

            content.ShouldBeOfType<ContentWithEnum>().EnumValue.ShouldBe(SomeEnum.Second);
        }

        [TestMethod]
        public void DiInitializedContent_IsResolvedFromTheContainerAndPopulated()
        {
            ISampleService service = Substitute.For<ISampleService>();
            using ServiceProvider provider = JsonTestContainer.Build(s => s.AddSingleton(service));

            ISomeContentInterface? content = JsonSerializer.Deserialize<ISomeContentInterface>(
                """{"contentType":"application/vnd.corvus.jsontest.diinitializedcontent","someValue":"from-di"}""",
                provider.GetOptions());

            DiInitializedContent diContent = content.ShouldBeOfType<DiInitializedContent>();
            diContent.SomeValue.ShouldBe("from-di");
            diContent.Service.ShouldBeSameAs(service);
        }

        [TestMethod]
        public void NestedDiInitializedContent_PopulatesBothInstances()
        {
            // Pins the populate mechanism's reentrancy handling: a DI-constructed child inside
            // DI-constructed content, with a sibling property after the child to prove the
            // parent's populate continues correctly once the nested populate completes.
            ISampleService service = Substitute.For<ISampleService>();
            using ServiceProvider provider = JsonTestContainer.Build(s => s.AddSingleton(service));

            ISomeContentInterface? content = JsonSerializer.Deserialize<ISomeContentInterface>(
                """
                {
                    "contentType": "application/vnd.corvus.jsontest.diinitializedcontent",
                    "child": {
                        "contentType": "application/vnd.corvus.jsontest.diinitializedcontent",
                        "someValue": "inner"
                    },
                    "someValue": "outer"
                }
                """,
                provider.GetOptions());

            DiInitializedContent outer = content.ShouldBeOfType<DiInitializedContent>();
            outer.SomeValue.ShouldBe("outer");
            outer.Service.ShouldBeSameAs(service);
            DiInitializedContent inner = outer.Child.ShouldBeOfType<DiInitializedContent>();
            inner.SomeValue.ShouldBe("inner");
            inner.ShouldNotBeSameAs(outer);
        }

        [TestMethod]
        public void UnknownContentType_Throws()
        {
            using ServiceProvider provider = JsonTestContainer.Build();

            Should.Throw<InvalidOperationException>(() => JsonSerializer.Deserialize<ISomeContentInterface>(
                """{"contentType":"application/vnd.other.unknown","someValue":"x"}""",
                provider.GetOptions()));
        }

        [TestMethod]
        public void MissingDiscriminator_ThrowsJsonException()
        {
            using ServiceProvider provider = JsonTestContainer.Build();

            Should.Throw<JsonException>(() => JsonSerializer.Deserialize<ISomeContentInterface>(
                """{"someValue":"x"}""",
                provider.GetOptions()));
        }

        [TestMethod]
        public void UnregisteredSubtype_FallsBackToTheRegisteredParentContentType()
        {
            // The key feature applied to deserialization: a document stamped with a more
            // specific content type deserializes as the registered parent type.
            using ServiceProvider provider = JsonTestContainer.Build();

            ISomeContentInterface? content = JsonSerializer.Deserialize<ISomeContentInterface>(
                """{"contentType":"application/vnd.corvus.jsontest.interfacecontent.v2.special","someValue":"fallback"}""",
                provider.GetOptions());

            content.ShouldBeOfType<InterfaceContent>().SomeValue.ShouldBe("fallback");
        }

        [TestMethod]
        public void ConcreteTypeEqualToTarget_ThrowsNotSupported()
        {
            using ServiceProvider provider = JsonTestContainer.Build(
                configureContent: c => c.AddSerializedContent<SomeContentBase>());

            Should.Throw<NotSupportedException>(() => JsonSerializer.Deserialize<SomeContentBase>(
                """{"contentType":"application/vnd.corvus.jsontest.base","baseValue":"x"}""",
                provider.GetOptions()));
        }
    }
}
