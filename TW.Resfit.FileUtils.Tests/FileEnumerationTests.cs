﻿// --------------------------------------------------------------------------------------------------------------------
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
            this.FileSystem.AllFiles(this.TestDirectoryPath, FileFilter.NoFilter).Count().ShouldBe(7, "There were not exactly 5 files enumerated");
        }

        [Test]
        public void ShouldIgnoreBlacklistedDirectories()
        {
            var filter = new FileFilter { DirectoryBlacklist = new Regex(@"Directory1") };

            var files = this.FileSystem.AllFiles(this.TestDirectoryPath, filter).Select(x => x.DirectoryName);
            files.ShouldNotContain(x => x.Contains(@"\Directory1"));
        }

        [Test]
        public void ShouldOnlyReturnWhitelistedFilenames()
        {
            var filter = new FileFilter { FileExtensionWhitelist = new Regex(@"\.(?:cs|resx)") };

            var fileNames = this.FileSystem.AllFiles(this.TestDirectoryPath, filter).Select(x => x.Name);
            fileNames.ShouldNotContain(x => x.EndsWith(".txt"));
        }

        [Test]
        public void PreventsEnumeratingNullPath()
        {
            Should.Throw<ArgumentNullException>(() => this.FileSystem.AllFiles(string.Empty, FileFilter.NoFilter).First());
        }
    }
}
