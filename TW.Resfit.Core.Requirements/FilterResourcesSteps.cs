// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterResourcesSteps.cs" company="Tygertec">
//   Copyright © 2016 Ty Walls.
//   All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace TW.Resfit.Core.Requirements
{
    using System.Text.RegularExpressions;
    using System.Xml.Linq;

    using Shouldly;

    using StronglyTypedContext;
    using TechTalk.SpecFlow;

    using TW.Resfit.FileUtils.HierarchyBuilder;
    using TW.Resfit.Framework.Requirements;

    [Binding]
    [Scope(Feature = "Filter Resources")]
    public class FilterResourcesSteps : ResfitSteps
    {
        public interface IFilterResourcesStepsContext
        {
            XElement Xelement { get; set; }

            ResourceFilter ResourceFilter { get; set; }

            ResourceList ResourceList { get; set; }
        }

        [ScenarioContext]
        public virtual IFilterResourcesStepsContext Context { get; set; }

        [Given(@"an xml file containing valid resources")]
        public void GivenAnXmlFileContainingValidResources()
        {
            this.Context.Xelement = XElement.Parse(SampleData.SampleXmlResourceString);
        }

        [Given(@"I have a filter criterion matching one or more of those resource keys")]
        public void GivenIHaveAFilterCriterionMatchingOneOrMoreOfThoseResourceKeys()
        {
            this.Context.ResourceFilter = new ResourceFilter { KeyRegex = new Regex(@"Kiwi") };
        }

        [Given(@"I have a filter criterion matching one or more of those resource values")]
        public void GivenIHaveAFilterCriterionMatchingOneOrMoreOfThoseResourceValues()
        {
            this.Context.ResourceFilter = new ResourceFilter { ValueRegex = new Regex(@"Kiwi resource") };
        }

        [When(@"I load the resources from the file into a new resource list")]
        public void WhenILoadTheResourcesFromTheFileIntoANewResourceList()
        {
            this.Context.ResourceList = XmlResourceParser.ParseAsResourceList(this.Context.Xelement, this.Context.ResourceFilter);
        }

        [Then(@"the list contains only the resources matching the filter criterion")]
        public void ThenTheListContainsOnlyTheResourcesMatchingTheFilterCriterion()
        {
            this.Context.ResourceList.ShouldAllBe(x => x.Key.Contains("Kiwi"));
        }
    }
}
