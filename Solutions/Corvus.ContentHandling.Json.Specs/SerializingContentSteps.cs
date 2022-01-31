// <copyright file="SerializingContentSteps.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Json.Specs
{
    using System.Linq;

    using Corvus.ContentHandling.Json.Specs.Samples;
    using Corvus.Extensions.Json;
    using Corvus.Testing.SpecFlow;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using NUnit.Framework;
    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    [Binding]
    public class SerializingContentSteps
    {
        private readonly FeatureContext featureContext;
        private readonly ScenarioContext scenarioContext;

        public SerializingContentSteps(FeatureContext featureContext, ScenarioContext scenarioContext)
        {
            this.featureContext = featureContext;
            this.scenarioContext = scenarioContext;
        }

        [Given("I have an instance of a content object called '(.*)' with content type '(.*)'")]
        public void GivenIHaveAnInstanceOfAContentObjectCalled(string instanceName, string contentType, Table table)
        {
            Assert.AreEqual(1, table.RowCount);

            object instance = table.CreateInstance(() => ContainerBindings.GetServiceProvider(this.featureContext).GetContent(contentType));

            this.scenarioContext.Set(instance, instanceName);
        }

        [Given("I have a dictionary called '(.*)' with values")]
        public void GivenIHaveADictionaryCalledWithValues(string dictionaryName, Table table)
        {
            var dictionary = table.Rows.ToDictionary(x => x["Key"], x => x["Value"]);

            this.scenarioContext.Set(dictionary, dictionaryName);
        }

        [Given("I have an instance of a poc object called '(.*)'")]
        public void GivenIHaveAnInstanceOfAPocObjectCalled(string name, Table table)
        {
            this.scenarioContext.Set(table.CreateInstance<PocObject>(), name);
        }

        [Given("I have an instance of a poc object with enum called '(.*)'")]
        public void GivenIHaveAnInstanceOfAPocObjectWithEnumCalled(string name, Table table)
        {
            this.scenarioContext.Set(table.CreateInstance<PocObjectWithEnum>(), name);
        }

        [Given("I have an instance of a poc object with dictionary called '(.*)'")]
        public void GivenIHaveAnInstanceOfAPocObjectCalledWithDictionaryCalled(string name, Table table)
        {
            PocObjectWithDictionary obj = table.CreateInstance<PocObjectWithDictionary>();
            this.scenarioContext.Set(obj, name);
        }

        [When("I serialize the content object called '(.*)' as '(.*)'")]
        public void WhenISerializeTheContentObjectCalled(string instanceName, string resultName)
        {
            object instance = this.scenarioContext.Get<object>(instanceName);
            string serializedValue = JsonConvert.SerializeObject(instance, ContainerBindings.GetServiceProvider(this.featureContext).GetRequiredService<IJsonSerializerSettingsProvider>().Instance);
            this.scenarioContext.Set(serializedValue, resultName);
        }

        [Then("the value called '(.*)' should be a json object '(.*)'")]
        public void ThenTheResultShouldBeAJsonObject(string resultName, string jsonString)
        {
            string result = this.scenarioContext.Get<string>(resultName);
            var actual = JObject.Parse(result);

            var expected = JObject.Parse(jsonString);

            Assert.True(JToken.DeepEquals(expected, actual));
        }
    }
}