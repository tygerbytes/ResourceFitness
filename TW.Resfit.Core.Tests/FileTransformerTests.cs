// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileTransformerTests.cs" company="Tygertec">
//   Copyright © 2016 Ty Walls.
//   All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace TW.Resfit.Core.Tests
{
    using System.IO;
    using System.Linq;

    using NUnit.Framework;
    using Shouldly;
    using TW.Resfit.Core;
    using TW.Resfit.Core.Transforms;
    using TW.Resfit.FileUtils.HierarchyBuilder;
    using TW.Resfit.Framework.Testing;

    [TestFixture]
    public class FileTransformerTests : UnitTests
    {
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
            new FileTransformer(this.FileSystem, apples).TransformDirectory(path);

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
    }
}
