// <copyright file="DeserializingContentSteps.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Json.Specs
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json;

    using Corvus.ContentHandling.Json.Specs.Samples;
    using Corvus.Json.Serialization;
    using Corvus.Testing.SpecFlow;
    using Microsoft.Extensions.DependencyInjection;
    using NUnit.Framework;
    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    [Binding]
    public class DeserializingContentSteps
    {
        private readonly FeatureContext featureContext;
        private readonly ScenarioContext scenarioContext;
        private readonly IJsonSerializerOptionsProvider optionsProvider;

        private Dictionary<string, object?> deserializedItems = new();

        public DeserializingContentSteps(
            FeatureContext featureContext,
            ScenarioContext scenarioContext)
        {
            this.featureContext = featureContext;
            this.scenarioContext = scenarioContext;

            IServiceProvider serviceProvider = ContainerBindings.GetServiceProvider(this.featureContext);
            this.optionsProvider = serviceProvider.GetRequiredService<IJsonSerializerOptionsProvider>();
        }

        [When("I deserialize the json object '(.*)' to the common interface as '(.*)'")]
        public void WhenIDeserializeTheJsonObjectAs(string json, string instanceName)
        {
            this.deserializedItems.Add(
                instanceName,
                JsonSerializer.Deserialize<ISomeContentInterface>(json, this.optionsProvider.Instance));
        }

        [When("I deserialize the json object '(.*)' to the common base as '(.*)'")]
        public void WhenIDeserializeTheJsonObjectToTheCommonBaseAs(string json, string instanceName)
        {
            this.deserializedItems.Add(
                instanceName,
                JsonSerializer.Deserialize<SomeContentBase>(json, this.optionsProvider.Instance));
        }

        [When("I deserialize the json object '(.*)' to the common abstract base as '(.*)'")]
        public void WhenIDeserializeTheJsonObjectToTheCommonAbstractBaseAs(string json, string instanceName)
        {
            this.deserializedItems.Add(
                instanceName,
                JsonSerializer.Deserialize<SomeContentAbstractBase>(json, this.optionsProvider.Instance));
        }

        [When("I deserialize the json object '(.*)' as a poc object with dictionary as '(.*)'")]
        public void WhenIDeserializeTheJsonObjectAsAPocObjectWithDictionaryAs(string json, string instanceName)
        {
            this.deserializedItems.Add(
                instanceName,
                JsonSerializer.Deserialize<PocObjectWithDictionary>(json, this.optionsProvider.Instance));
        }

        [Then("the value called '(.*)' should match the poc object with dictionary")]
        public void ThenTheValueCalledShouldMatchThePocObjectWithDictionary(string instanceName, Table table)
        {
            PocObjectWithDictionary expected = table.CreateInstance<PocObjectWithDictionary>();
            object? actual = this.deserializedItems[instanceName];

            Assert.AreEqual(expected, actual);
        }

        [Then("the value called '(.*)' should match the content object with content type '(.*)'")]
        public void ThenTheValueCalledShouldMatchTheContentObjectWithContentType(string instanceName, string contentType, Table table)
        {
            Assert.AreEqual(1, table.RowCount);

            object expected = table.CreateInstance(() => ContainerBindings.GetServiceProvider(this.featureContext).GetContent(contentType));

            object? actual = this.deserializedItems[instanceName];

            Assert.AreEqual(expected, actual);
        }

        [Then("the value called '(.*)' should match the content object implementing a common abstract base with a POC child object via constructor with value '(.*)', and child some value '(.*)'")]
        public void ThenTheValueCalledShouldMatchTheContentObjectImplementingACommonAbstractBaseWithAPOCChildObjectViaConstructorWithValueAndChildSomeValue(
            string instanceName, string value, string childValue)
        {
            var expected = new SomeContentWithAbstractBaseAndPocChildCtorInitialized(
                value,
                new PocObjectWithCtorInitialization(childValue));

            object? actual = this.deserializedItems[instanceName];

            Assert.AreEqual(expected, actual);
        }
    }
}