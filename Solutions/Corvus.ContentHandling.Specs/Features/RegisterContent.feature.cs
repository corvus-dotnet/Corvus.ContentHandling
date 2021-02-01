﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (https://www.specflow.org/).
//      SpecFlow Version:3.5.0.0
//      SpecFlow Generator Version:3.5.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace Corvus.ContentHandling.Specs.Features
{
    using TechTalk.SpecFlow;
    using System;
    using System.Linq;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.5.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("RegisterContent")]
    public partial class RegisterContentFeature
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
        private string[] _featureTags = ((string[])(null));
        
#line 1 "RegisterContent.feature"
#line hidden
        
        [NUnit.Framework.OneTimeSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Features", "RegisterContent", "\tIn order to create content dynamically by a media type \r\n\tAs a developer\r\n\tI wan" +
                    "t to be able to register and retrieve various kinds of content", ProgrammingLanguage.CSharp, ((string[])(null)));
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
        public virtual void TestTearDown()
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
        [NUnit.Framework.DescriptionAttribute("Register and retrieve content")]
        public virtual void RegisterAndRetrieveContent()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Register and retrieve content", null, tagsOfScenario, argumentsOfScenario);
#line 6
this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
                TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                            "Type",
                            "Explicit content type",
                            "Registration kind"});
                table1.AddRow(new string[] {
                            "ExplicitType1",
                            "application/vnd.corvus.explicittype1",
                            "Singleton"});
                table1.AddRow(new string[] {
                            "ExplicitType2",
                            "application/vnd.corvus.explicittype2",
                            "Singleton"});
                table1.AddRow(new string[] {
                            "ExplicitType1Subtype1",
                            "application/vnd.corvus.explicittype1.subtype1",
                            "Singleton"});
                table1.AddRow(new string[] {
                            "ExplicitType3",
                            "application/vnd.corvus.explicittype3",
                            "Transient"});
                table1.AddRow(new string[] {
                            "ExplicitType4",
                            "application/vnd.corvus.explicittype4",
                            "Transient"});
                table1.AddRow(new string[] {
                            "ExplicitType3Subtype1",
                            "application/vnd.corvus.explicittype3.subtype1",
                            "Transient"});
                table1.AddRow(new string[] {
                            "ExplicitType5",
                            "application/vnd.corvus.explicittype5",
                            "Scoped"});
                table1.AddRow(new string[] {
                            "ExplicitType6",
                            "application/vnd.corvus.explicittype6",
                            "Scoped"});
                table1.AddRow(new string[] {
                            "ExplicitType5Subtype1",
                            "application/vnd.corvus.explicittype5.subtype1",
                            "Scoped"});
                table1.AddRow(new string[] {
                            "ContentType1",
                            "",
                            "Singleton"});
                table1.AddRow(new string[] {
                            "ContentType2",
                            "",
                            "Singleton"});
                table1.AddRow(new string[] {
                            "ContentType1Subtype1",
                            "",
                            "Singleton"});
                table1.AddRow(new string[] {
                            "ContentType3",
                            "",
                            "Transient"});
                table1.AddRow(new string[] {
                            "ContentType4",
                            "",
                            "Transient"});
                table1.AddRow(new string[] {
                            "ContentType3Subtype1",
                            "",
                            "Transient"});
                table1.AddRow(new string[] {
                            "ContentType5",
                            "",
                            "Scoped"});
                table1.AddRow(new string[] {
                            "ContentType6",
                            "",
                            "Scoped"});
                table1.AddRow(new string[] {
                            "ContentType5Subtype1",
                            "",
                            "Scoped"});
#line 7
 testRunner.Given("I have registered the following types", ((string)(null)), table1, "Given ");
#line hidden
                TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                            "Content type"});
                table2.AddRow(new string[] {
                            "application/vnd.corvus.explicittype1"});
                table2.AddRow(new string[] {
                            "application/vnd.corvus.explicittype2"});
                table2.AddRow(new string[] {
                            "application/vnd.corvus.explicittype1.subtype1"});
                table2.AddRow(new string[] {
                            "application/vnd.corvus.explicittype2.subtype1"});
                table2.AddRow(new string[] {
                            "application/vnd.corvus.explicittype3"});
                table2.AddRow(new string[] {
                            "application/vnd.corvus.explicittype4"});
                table2.AddRow(new string[] {
                            "application/vnd.corvus.explicittype3.subtype1"});
                table2.AddRow(new string[] {
                            "application/vnd.corvus.explicittype4.subtype1"});
                table2.AddRow(new string[] {
                            "application/vnd.corvus.explicittype5"});
                table2.AddRow(new string[] {
                            "application/vnd.corvus.explicittype6"});
                table2.AddRow(new string[] {
                            "application/vnd.corvus.explicittype5.subtype1"});
                table2.AddRow(new string[] {
                            "application/vnd.corvus.explicittype6.subtype1"});
                table2.AddRow(new string[] {
                            "application/vnd.corvus.contenttype1"});
                table2.AddRow(new string[] {
                            "application/vnd.corvus.contenttype2"});
                table2.AddRow(new string[] {
                            "application/vnd.corvus.contenttype1.subtype1"});
                table2.AddRow(new string[] {
                            "application/vnd.corvus.contenttype2.subtype1"});
                table2.AddRow(new string[] {
                            "application/vnd.corvus.contenttype3"});
                table2.AddRow(new string[] {
                            "application/vnd.corvus.contenttype4"});
                table2.AddRow(new string[] {
                            "application/vnd.corvus.contenttype3.subtype1"});
                table2.AddRow(new string[] {
                            "application/vnd.corvus.contenttype4.subtype1"});
                table2.AddRow(new string[] {
                            "application/vnd.corvus.contenttype5"});
                table2.AddRow(new string[] {
                            "application/vnd.corvus.contenttype6"});
                table2.AddRow(new string[] {
                            "application/vnd.corvus.contenttype5.subtype1"});
                table2.AddRow(new string[] {
                            "application/vnd.corvus.contenttype6.subtype1"});
#line 27
 testRunner.When("I get the following content", ((string)(null)), table2, "When ");
#line hidden
                TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                            "Type"});
                table3.AddRow(new string[] {
                            "ExplicitType1"});
                table3.AddRow(new string[] {
                            "ExplicitType2"});
                table3.AddRow(new string[] {
                            "ExplicitType1Subtype1"});
                table3.AddRow(new string[] {
                            "ExplicitType2"});
                table3.AddRow(new string[] {
                            "ExplicitType3"});
                table3.AddRow(new string[] {
                            "ExplicitType4"});
                table3.AddRow(new string[] {
                            "ExplicitType3Subtype1"});
                table3.AddRow(new string[] {
                            "ExplicitType4"});
                table3.AddRow(new string[] {
                            "ExplicitType5"});
                table3.AddRow(new string[] {
                            "ExplicitType6"});
                table3.AddRow(new string[] {
                            "ExplicitType5Subtype1"});
                table3.AddRow(new string[] {
                            "ExplicitType6"});
                table3.AddRow(new string[] {
                            "ContentType1"});
                table3.AddRow(new string[] {
                            "ContentType2"});
                table3.AddRow(new string[] {
                            "ContentType1Subtype1"});
                table3.AddRow(new string[] {
                            "ContentType2"});
                table3.AddRow(new string[] {
                            "ContentType3"});
                table3.AddRow(new string[] {
                            "ContentType4"});
                table3.AddRow(new string[] {
                            "ContentType3Subtype1"});
                table3.AddRow(new string[] {
                            "ContentType4"});
                table3.AddRow(new string[] {
                            "ContentType5"});
                table3.AddRow(new string[] {
                            "ContentType6"});
                table3.AddRow(new string[] {
                            "ContentType5Subtype1"});
                table3.AddRow(new string[] {
                            "ContentType6"});
#line 53
 testRunner.Then("the results should be of types", ((string)(null)), table3, "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
