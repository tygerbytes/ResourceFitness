﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MatchAndReplaceSteps.cs" company="Tygertec">
//   Copyright © 2016 Ty Walls.
//   All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace TW.Resfit.Core.Requirements
{
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;
    using Shouldly;
    using StronglyTypedContext;
    using TechTalk.SpecFlow;
    using TW.Resfit.Core;
    using TW.Resfit.Core.Transforms;
    using TW.Resfit.FileUtils;
    using TW.Resfit.FileUtils.HierarchyBuilder;
    using TW.Resfit.Framework.Requirements;

    [Binding]
    [Scope(Feature = "Batch match and replace")]
    public class MatchAndReplaceSteps : ResfitSteps
    {
        public interface ILoadingResourcesStepsContext
        {
            string XmlFileName { get; set; }

            XElement Xml { get; set; }

            ResourceList ResourceList { get; set; }

            Resource ReplacementResource { get; set; }

            string DirectoryPath { get; set; }
        }

        [ScenarioContext]
        public virtual ILoadingResourcesStepsContext Context { get; set; }

        [Given(@"a file containing a list of resource keys")]
        public void GivenAFileContainingAListOfResourceKeys()
        {
            this.Context.XmlFileName = Path.Combine(SampleData.GenerateRandomTempPath("MatchAndReplaceTests"), "XmlFile.resx");

            this.FileSystem.WriteToFile(this.Context.XmlFileName, SampleData.SampleXmlResourceString);
        }

        [When(@"I load the XML file")]
        public void WhenILoadTheXmlFile()
        {
            this.Context.Xml = this.FileSystem.LoadXmlFile(this.Context.XmlFileName);
        }

        [Then(@"it is loaded as a list of resources")]
        public void ThenItIsLoadedAsAListOfResources()
        {
            this.Context.ResourceList = XmlResourceParser.ParseAsResourceList(this.Context.Xml);

            this.Context.ResourceList.Count.ShouldBe(5);
            this.Context.ResourceList.First().Key.ShouldBe("Resfit_Tests_Banana_Resource_One");
            this.Context.ResourceList.First().Value.ShouldBe("This is the first Banana resource in the file");
            this.Context.ResourceList.Last().Key.ShouldBe("Resfit_Tests_Kiwi_Resource_Two");
            this.Context.ResourceList.Last().Value.ShouldBe("This is the second Kiwi resource in the file");
        }

        [Given(@"a list of resources")]
        public void GivenAListOfResources()
        {
            this.Context.ResourceList = XmlResourceParser.ParseAsResourceList(SampleData.SampleXmlResourceString);
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

            this.Context.ResourceList.First()
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
            this.Context.DirectoryPath = SampleData.GenerateRandomTempPath("MatchAndReplaceTests");
            this.GenerateSampleSourceFiles(this.Context.DirectoryPath);

            this.Context.ResourceList = XmlResourceParser.ParseAllResourceFiles(this.FileSystem, this.Context.DirectoryPath);

            this.Context.ResourceList.First().Transforms.Add(new ResourceReplacementTransform(new Resource("Resfit_Tests_Banana_Resource_OneReplacement", "One replaced")));
            this.Context.ResourceList.Last().Transforms.Add(new ResourceReplacementTransform(new Resource("Resfit_Tests_Banana_Resource_ThreeReplacement", "Three replaced")));
        }

        [When(@"I supply a directory of files to search and replace")]
        public void WhenISupplyADirectoryOfFilesToSearchAndReplace()
        {
        }

        [When(@"I initiate a batch resource replacement command")]
        public void WhenIInitiateABatchResourceReplacementCommand()
        {
            new FileTransformer(this.FileSystem, this.Context.ResourceList)
                .TransformDirectory(this.Context.DirectoryPath, FileFilter.Typical);
        }

        [Then(@"all of the existing resources from the resource list will be replaced with their matches")]
        public void ThenAllOfTheExistingResourcesFromTheResourceListWillBeReplacedWithTheirMatches()
        {
            var modifiedResources = XmlResourceParser.ParseAllResourceFiles(this.FileSystem, this.Context.DirectoryPath);

            var expectedResources = this.Context.ResourceList.TransformSelfIntoNewList();

            modifiedResources.Count.ShouldBe(expectedResources.Count);
            modifiedResources.ShouldBeSubsetOf(expectedResources);
        }

        private void GenerateSampleSourceFiles(string directoryPath)
        {
            var directoryPathA = Path.Combine(directoryPath, "Dir01");
            this.FileSystem.CreateDirectory(directoryPathA);
            var file01Content = SampleData.SampleXmlResourceString.Replace("LoadFromFile", "LoadFromFile01");
            this.FileSystem.WriteToFile(Path.Combine(directoryPathA, "file01.resx"), file01Content);

            var directoryPathB = Path.Combine(directoryPath, "Dir02");
            this.FileSystem.CreateDirectory(directoryPathB);
            var file02Content = SampleData.SampleXmlResourceString.Replace("LoadFromFile", "LoadFromFile02");
            this.FileSystem.WriteToFile(Path.Combine(directoryPathB, "file02.resx"), file02Content);
        }
    }
}
