// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileEnumerationTests.cs" company="Tygertec">
//   Copyright © 2016 Ty Walls.
//   All rights reserved.
// </copyright>
// <summary>
//   Defines the FileEnumerationTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TW.Resfit.FileUtils.Tests
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;
    using NUnit.Framework;
    using Shouldly;
    using TW.Resfit.FileUtils.HierarchyBuilder;
    using TW.Resfit.Framework.Testing;

    [TestFixture]
    public class FileEnumerationTests : UnitTests
    {
        private string TestDirectoryPath { get; set; } 

        [OneTimeSetUp]
        public void SetupFixture()
        {
            this.TestDirectoryPath = SampleData.GenerateRandomTempPath("FileEnumerationTests");
            SampleData.CreateSampleFileHierarchy(this.FileSystem, this.TestDirectoryPath);
        }

        [Test]
        public void ShouldEnumerateAllFiles()
        {
            this.FileSystem.AllFiles(this.TestDirectoryPath).Count().ShouldBe(7, "There were not exactly 5 files enumerated");
        }

        [Test]
        public void ShouldIgnoreBlacklistedDirectories()
        {
            var folderBlacklist = new Regex(@"Folder1");
            var files = this.FileSystem.AllFiles(this.TestDirectoryPath, folderBlacklist).Select(x => x.DirectoryName);
            files.ShouldNotContain(x => x.Contains(@"\Folder1"));
        }

        [Test]
        public void ShouldOnlyReturnWhitelistedFilenames()
        {
            var fileExtensionWhitelist = new Regex(@"\.(?:cs|resx)");
            var fileNames = this.FileSystem.AllFiles(this.TestDirectoryPath, null, fileExtensionWhitelist).Select(x => x.Name);
            fileNames.ShouldNotContain(x => x.EndsWith(".txt"));
        }

        [Test]
        public void PreventsEnumeratingNullPath()
        {
            Should.Throw<ArgumentNullException>(() => this.FileSystem.AllFiles(string.Empty).First());
        }
    }
}
