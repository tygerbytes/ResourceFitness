// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoadingResourcesSteps.cs" company="Tygertec">
//   Copyright © 2016 Tyrone Walls.
//   All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace TW.Core.Requirements
{
    using Shouldly;
    using StronglyTypedContext;
    using TechTalk.SpecFlow;
    using TW.Resfit.Core;
    using TW.Resfit.FileUtils;

    [Binding]
    [Scope(Feature = "Loading resources")]
    public class LoadingResourcesSteps : ResfitSteps
    {
        public interface ILoadingResourcesStepsContext
        {
            string XmlFileName { get; set; }

            string Xml { get; set; }

            XmlResourceParser XmlResourceParser { get; set; }

            ResourceList ResourceList { get; set; }
        }

        [ScenarioContext]
        public virtual ILoadingResourcesStepsContext Context { get; set; }

        [Given(@"a file containing a list of resource keys")]
        public void GivenAFileContainingAListOfResourceKeys()
        {
            this.Context.XmlFileName = "XmlFile.resx";

            const string FileContents = @"
<?xml version=""1.0"" encoding=""utf-8""?>
<root>
	<data name=""Resfit_Tests_LoadFromFile_Resource_One"" xml:space=""preserve"">
    <value>This is the first resource in the file.</value>
  </data>
  <data name=""Resfit_Tests_LoadFromFile_Resource_Two"" xml:space=""preserve"">
    <value>This is the second resource in the file</value>
  </data>
  <data name=""Resfit_Tests_LoadFromFile_Resource_Three"" xml:space=""preserve"">
    <value>This is the third resource in the file</value>
  </data>
</root>
";
            FileUtils.WriteToFile(this.Context.XmlFileName, FileContents);
        }

        [When(@"I load the xml file")]
        public void WhenILoadTheXmlFile()
        {
            this.Context.Xml = FileUtils.LoadFile(this.Context.XmlFileName);
        }

        [Then(@"it is loaded as a list of resources")]
        public void ThenItIsLoadedAsAListOfResources()
        {
            this.Context.ResourceList = this.Context.XmlResourceParser.Parse(this.Context.Xml);

            this.Context.ResourceList.Count.ShouldBe(3);
            this.Context.ResourceList.First.Key.ShouldBe("Resfit_Tests_LoadFromFile_Resource_One");
            this.Context.ResourceList.First.Value.ShouldBe("This is the first resource in the file");
            this.Context.ResourceList.Last.Key.ShouldBe("Resfit_Tests_LoadFromFile_Resource_Three");
            this.Context.ResourceList.Last.Value.ShouldBe("This is the third resource in the file");
        }
    }
}
