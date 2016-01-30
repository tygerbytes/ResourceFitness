﻿// --------------------------------------------------------------------------------------------------------------------
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
    using System;
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

        [Test]
        public void ResourceShouldBeInitializedWithABlankListOfTransforms()
        {
            var resource = new Resource("My_resource");
            resource.Transforms.ShouldNotBeNull();
        }

        [Test]
        public void CanInitializeNewResourceFromExistingResoure()
        {
            var resource = new Resource("My_resource");

            var clonedResource = new Resource(resource);

            resource.ShouldBe(clonedResource);
        }

        [Test]
        public void ShouldConvertToXml()
        {
            var resource = new Resource("My_resource", "Banana's are quite good");

            var xmlString = resource.ToXml().ToString();

            const string ExpectedXml = 
@"<data name=""My_resource"" xml:space=""preserve"">
  <value>Banana's are quite good</value>
</data>";
            xmlString.ShouldBe(ExpectedXml);
        }

        [Test]
        public void ToStringShouldReturnResourceKey()
        {
            var resource = new Resource("My_resource", "Banana's are good");

            resource.ToString().ShouldBe("My_resource");
        }
    }
}
