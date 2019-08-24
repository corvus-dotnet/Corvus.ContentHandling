// <copyright file="DeserializingContentSteps.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

#pragma warning disable SA1600 // Elements should be documented
#pragma warning disable CS1591 // Elements should be documented

namespace Corvus.ContentHandling.Json.Specs
{
    using Corvus.ContentHandling.Json.Specs.Samples;
    using Corvus.Extensions.Json;
    using Corvus.SpecFlow.Extensions;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;
    using NUnit.Framework;
    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    [Binding]
    public class DeserializingContentSteps
    {
        private readonly FeatureContext featureContext;
        private readonly ScenarioContext scenarioContext;

        public DeserializingContentSteps(FeatureContext featureContext, ScenarioContext scenarioContext)
        {
            this.featureContext = featureContext;
            this.scenarioContext = scenarioContext;
        }

        [When("I deserialize the json object '(.*)' to the common interface as '(.*)'")]
        public void WhenIDeserializeTheJsonObjectAs(string json, string instanceName)
        {
            this.scenarioContext.Set(JsonConvert.DeserializeObject<ISomeContentInterface>(json, ContainerBindings.GetServiceProvider(this.featureContext).GetRequiredService<IJsonSerializerSettingsProvider>().Instance), instanceName);
        }

        [When("I deserialize the json object '(.*)' to the common base as '(.*)'")]
        public void WhenIDeserializeTheJsonObjectToTheCommonBaseAs(string json, string instanceName)
        {
            this.scenarioContext.Set(JsonConvert.DeserializeObject<SomeContentBase>(json, ContainerBindings.GetServiceProvider(this.featureContext).GetRequiredService<IJsonSerializerSettingsProvider>().Instance), instanceName);
        }

        [When("I deserialize the json object '(.*)' to the common abstract base as '(.*)'")]
        public void WhenIDeserializeTheJsonObjectToTheCommonAbstractBaseAs(string json, string instanceName)
        {
            this.scenarioContext.Set(JsonConvert.DeserializeObject<SomeContentAbstractBase>(json, ContainerBindings.GetServiceProvider(this.featureContext).GetRequiredService<IJsonSerializerSettingsProvider>().Instance), instanceName);
        }

        [When("I deserialize the json object '(.*)' as a poc object with dictionary as '(.*)'")]
        public void WhenIDeserializeTheJsonObjectAsAPocObjectWithDictionaryAs(string json, string instanceName)
        {
            this.scenarioContext.Set(JsonConvert.DeserializeObject<PocObjectWithDictionary>(json, ContainerBindings.GetServiceProvider(this.featureContext).GetRequiredService<IJsonSerializerSettingsProvider>().Instance), instanceName);
        }

        [Then("the value called '(.*)' should match the poc object with dictionary")]
        public void ThenTheValueCalledShouldMatchThePocObjectWithDictionary(string instanceName, Table table)
        {
            PocObjectWithDictionary expected = table.CreateInstance<PocObjectWithDictionary>();
            object actual = this.scenarioContext.Get<object>(instanceName);

            Assert.AreEqual(expected, actual);
        }

        [Then("the value called '(.*)' should match the content object with content type '(.*)'")]
        public void ThenTheValueCalledShouldMatchTheContentObjectWithContentType(string instanceName, string contentType, Table table)
        {
            Assert.AreEqual(1, table.RowCount);

            object expected = table.CreateInstance(() => ContainerBindings.GetServiceProvider(this.featureContext).GetContent(contentType));

            object actual = this.scenarioContext.Get<object>(instanceName);

            Assert.AreEqual(expected, actual);
        }
    }
}

#pragma warning restore SA1600 // Elements should be documented
#pragma warning restore CS1591 // Elements should be documented