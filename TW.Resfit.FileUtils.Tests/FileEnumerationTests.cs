// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileEnumerationTests.cs" company="Tygertec">
//   Copyright © 2016 Tyrone Walls.
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
    using NUnit.Framework;
    using Shouldly;

    using TW.Resfit.FileUtils.HierarchyBuilder;
    using TW.Resfit.Framework.Testing;

    [TestFixture]
    public class FileEnumerationTests : UnitTests
    {
        [Test]
        public void ShouldEnumerateAllFiles()
        {
            var path = this.CreateSampleFileHierarchy();

            this.FileSystem.AllFiles(path).Count().ShouldBe(5, "There were not exactly 5 files enumerated");
        }

        [Test]
        public void ShouldIgnoreBlacklistedDirectories()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void ShouldOnlyReturnWhitelistedFilenames()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void PreventsEnumeratingNullPath()
        {
            Should.Throw<ArgumentNullException>(() => this.FileSystem.AllFiles(string.Empty).First());
        }

        private string CreateSampleFileHierarchy()
        {
            var path = this.GenerateRandomTempPath("FileEnumerationTests");
            SampleData.CreateSampleFileHierarchy(this.FileSystem, path);
            return path;
        }
    }
}
