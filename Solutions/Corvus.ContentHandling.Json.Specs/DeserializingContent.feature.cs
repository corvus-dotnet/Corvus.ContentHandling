// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:3.0.0.0
//      SpecFlow Generator Version:3.0.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace Corvus.ContentHandling.Json.Specs
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.0.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("Deserializing content")]
    [NUnit.Framework.CategoryAttribute("setupContainer")]
    public partial class DeserializingContentFeature
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "DeserializingContent.feature"
#line hidden
        
        [NUnit.Framework.OneTimeSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Deserializing content", "\tIn order to support polymorphic content type serialization\r\n\tAs a developer\r\n\tI " +
                    "want to be able to deserialize polymorphic types from a JSON format", ProgrammingLanguage.CSharp, new string[] {
                        "setupContainer"});
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [NUnit.Framework.OneTimeTearDownAttribute()]
        public virtual void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [NUnit.Framework.SetUpAttribute()]
        public virtual void TestInitialize()
        {
        }
        
        [NUnit.Framework.TearDownAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioInitialize(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioInitialize(scenarioInfo);
            testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<NUnit.Framework.TestContext>(NUnit.Framework.TestContext.CurrentContext);
        }
        
        public virtual void ScenarioStart()
        {
            testRunner.OnScenarioStart();
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Deserialize a polymorphic content object implementing a common interface")]
        public virtual void DeserializeAPolymorphicContentObjectImplementingACommonInterface()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Deserialize a polymorphic content object implementing a common interface", null, ((string[])(null)));
#line 8
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 9
 testRunner.When("I deserialize the json object \'{ \"contentType\": \"application/vnd.corvus.someconte" +
                    "ntwithinterface\", \"someValue\": \"Hello\" }\' to the common interface as \'result\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "SomeValue"});
            table1.AddRow(new string[] {
                        "Hello"});
#line 10
 testRunner.Then("the value called \'result\' should match the content object with content type \'appl" +
                    "ication/vnd.corvus.somecontentwithinterface\'", ((string)(null)), table1, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Deserialize a polymorphic content object implementing a common interface with a n" +
            "ull polymorphic child object")]
        [NUnit.Framework.CategoryAttribute("useChildObjects")]
        public virtual void DeserializeAPolymorphicContentObjectImplementingACommonInterfaceWithANullPolymorphicChildObject()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Deserialize a polymorphic content object implementing a common interface with a n" +
                    "ull polymorphic child object", null, new string[] {
                        "useChildObjects"});
#line 15
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 16
 testRunner.When("I deserialize the json object \'{ \"contentType\": \"application/vnd.corvus.someconte" +
                    "ntwithinterfaceandchild\", \"someValue\": \"Hello\", \"child\": null }\' to the common i" +
                    "nterface as \'result\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "SomeValue",
                        "Child"});
            table2.AddRow(new string[] {
                        "Hello",
                        "null"});
#line 17
 testRunner.Then("the value called \'result\' should match the content object with content type \'appl" +
                    "ication/vnd.corvus.somecontentwithinterfaceandchild\'", ((string)(null)), table2, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Deserialize a polymorphic content object implementing a common interface with a p" +
            "olymorphic child object")]
        [NUnit.Framework.CategoryAttribute("useChildObjects")]
        public virtual void DeserializeAPolymorphicContentObjectImplementingACommonInterfaceWithAPolymorphicChildObject()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Deserialize a polymorphic content object implementing a common interface with a p" +
                    "olymorphic child object", null, new string[] {
                        "useChildObjects"});
#line 22
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "SomeValue"});
            table3.AddRow(new string[] {
                        "Dolly"});
#line 23
 testRunner.Given("I have an instance of a content object called \'child\' with content type \'applicat" +
                    "ion/vnd.corvus.somecontentwithinterface\'", ((string)(null)), table3, "Given ");
#line 26
 testRunner.When(@"I deserialize the json object '{ ""contentType"": ""application/vnd.corvus.somecontentwithinterfaceandchild"", ""someValue"": ""Hello"", ""child"": { ""contentType"": ""application/vnd.corvus.somecontentwithinterface"", ""someValue"": ""Dolly"" } }' to the common interface as 'result'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "SomeValue",
                        "Child"});
            table4.AddRow(new string[] {
                        "Hello",
                        "{child}"});
#line 27
 testRunner.Then("the value called \'result\' should match the content object with content type \'appl" +
                    "ication/vnd.corvus.somecontentwithinterfaceandchild\'", ((string)(null)), table4, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Deserialize a polymorphic content object implementing a common interface with a P" +
            "OC child object")]
        [NUnit.Framework.CategoryAttribute("useChildObjects")]
        public virtual void DeserializeAPolymorphicContentObjectImplementingACommonInterfaceWithAPOCChildObject()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Deserialize a polymorphic content object implementing a common interface with a P" +
                    "OC child object", null, new string[] {
                        "useChildObjects"});
#line 32
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "SomeValue"});
            table5.AddRow(new string[] {
                        "Dolly"});
#line 33
 testRunner.Given("I have an instance of a poc object called \'child\'", ((string)(null)), table5, "Given ");
#line 36
 testRunner.When("I deserialize the json object \'{ \"contentType\": \"application/vnd.corvus.someconte" +
                    "ntwithinterfaceandpocchild\", \"someValue\": \"Hello\", \"child\": { \"someValue\": \"Doll" +
                    "y\" } }\' to the common interface as \'result\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table6 = new TechTalk.SpecFlow.Table(new string[] {
                        "SomeValue",
                        "Child"});
            table6.AddRow(new string[] {
                        "Hello",
                        "{child}"});
#line 37
 testRunner.Then("the value called \'result\' should match the content object with content type \'appl" +
                    "ication/vnd.corvus.somecontentwithinterfaceandpocchild\'", ((string)(null)), table6, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Deserialize a polymorphic content object implementing a common base")]
        public virtual void DeserializeAPolymorphicContentObjectImplementingACommonBase()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Deserialize a polymorphic content object implementing a common base", null, ((string[])(null)));
#line 43
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 44
 testRunner.When("I deserialize the json object \'{ \"contentType\": \"application/vnd.corvus.someconte" +
                    "ntwithbase\", \"someValue\": \"Hello\" }\' to the common base as \'result\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table7 = new TechTalk.SpecFlow.Table(new string[] {
                        "SomeValue"});
            table7.AddRow(new string[] {
                        "Hello"});
#line 45
 testRunner.Then("the value called \'result\' should match the content object with content type \'appl" +
                    "ication/vnd.corvus.somecontentwithbase\'", ((string)(null)), table7, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Deserialize a polymorphic content object implementing a common base with a null p" +
            "olymorphic child object")]
        [NUnit.Framework.CategoryAttribute("useChildObjects")]
        public virtual void DeserializeAPolymorphicContentObjectImplementingACommonBaseWithANullPolymorphicChildObject()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Deserialize a polymorphic content object implementing a common base with a null p" +
                    "olymorphic child object", null, new string[] {
                        "useChildObjects"});
#line 50
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 51
 testRunner.When("I deserialize the json object \'{ \"contentType\": \"application/vnd.corvus.someconte" +
                    "ntwithbaseandchild\", \"someValue\": \"Hello\", \"child\": null }\' to the common base a" +
                    "s \'result\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table8 = new TechTalk.SpecFlow.Table(new string[] {
                        "SomeValue",
                        "Child"});
            table8.AddRow(new string[] {
                        "Hello",
                        "null"});
#line 52
 testRunner.Then("the value called \'result\' should match the content object with content type \'appl" +
                    "ication/vnd.corvus.somecontentwithbaseandchild\'", ((string)(null)), table8, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Deserialize a polymorphic content object implementing a common base with a polymo" +
            "rphic child object")]
        [NUnit.Framework.CategoryAttribute("useChildObjects")]
        public virtual void DeserializeAPolymorphicContentObjectImplementingACommonBaseWithAPolymorphicChildObject()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Deserialize a polymorphic content object implementing a common base with a polymo" +
                    "rphic child object", null, new string[] {
                        "useChildObjects"});
#line 57
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line hidden
            TechTalk.SpecFlow.Table table9 = new TechTalk.SpecFlow.Table(new string[] {
                        "SomeValue"});
            table9.AddRow(new string[] {
                        "Dolly"});
#line 58
 testRunner.Given("I have an instance of a content object called \'child\' with content type \'applicat" +
                    "ion/vnd.corvus.somecontentwithbase\'", ((string)(null)), table9, "Given ");
#line 61
 testRunner.When("I deserialize the json object \'{ \"contentType\": \"application/vnd.corvus.someconte" +
                    "ntwithbaseandchild\", \"someValue\": \"Hello\", \"child\": { \"contentType\": \"applicatio" +
                    "n/vnd.corvus.somecontentwithbase\", \"someValue\": \"Dolly\" } }\' to the common base " +
                    "as \'result\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table10 = new TechTalk.SpecFlow.Table(new string[] {
                        "SomeValue",
                        "Child"});
            table10.AddRow(new string[] {
                        "Hello",
                        "{child}"});
#line 62
 testRunner.Then("the value called \'result\' should match the content object with content type \'appl" +
                    "ication/vnd.corvus.somecontentwithbaseandchild\'", ((string)(null)), table10, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Deserialize a polymorphic content object implementing a common base with a POC ch" +
            "ild object")]
        [NUnit.Framework.CategoryAttribute("useChildObjects")]
        public virtual void DeserializeAPolymorphicContentObjectImplementingACommonBaseWithAPOCChildObject()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Deserialize a polymorphic content object implementing a common base with a POC ch" +
                    "ild object", null, new string[] {
                        "useChildObjects"});
#line 67
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line hidden
            TechTalk.SpecFlow.Table table11 = new TechTalk.SpecFlow.Table(new string[] {
                        "SomeValue"});
            table11.AddRow(new string[] {
                        "Dolly"});
#line 68
 testRunner.Given("I have an instance of a poc object called \'child\'", ((string)(null)), table11, "Given ");
#line 71
 testRunner.When("I deserialize the json object \'{ \"contentType\": \"application/vnd.corvus.someconte" +
                    "ntwithbaseandpocchild\", \"someValue\": \"Hello\", \"child\": { \"someValue\": \"Dolly\" } " +
                    "}\' to the common base as \'result\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table12 = new TechTalk.SpecFlow.Table(new string[] {
                        "SomeValue",
                        "Child"});
            table12.AddRow(new string[] {
                        "Hello",
                        "{child}"});
#line 72
 testRunner.Then("the value called \'result\' should match the content object with content type \'appl" +
                    "ication/vnd.corvus.somecontentwithbaseandpocchild\'", ((string)(null)), table12, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Deserialize a polymorphic content object implementing a common abstract base")]
        public virtual void DeserializeAPolymorphicContentObjectImplementingACommonAbstractBase()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Deserialize a polymorphic content object implementing a common abstract base", null, ((string[])(null)));
#line 78
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 79
 testRunner.When("I deserialize the json object \'{ \"contentType\": \"application/vnd.corvus.someconte" +
                    "ntwithabstractbase\", \"someValue\": \"Hello\" }\' to the common abstract base as \'res" +
                    "ult\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table13 = new TechTalk.SpecFlow.Table(new string[] {
                        "SomeValue"});
            table13.AddRow(new string[] {
                        "Hello"});
#line 80
 testRunner.Then("the value called \'result\' should match the content object with content type \'appl" +
                    "ication/vnd.corvus.somecontentwithabstractbase\'", ((string)(null)), table13, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Deserialize a polymorphic content object implementing a common abstract base with" +
            " a null polymorphic child object")]
        [NUnit.Framework.CategoryAttribute("useChildObjects")]
        public virtual void DeserializeAPolymorphicContentObjectImplementingACommonAbstractBaseWithANullPolymorphicChildObject()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Deserialize a polymorphic content object implementing a common abstract base with" +
                    " a null polymorphic child object", null, new string[] {
                        "useChildObjects"});
#line 85
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 86
 testRunner.When("I deserialize the json object \'{ \"contentType\": \"application/vnd.corvus.someconte" +
                    "ntwithabstractbaseandchild\", \"someValue\": \"Hello\", \"child\": null }\' to the commo" +
                    "n abstract base as \'result\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table14 = new TechTalk.SpecFlow.Table(new string[] {
                        "SomeValue",
                        "Child"});
            table14.AddRow(new string[] {
                        "Hello",
                        "null"});
#line 87
 testRunner.Then("the value called \'result\' should match the content object with content type \'appl" +
                    "ication/vnd.corvus.somecontentwithabstractbaseandchild\'", ((string)(null)), table14, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Deserialize a polymorphic content object implementing a common abstract base with" +
            " a polymorphic child object")]
        [NUnit.Framework.CategoryAttribute("useChildObjects")]
        public virtual void DeserializeAPolymorphicContentObjectImplementingACommonAbstractBaseWithAPolymorphicChildObject()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Deserialize a polymorphic content object implementing a common abstract base with" +
                    " a polymorphic child object", null, new string[] {
                        "useChildObjects"});
#line 92
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line hidden
            TechTalk.SpecFlow.Table table15 = new TechTalk.SpecFlow.Table(new string[] {
                        "SomeValue"});
            table15.AddRow(new string[] {
                        "Dolly"});
#line 93
 testRunner.Given("I have an instance of a content object called \'child\' with content type \'applicat" +
                    "ion/vnd.corvus.somecontentwithabstractbase\'", ((string)(null)), table15, "Given ");
#line 96
 testRunner.When(@"I deserialize the json object '{ ""contentType"": ""application/vnd.corvus.somecontentwithabstractbaseandchild"", ""someValue"": ""Hello"", ""child"": { ""contentType"": ""application/vnd.corvus.somecontentwithabstractbase"", ""someValue"": ""Dolly"" } }' to the common abstract base as 'result'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table16 = new TechTalk.SpecFlow.Table(new string[] {
                        "SomeValue",
                        "Child"});
            table16.AddRow(new string[] {
                        "Hello",
                        "{child}"});
#line 97
 testRunner.Then("the value called \'result\' should match the content object with content type \'appl" +
                    "ication/vnd.corvus.somecontentwithabstractbaseandchild\'", ((string)(null)), table16, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Deserialize a polymorphic content object implementing a common abstract base with" +
            " a POC child object")]
        [NUnit.Framework.CategoryAttribute("useChildObjects")]
        public virtual void DeserializeAPolymorphicContentObjectImplementingACommonAbstractBaseWithAPOCChildObject()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Deserialize a polymorphic content object implementing a common abstract base with" +
                    " a POC child object", null, new string[] {
                        "useChildObjects"});
#line 102
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line hidden
            TechTalk.SpecFlow.Table table17 = new TechTalk.SpecFlow.Table(new string[] {
                        "SomeValue"});
            table17.AddRow(new string[] {
                        "Dolly"});
#line 103
 testRunner.Given("I have an instance of a poc object called \'child\'", ((string)(null)), table17, "Given ");
#line 106
 testRunner.When("I deserialize the json object \'{ \"contentType\": \"application/vnd.corvus.someconte" +
                    "ntwithabstractbaseandpocchild\", \"someValue\": \"Hello\", \"child\": { \"someValue\": \"D" +
                    "olly\" } }\' to the common abstract base as \'result\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table18 = new TechTalk.SpecFlow.Table(new string[] {
                        "SomeValue",
                        "Child"});
            table18.AddRow(new string[] {
                        "Hello",
                        "{child}"});
#line 107
 testRunner.Then("the value called \'result\' should match the content object with content type \'appl" +
                    "ication/vnd.corvus.somecontentwithabstractbaseandpocchild\'", ((string)(null)), table18, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Deserialize an object with a dictionary")]
        [NUnit.Framework.CategoryAttribute("useChildObjects")]
        public virtual void DeserializeAnObjectWithADictionary()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Deserialize an object with a dictionary", null, new string[] {
                        "useChildObjects"});
#line 112
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line hidden
            TechTalk.SpecFlow.Table table19 = new TechTalk.SpecFlow.Table(new string[] {
                        "Key",
                        "Value"});
            table19.AddRow(new string[] {
                        "Key1",
                        "Value 1"});
            table19.AddRow(new string[] {
                        "KEY2",
                        "Value 2"});
            table19.AddRow(new string[] {
                        "key3",
                        "Value 3"});
#line 113
 testRunner.Given("I have a dictionary called \'dictionary\' with values", ((string)(null)), table19, "Given ");
#line 118
 testRunner.When("I deserialize the json object \'{ \"someValue\": \"Hello\", \"dictionary\": { \"Key1\": \"V" +
                    "alue 1\", \"KEY2\": \"Value 2\", \"key3\": \"Value 3\" } }\' as a poc object with dictiona" +
                    "ry as \'result\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table20 = new TechTalk.SpecFlow.Table(new string[] {
                        "SomeValue",
                        "Dictionary"});
            table20.AddRow(new string[] {
                        "Hello",
                        "{dictionary}"});
#line 119
 testRunner.Then("the value called \'result\' should match the poc object with dictionary", ((string)(null)), table20, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion