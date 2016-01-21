// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MatchAndReplaceSteps.cs" company="Tygertec">
//   Copyright © 2016 Tyrone Walls.
//   All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace TW.Core.Requirements
{
    using System.IO;
    using System.Xml.Linq;
    using Shouldly;
    using StronglyTypedContext;
    using TechTalk.SpecFlow;
    using TW.Resfit.Core;
    using TW.Resfit.FileUtils;

    [Binding]
    [Scope(Feature = "Batch match and replace")]
    public class MatchAndReplaceSteps : ResfitSteps
    {
        private const string SampleXmlResourceString = @"<?xml version=""1.0"" encoding=""utf-8""?>
<root>
	<data name=""Resfit_Tests_LoadFromFile_Resource_One"" xml:space=""preserve"">
    <value>This is the first resource in the file</value>
  </data>
  <data name=""Resfit_Tests_LoadFromFile_Resource_Two"" xml:space=""preserve"">
    <value>This is the second resource in the file</value>
  </data>
  <data name=""Resfit_Tests_LoadFromFile_Resource_Three"" xml:space=""preserve"">
    <value>This is the third resource in the file</value>
  </data>
</root>
";
        
        public interface ILoadingResourcesStepsContext
        {
            string XmlFileName { get; set; }

            XElement Xml { get; set; }

            XmlResourceParser XmlResourceParser { get; set; }

            ResourceList ResourceList { get; set; }

            MatchableResourceList MatchableResourceList { get; set; }

            Resource ReplacementResourcee { get; set; }
        }

        [ScenarioContext]
        public virtual ILoadingResourcesStepsContext Context { get; set; }

        [Given(@"a file containing a list of resource keys")]
        public void GivenAFileContainingAListOfResourceKeys()
        {
            this.Context.XmlFileName = Path.Combine(Path.GetTempPath(), "XmlFile.resx");

            FileHelper.WriteToFile(this.Context.XmlFileName, SampleXmlResourceString);
        }

        [When(@"I load the XML file")]
        public void WhenILoadTheXmlFile()
        {
            this.Context.Xml = FileHelper.LoadXmlFile(this.Context.XmlFileName);
        }

        [Then(@"it is loaded as a list of resources")]
        public void ThenItIsLoadedAsAListOfResources()
        {
            XElement xmlDocument = this.Context.Xml;
            ResourceList resourceList = this.Context.XmlResourceParser.Parse(xmlDocument);
            this.Context.ResourceList = resourceList;

            this.Context.ResourceList.Count.ShouldBe(3);
            this.Context.ResourceList.First.Key.ShouldBe("Resfit_Tests_LoadFromFile_Resource_One");
            this.Context.ResourceList.First.Value.ShouldBe("This is the first resource in the file");
            this.Context.ResourceList.Last.Key.ShouldBe("Resfit_Tests_LoadFromFile_Resource_Three");
            this.Context.ResourceList.Last.Value.ShouldBe("This is the third resource in the file");
        }

        [Given(@"a list of resources")]
        public void GivenAListOfResources()
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"I match one of the resources with a replacement resource")]
        public void WhenIMatchOneOfTheResourcesWithAReplacementResource()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"the replacement resource is stored alongside the original resource")]
        public void ThenTheReplacementResourceIsStoredAlongsideTheOriginalResource()
        {
            ScenarioContext.Current.Pending();
        }
    }
}
