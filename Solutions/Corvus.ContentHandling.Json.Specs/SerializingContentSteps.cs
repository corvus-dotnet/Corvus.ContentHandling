// <copyright file="SerializingContentSteps.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Json.Specs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.Json;
    using System.Text.Json.JsonDiffPatch;
    using System.Text.Json.Nodes;

    using Corvus.ContentHandling.Json.Specs.Samples;
    using Corvus.Json.Serialization;
    using Corvus.Testing.SpecFlow;

    using Microsoft.Extensions.DependencyInjection;

    using NUnit.Framework;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    [Binding]
    public class SerializingContentSteps
    {
        private readonly FeatureContext featureContext;
        private readonly ScenarioContext scenarioContext;
        private readonly IJsonSerializerOptionsProvider optionsProvider;
        private readonly Dictionary<string, object> inputs = new();

        private string? serialized;

        public SerializingContentSteps(FeatureContext featureContext, ScenarioContext scenarioContext)
        {
            this.featureContext = featureContext;
            this.scenarioContext = scenarioContext;

            IServiceProvider serviceProvider = ContainerBindings.GetServiceProvider(this.featureContext);
            this.optionsProvider = serviceProvider.GetRequiredService<IJsonSerializerOptionsProvider>();
        }

        [Given("I have an instance of a content object called '(.*)' with content type '(.*)'")]
        public void GivenIHaveAnInstanceOfAContentObjectCalled(string instanceName, string contentType, Table table)
        {
            Assert.AreEqual(1, table.RowCount);

            object instance = table.CreateInstance(() => ContainerBindings.GetServiceProvider(this.featureContext).GetRequiredContent(contentType));

            this.inputs.Add(instanceName, instance);
        }

        [Given("I have an instance of a content object called '(.*)' with content type '(.*)' available as a child object")]
        public void GivenIHaveAnInstanceOfAContentObjectCalledAsChild(string instanceName, string contentType, Table table)
        {
            Assert.AreEqual(1, table.RowCount);

            object instance = table.CreateInstance(() => ContainerBindings.GetServiceProvider(this.featureContext).GetRequiredContent(contentType));

            this.scenarioContext.Set(instance, instanceName);
        }

        [Given("I have a dictionary called '(.*)' with values available as a child object")]
        public void GivenIHaveADictionaryCalledWithValues(string dictionaryName, Table table)
        {
            var dictionary = table.Rows.ToDictionary(x => x["Key"], x => x["Value"]);

            this.scenarioContext.Set(dictionary, dictionaryName);
        }

        [Given("I have an instance of a poc object called '([^']*)' available as a child object")]
        public void GivenIHaveAnInstanceOfAPocObjectCalled(string name, Table table)
        {
            this.scenarioContext.Set(table.CreateInstance<PocObject>(), name);
        }

        [Given("I have an instance of a poc object with enum called '(.*)'")]
        public void GivenIHaveAnInstanceOfAPocObjectWithEnumCalled(string name, Table table)
        {
            this.inputs.Add(name, table.CreateInstance<PocObjectWithEnum>());
        }

        [When("I serialize the content object called '([^']*)'")]
        public void WhenISerializeTheContentObjectCalled(string instanceName)
        {
            this.serialized = JsonSerializer.Serialize(this.inputs[instanceName], this.optionsProvider.Instance);
        }

        [Then("the serialized result should be a json object '(.*)'")]
        public void ThenTheResultShouldBeAJsonObject(string jsonString)
        {
            var actual = JsonNode.Parse(this.serialized!);

            var expected = JsonNode.Parse(jsonString);

            Assert.IsTrue(actual.DeepEquals(expected));
        }
    }
}