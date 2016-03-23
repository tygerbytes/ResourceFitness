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
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

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
        public void ShouldTransformFiles()
        {
            var path = SampleData.GenerateRandomTempPath("TransformFilesTests");
            SampleData.CreateSampleFileHierarchy(this.FileSystem, path);

            var apples = XmlResourceParser.ParseAsResourceList(SampleData.SampleXmlFruitResourceString("Apple"));

            var appleResourceOne = apples.First(x => x.Key == "Resfit_Tests_Apple_Resource_One");
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
            changedAppleResources.ShouldNotContain(x => x.Key == "Resfit_Tests_Apple_Resource_One");
            var oranges = changedAppleResources.Where(x => x.Key == "Resfit_Tests_Orange_Resource_One").ToArray();
            oranges.Count().ShouldBe(1);
            var orange = oranges.First();
            orange.Key.ShouldBe("Resfit_Tests_Orange_Resource_One");
            orange.Value.ShouldBe("My orange is taking over the world");
        }

        [Test]
        public void CanFilterResourcesOnClone()
        {
            var list = XmlResourceParser.ParseAsResourceList(SampleData.SampleXmlResourceString);

            var newList = list.Clone(new ResourceFilter { KeyRegex = new Regex(@"Kiwi") });

            newList.ShouldAllBe(x => x.Key.Contains("Kiwi"));
        }
    }
}
