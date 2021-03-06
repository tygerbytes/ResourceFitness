﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResourceFormatTests.cs" company="Tygertec">
//   Copyright © 2016 Ty Walls.
//   All rights reserved.
// </copyright>
// <summary>
//   Defines the ResourceFormatTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TW.Resfit.Core.Tests
{
    using NUnit.Framework;
    using Shouldly;
    using TW.Resfit.Core;

    [TestFixture]
    public class ResourceFormatTests
    {
        [Test]
        public void ShouldValidateCorrectlyFormattedKey()
        {
            const string Key = "My_resource_key";

            ResourceFormat.Default.IsValidKey(Key).ShouldBeTrue();
        }

        [Test]
        public void ShouldCatchIncorrectlyFormattedKey()
        {
            const string BadKey = "My%bad%key";

            ResourceFormat.Default.IsValidKey(BadKey).ShouldBeFalse();
        }

        [Test]
        public void CanOverrideFormatName()
        {
            new ResourceFormat('_').Name.ShouldBe("ResourceFormat");
            new ResourceFormat('_', "Custom Name").Name.ShouldBe("Custom Name");
        }

        [Test]
        public void DefaultConstructorShouldCreateTypicalResourceFormat()
        {
            new ResourceFormat().Separator.ShouldBe('_');
        }

        [Test]
        public void ToStringShouldReturnFormatName()
        {
            new ResourceFormat('_', "Amazing format").ToString().ShouldBe("Amazing format");
        }
    }
}
