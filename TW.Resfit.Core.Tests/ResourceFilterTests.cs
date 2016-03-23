// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResourceFilterTests.cs" company="Tygertec">
//   Copyright © 2016 Ty Walls.
//   All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace TW.Resfit.Core.Tests
{
    using System.Text.RegularExpressions;

    using NUnit.Framework;
    using Shouldly;
    using TW.Resfit.Core;
    using TW.Resfit.Framework.Testing;

    [TestFixture]
    public class ResourceFilterTests : UnitTests
    {
        [Test]
        public void ShouldMatchResourceOnKey()
        {
            var filter = new ResourceFilter { KeyRegex = new Regex(@"Kiwi") };
            var resource = new Resource("This_is_my_Kiwi_resource");

            filter.IsMatch(resource).ShouldBeTrue();
        }

        [Test]
        public void ShouldMatchResourceOnValue()
        {
            var filter = new ResourceFilter { ValueRegex = new Regex(@"amazing kiwi") };
            var resource = new Resource("This_is_my_Kiwi_resource", "What an amazing kiwi");

            filter.IsMatch(resource).ShouldBeTrue();
        }

        [Test]
        public void ShouldMatchResourceOnKeyAndValue()
        {
            var filter = new ResourceFilter { KeyRegex = new Regex(@"Kiwi"), ValueRegex = new Regex(@"amazing kiwi") };
            var resource = new Resource("This_is_my_Kiwi_resource", "What an amazing kiwi");

            filter.IsMatch(resource).ShouldBeTrue();
        }

        [Test]
        public void ShouldMatchKeyString()
        {
            var filter = new ResourceFilter { KeyRegex = new Regex(@"Kiwi") };
            filter.KeyIsMatch("This_is_my_Kiwi_resource").ShouldBeTrue();
        }

        [Test]
        public void ShouldMatchValueString()
        {
            var filter = new ResourceFilter { ValueRegex = new Regex(@"amazing kiwi") };
            filter.ValueIsMatch("What an amazing kiwi").ShouldBeTrue();
        }
    }
}
