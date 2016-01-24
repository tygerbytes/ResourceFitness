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
        public void PreventsEnumeratingNullPath()
        {
            Should.Throw<ArgumentNullException>(() => this.FileSystem.AllFiles(string.Empty).First());
        }
    }
}
