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
    using TW.Resfit.Framework.Testing;

    [TestFixture]
    public class FileEnumerationTests : UnitTests
    {
        [Test]
        public void ShouldEnumerateAllFiles()
        {
            throw new NotImplementedException();
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

        //private void CreateFiles()
        //{
        //    var rootPath = "path";
        //    var root = new Folder(rootPath);
        //    root.Folder("Folder1")
        //        .Files(
        //    {
        //        new FileItem("file1.txt", "some content"),
        //        new FileItem("file2.resx", "some resources"),
        //        new Folder("FOlder2")   
        //            .Files()
        //    })


        //}
    }
}
