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
    using System.IO;
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
        public void MergeShouldNotAddDuplicates()
        {
            var resource = new Resource("My_Apple", "The apple is good");

            var list1 = new ResourceList();
            list1.Items.Add(resource);

            var list2 = new ResourceList();
            list2.Items.Add(resource);

            list1.Merge(list2);

            list1.Items.Count.ShouldBe(1);
        }

        [Test]
        public void ShouldBeFullyClonable()
        {
            var apples = XmlResourceParser.ParseAsResourceList(SampleData.SampleXmlResourceString);
            var clone = apples.Clone();

            apples.Items.ShouldBe(clone.Items);
        }

        [Test]
        public void ShouldPerformAutoTransformIntoNewResourceList()
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

        [Test]
        public void ShouldTransformFiles()
        {
            var path = SampleData.GenerateRandomTempPath("TransformFilesTests");
            SampleData.CreateSampleFileHierarchy(this.FileSystem, path);

            var apples = XmlResourceParser.ParseAsResourceList(SampleData.SampleXmlFruitResourceString("Apple"));

            var appleResourceOne = apples.Items.First(x => x.Key == "Resfit_Tests_Apple_Resource_One");
            var replacementOrangeOne = new Resource(
                "Resfit_Tests_Orange_Resource_One",
                "My orange is taking over the world");

            appleResourceOne.Transforms.Add(new ResourceReplacementTransform(replacementOrangeOne));

            apples.TransformFolder(path);

            // -- Verify the files were changed as expected
            var changedAppleSourceFile = this.FileSystem.LoadFile(Path.Combine(path, "Apples.cs"));
            changedAppleSourceFile.ShouldNotContain("Resfit_Tests_Apple_Resource_One");
            changedAppleSourceFile.ShouldContain("Resfit_Tests_Orange_Resource_One");

            // -- Verify the resources where changd as expected
            var changedAppleResourcesFile = this.FileSystem.LoadFile(Path.Combine(path, "Apples.resx"));
            var changedAppleResources = XmlResourceParser.ParseAsResourceList(changedAppleResourcesFile);
            changedAppleResources.Items.ShouldNotContain(x => x.Key == "Resfit_Tests_Apple_Resource_One");
            var oranges = changedAppleResources.Items.Where(x => x.Key == "Resfit_Tests_Orange_Resource_One").ToArray();
            oranges.Count().ShouldBe(1);
            var orange = oranges.First();
            orange.Key.ShouldBe("Resfit_Tests_Orange_Resource_One");
            orange.Value.ShouldBe("My orange is taking over the world");
        }
    }
}
