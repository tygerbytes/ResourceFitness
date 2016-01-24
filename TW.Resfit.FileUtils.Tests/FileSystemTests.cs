// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileSystemTests.cs" company="Tygertec">
//   Copyright © 2016 Tyrone Walls.
//   All rights reserved.
// </copyright>
// <summary>
//   Defines the FileSystemTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TW.Resfit.FileUtils.Tests
{
    using System;
    using System.IO;
    using NUnit.Framework;
    using Shouldly;
    using TW.Resfit.Framework.Testing;

    [TestFixture]
    internal class FileSystemTests : UnitTests
    {
        [Test]
        public void ShouldSaveAndLoadFile()
        {
            var randomFileName = string.Format("TW.Resfit.TestSave_{0}.txt", Guid.NewGuid().ToString().Substring(0, 10));
            var path = Path.Combine(Path.GetTempPath(), randomFileName);

            const string Content = "Line1\r\nLine2\r\nLine3";

            FileSystem.WriteToFile(path, Content);

            FileSystem.LoadFile(path).ShouldBe(Content);
        }

        [Test]
        public void CannotSaveToFileWithoutFilename()
        {
            Should.Throw<ArgumentNullException>(() => this.FileSystem.WriteToFile(string.Empty, "Stuff."));
        }

        [Test]
        public void CannotLoadFileWithoutFilename()
        {
            Should.Throw<ArgumentNullException>(() => this.FileSystem.LoadFile(string.Empty));
        }
    }
}
