// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileHierarchyBuilderTests.cs" company="Tygertec">
//   Copyright © 2016 Ty Walls.
//   All rights reserved.
// </copyright>
// <summary>
//   Defines the FileHierarchyBuilderTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TW.Resfit.FileUtils.Tests
{
    using System.IO;
    using NUnit.Framework;
    using Shouldly;
    using TW.Resfit.FileUtils.HierarchyBuilder;
    using TW.Resfit.Framework.Testing;

    [TestFixture]
    public class FileHierarchyBuilderTests : UnitTests
    {
        [Test]
        public void ShouldCreateDirecoryAtRootNode()
        {
            var rootPath = SampleData.GenerateRandomTempPath("FileHierarchyBuilderTests");

            new FileHierarchyBuilder(rootPath).Execute(this.FileSystem);

            Directory.Exists(rootPath).ShouldBe(true, string.Format("Directory does not exist: {0}", rootPath));
        }

        [Test]
        public void ShouldCreateSubDirectory()
        {
            var rootPath = SampleData.GenerateRandomTempPath("FileHierarchyBuilderTests");

            new FileHierarchyBuilder(rootPath).AddDirectory("SubDirectory").Execute(this.FileSystem);

            var pathToDirectory = Path.Combine(rootPath, "SubDirectory");
            Directory.Exists(pathToDirectory).ShouldBe(true, string.Format("Directory does not exist: {0}", pathToDirectory));
        }

        [Test]
        public void ShouldCreateSubFiles()
        {
            var rootPath = SampleData.GenerateRandomTempPath("FileHierarchyBuilderTests");

            var builder = new FileHierarchyBuilder(rootPath);
            builder.AddFiles(new[] { new FileNode("RootFile01.txt", "Contents of root file 01") })
                .AddDirectory("SubDirectory")
                .AddFiles(
                    new[]
                        {
                            new FileNode("File01.txt", "Contents of file 01"),
                            new FileNode("File02.txt", "Contents of file 02")
                        });

            builder.Execute(this.FileSystem);

            var pathToRootFile1 = Path.Combine(rootPath, "RootFile01.txt");
            var pathToFile1 = Path.Combine(rootPath, "SubDirectory", "File01.txt");
            var pathToFile2 = Path.Combine(rootPath, "SubDirectory", "File02.txt");

            File.Exists(pathToRootFile1).ShouldBe(true, string.Format("File doesn't exist: {0}", pathToRootFile1));
            File.Exists(pathToFile1).ShouldBe(true, string.Format("File doesn't exist: {0}", pathToFile1));
            File.Exists(pathToFile2).ShouldBe(true, string.Format("File doesn't exist: {0}", pathToFile2));

            var file1Contents = this.FileSystem.LoadFile(pathToFile1);
            file1Contents.ShouldBe("Contents of file 01");
        }
    }
}
