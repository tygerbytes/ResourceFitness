// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MatchAndReplaceSteps.cs" company="Tygertec">
//   Copyright © 2016 Tyrone Walls.
//   All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace TW.Core.Requirements
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
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

            ResourceList ResourceList { get; set; }

            Resource ReplacementResource { get; set; }

            string FolderPath { get; set; }
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
            this.Context.ResourceList = XmlResourceParser.ParseAsResourceList(this.Context.Xml);

            this.Context.ResourceList.Items.Count.ShouldBe(3);
            this.Context.ResourceList.Items.First().Key.ShouldBe("Resfit_Tests_LoadFromFile_Resource_One");
            this.Context.ResourceList.Items.First().Value.ShouldBe("This is the first resource in the file");
            this.Context.ResourceList.Items.Last().Key.ShouldBe("Resfit_Tests_LoadFromFile_Resource_Three");
            this.Context.ResourceList.Items.Last().Value.ShouldBe("This is the third resource in the file");
        }

        [Given(@"a list of resources")]
        public void GivenAListOfResources()
        {
            this.Context.ResourceList = XmlResourceParser.ParseAsResourceList(SampleXmlResourceString);
        }

        [When(@"I match one of the resources with a replacement resource")]
        public void WhenIMatchOneOfTheResourcesWithAReplacementResource()
        {
            this.Context.ReplacementResource = new Resource("Resfit_Tests_LoadFromFile_Resource_ReplacementOne", "Replacement for resource One");
        }

        [Then(@"the replacement resource is stored alongside the original resource")]
        public void ThenTheReplacementResourceIsStoredAlongsideTheOriginalResource()
        {
            var resourceReplacementTransform = new ResourceReplacementTransform(this.Context.ReplacementResource);

            this.Context.ResourceList.Items.First()
                .Transforms.Add(resourceReplacementTransform);
        }

        [Then(@"the original resource is tagged for eventual replacement")]
        public void ThenTheOriginalResourceIsTaggedForEventualReplacement()
        {
            // Covered.
        }

        [Given(@"a list of resources with matches")]
        public void GivenAListOfResourcesWithMatches()
        {
            this.Context.FolderPath = Path.Combine(Path.GetTempPath(), "TW.Resfit.SampleSourceFiles", DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture));
            this.GenerateSampleSourceFiles(this.Context.FolderPath);

            this.Context.ResourceList = FileHelper.LoadAllResourcesFromPath(this.Context.FolderPath);

            this.Context.ResourceList.Items.First().Transforms.Add(new ResourceReplacementTransform(new Resource("Resfit_Tests_LoadFromFile_Resource_OneReplacement", "One replaced")));
            this.Context.ResourceList.Items.Last().Transforms.Add(new ResourceReplacementTransform(new Resource("Resfit_Tests_LoadFromFile_Resource_ThreeReplacement", "Three replaced")));
        }

        [When(@"I supply a directory of files to search and replace")]
        public void WhenISupplyADirectoryOfFilesToSearchAndReplace()
        {
        }

        [When(@"I initiate a batch resource replacement command")]
        public void WhenIInitiateABatchResourceReplacementCommand()
        {
            this.Context.ResourceList.TransformFolder(this.Context.FolderPath);
        }

        [Then(@"all of the existing resources from the resource list will be replaced with their matches")]
        public void ThenAllOfTheExistingResourcesFromTheResourceListWillBeReplacedWithTheirMatches()
        {
            var modifiedResources = FileHelper.LoadAllResourcesFromPath(this.Context.FolderPath);

            var expectedResources = this.Context.ResourceList.TransformSelfIntoNewList();

            expectedResources.Items.ShouldBe(expectedResources.Items, ignoreOrder: true);
        }

        private void GenerateSampleSourceFiles(string folderPath)
        {
            throw new NotImplementedException();
        }
    }
}
