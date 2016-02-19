// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResourceFormatTests.cs" company="Tygertec">
//   Copyright © 2016 Tyrone Walls.
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
    }
}
