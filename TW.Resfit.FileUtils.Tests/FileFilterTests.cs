// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileFilterTests.cs" company="Tygertec">
//   Copyright © 2016 Ty Walls.
//   All rights reserved.
// </copyright>
// <summary>
//
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TW.Resfit.FileUtils.Tests
{
    using System.Text.RegularExpressions;
    using NUnit.Framework;

    using Shouldly;

    [TestFixture]
    public class FileFilterTests
    {
        [Test]
        public void ShouldMatchFileExtension()
        {
            var filter = new FileFilter { FileExtensionWhitelist = new Regex(@"\.txt$") };

            filter.IsMatch(@"C:\Dir\File.txt").ShouldBeTrue();
            filter.IsMatch(@"C:\Dir\File.cs").ShouldBeFalse();
        }

        [Test]
        public void ShouldNotMatchBlacklistedDirectory()
        {
            var filter = new FileFilter { DirectoryBlacklist = new Regex(@"BadDir") };

            filter.IsMatch(@"C:\This\Is\A\BadDir").ShouldBeFalse();
            filter.IsMatch(@"C:\This\Dir\Is\Ok").ShouldBeTrue();
        }

        [Test]
        public void ShouldMatchCombinationOfExtensionWhitelistAndDirectoryBlacklist()
        {
            var filter = new FileFilter
                             {
                                 DirectoryBlacklist = new Regex(@"BadDir"),
                                 FileExtensionWhitelist = new Regex(@"\.txt$")
                             };

            filter.IsMatch(@"C:\BadDir\file.txt").ShouldBeFalse("Should not match because of directory blacklist");
            filter.IsMatch(@"C:\CoolDir\file.txt").ShouldBeTrue();
        }
    }
}
