// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringExtensionsTests.cs" company="Tygertec">
//   Copyright © 2016 Ty Walls.
//   All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace TW.Resfit.Framework.Tests
{
    using NUnit.Framework;

    using Shouldly;

    [TestFixture]
    public class StringExtensionsTests
    {
        [Test]
        public void ShouldStripCrLfFromString()
        {
            const string TestString = "Line1\r\nLine2\r\n";

            TestString.StripNewLines().ShouldBe("Line1Line2");
        }

        [Test]
        public void ShouldStripLfFromString()
        {
            const string TestString = "Line1\nLine2\n";

            TestString.StripNewLines().ShouldBe("Line1Line2");
        }

        [Test]
        public void ShouldStripMixedCrLfFromString()
        {
            const string TestString = "Line1\r\nLine2\nLine3\r\n";

            TestString.StripNewLines().ShouldBe("Line1Line2Line3");
        }
    }
}
