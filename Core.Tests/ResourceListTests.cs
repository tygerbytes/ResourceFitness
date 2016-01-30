// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResourceListTests.cs" company="Tygertec">
//   Copyright © 2016 Tyrone Walls.
//   All rights reserved.
// </copyright>
// <summary>
//   Defines the ResourceListTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Core.Tests
{
    using System.Linq;

    using NUnit.Framework;
    using Shouldly;
    using TW.Resfit.Core;
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

            apples.Items.ShouldBeSubsetOf(applesAndOranges.Items);
            oranges.Items.ShouldBeSubsetOf(applesAndOranges.Items, "The Oranges resource list was not merged in.");
        }

        [Test]
        public void ShouldBeFullyClonable()
        {
            var apples = XmlResourceParser.ParseAsResourceList(SampleData.SampleXmlResourceString);
            var clone = apples.Clone();

            apples.Items.ShouldBe(clone.Items);
        }

        [Test]
        public void PerformsAutoTransform()
        {
            var apples = XmlResourceParser.ParseAsResourceList(SampleData.SampleXmlFruitResourceString("apples"));
            var oranges = XmlResourceParser.ParseAsResourceList(SampleData.SampleXmlFruitResourceString("oranges")).Items.ToArray();

            var orangeIndex = 0;
            foreach (var appleResource in apples.Items)
            {
                appleResource.Transforms.Add(new ResourceReplacementTransform(oranges[orangeIndex++]));
            }

            var applesTransformedToOranges = apples.TransformSelfIntoNewList();

            applesTransformedToOranges.Items.ShouldBeSubsetOf(oranges);
        }
    }
}
