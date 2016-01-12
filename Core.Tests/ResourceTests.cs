// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResourceTests.cs" company="Tygertec">
//   Copyright © 2016 Tyrone Walls.
//   All rights reserved.
// </copyright>
// <summary>
//   Defines the ResourceTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Core.Tests
{
    using System.IO;
    using NUnit.Framework;
    using Shouldly;
    using TW.Resfit.Core;

    [TestFixture]
    public class ResourceTests
    {
        [Test]
        public void ShouldCreateResourceFromString()
        {
            var r = new Resource("My_resource");

            r.ShouldNotBeNull();
        }

        [Test]
        public void ResourceCreatedFromStringShouldBeFormattedCorrectly()
        {
            const string Key = "My resource key";

            Should.Throw<InvalidDataException>(() => new Resource(Key));
        }
    }
}
