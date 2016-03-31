// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResourceListTests.cs" company="Tygertec">
//   Copyright © 2016 Ty Walls.
//   All rights reserved.
// </copyright>
// <summary>
//   Defines the ResourceListTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TW.Resfit.Core.Tests
{
    using System.Linq;
    using System.Text.RegularExpressions;

    using NUnit.Framework;
    using Shouldly;
    using TW.Resfit.Core;
    using TW.Resfit.Core.Transforms;
    using TW.Resfit.FileUtils.HierarchyBuilder;
    using TW.Resfit.Framework.Testing;

    [TestFixture]
    public class ResourceListTests : UnitTests
    {
        [Test]
        public void MergesTwoResourceLists()
        {
            var apples = XmlResourceParser.ParseAsResourceList(SampleData.SampleXmlFruitResourceString("apples"));
            var oranges = XmlResourceParser.ParseAsResourceList(SampleData.SampleXmlFruitResourceString("oranges"));

            var applesAndOranges = apples.Clone();

            applesAndOranges.Merge(oranges);

            apples.ShouldBeSubsetOf(applesAndOranges);
            oranges.ShouldBeSubsetOf(applesAndOranges, "The Oranges resource list was not merged in.");
        }

        [Test]
        public void MergeShouldNotAddDuplicates()
        {
            var resource = new Resource("My_Apple", "The apple is good");

            var list1 = new ResourceList { resource };
            var list2 = new ResourceList { resource };

            list1.Merge(list2);

            list1.Count.ShouldBe(1);
        }

        [Test]
        public void ShouldBeFullyClonable()
        {
            var apples = XmlResourceParser.ParseAsResourceList(SampleData.SampleXmlResourceString);
            var clone = apples.Clone();

            apples.ShouldBe(clone);
        }

        [Test]
        public void ShouldPerformAutoTransformIntoNewResourceList()
        {
            var apples = XmlResourceParser.ParseAsResourceList(SampleData.SampleXmlFruitResourceString("apples"));
            var oranges = XmlResourceParser.ParseAsResourceList(SampleData.SampleXmlFruitResourceString("oranges")).ToArray();

            var orangeIndex = 0;
            foreach (var appleResource in apples)
            {
                appleResource.Transforms.Add(new ResourceReplacementTransform(oranges[orangeIndex++]));
            }

            var applesTransformedToOranges = apples.TransformSelfIntoNewList();

            applesTransformedToOranges.ShouldBeSubsetOf(oranges);
        }

        [Test]
        public void ShouldAllowFilteringResourcesOnClone()
        {
            var list = XmlResourceParser.ParseAsResourceList(SampleData.SampleXmlResourceString);

            var newList = list.Clone(new ResourceFilter { KeyRegex = new Regex(@"Kiwi") });

            newList.ShouldAllBe(x => x.Key.Contains("Kiwi"));
        }

        [Test]
        public void ShouldAllowInitializingFromCollectionOfResources()
        {
            var list = XmlResourceParser.ParseAsResourceList(SampleData.SampleXmlResourceString);
            var newList = new ResourceList(list.Resources.Select(x => x));

            newList.Count.ShouldBe(list.Count, "Unable to initialize new ResourceList from a collection of Resources");
        }
    }
}
