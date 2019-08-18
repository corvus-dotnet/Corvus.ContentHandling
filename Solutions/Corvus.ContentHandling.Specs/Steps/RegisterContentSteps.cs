﻿namespace Corvus.Extensions.Specs.Steps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Corvus.Extensions.Specs.Driver;
    using Microsoft.Extensions.DependencyInjection;
    using NUnit.Framework;
    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    [Binding]
    public class RegisterContentSteps
    {
        public ScenarioContext ScenarioContext { get; }

        public RegisterContentSteps(ScenarioContext scenarioContext)
        {
            this.ScenarioContext = scenarioContext;
        }

        [Given(@"I have registered the following types")]
        public void GivenIHaveRegisteredTheFollowingTypes(Table table)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddContentFactory(contentFactory =>
            {
                IEnumerable<Registration> registrations = table.CreateSet(row =>
                {
                    return new Registration(row.GetString("Type"), row.GetString("Explicit content type"), row.GetString("Registration kind"));
                });
                foreach (Registration registration in registrations)
                {
                    registration.Register(this.ScenarioContext, contentFactory, false);
                }
            });

            this.ScenarioContext.Add("ServiceProvider", serviceCollection.BuildServiceProvider());
        }

        [When(@"I get the following content")]
        public void WhenIGetTheFollowingContent(Table table)
        {
            IServiceProvider serviceProvider = this.ScenarioContext.Get<IServiceProvider>("ServiceProvider");
            IEnumerable<string> contentTypes = table.CreateSet(row =>
            {
                return row.GetString("Content type");
            });
            var results = new List<object>();
            foreach (string contentType in contentTypes)
            {
                results.Add(serviceProvider.GetContent(contentType));
            }
            this.ScenarioContext.Add("Result", results);
        }

        [Then(@"the results should be of types")]
        public void ThenTheResultsShouldBeOfTypes(Table table)
        {
            IList<Type> types = table.CreateSet(row =>
            {
                return TypeMap.GetTypeFor(row.GetString("Type"));
            }).ToList();

            List<object> results = this.ScenarioContext.Get<List<object>>("Result");

            Assert.AreEqual(types.Count, results.Count);
            for (int i = 0; i < results.Count; ++i)
            {
                Assert.IsAssignableFrom(types[i], results[i]);
            }
        }
    }
}
