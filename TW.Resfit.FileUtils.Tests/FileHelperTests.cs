// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileHelperTests.cs" company="Tygertec">
//   Copyright © 2016 Tyrone Walls.
//   All rights reserved.
// </copyright>
// <summary>
//   Defines the FileHelperTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TW.Resfit.FileUtils.Tests
{
    using System;
    using System.IO;

    using NUnit.Framework;

    using Shouldly;

    [TestFixture]
    public class FileHelperTests
    {
        [Test]
        public void CannotSaveToFileWithoutFilename()
        {
            Should.Throw<ArgumentNullException>(() => FileHelper.WriteToFile(string.Empty, "Stuff."));
        }

        [Test]
        public void ShouldSaveAndLoadFile()
        {
            var randomFileName = string.Format("TW.Resfit.TestSave_{0}.txt", Guid.NewGuid().ToString().Substring(0, 10));
            var path = Path.Combine(Path.GetTempPath(), randomFileName);

            const string Content = "Line1\r\nLine2\r\nLine3";

            FileHelper.WriteToFile(path, Content);

            FileHelper.LoadFile(path).ShouldBe(Content);
        }
    }
}
