// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResourceReplacementTransformTests.cs" company="Tygertec">
//   Copyright © 2016 Tyrone Walls.
//   All rights reserved.
// </copyright>
// <summary>
//   Defines the ResourceReplacementTransformTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Core.Tests
{
    using System.Linq;

    using NUnit.Framework;

    using Shouldly;

    using TW.Resfit.Core;

    [TestFixture]
    public class ResourceReplacementTransformTests
    {
        [Test]
        public void ShouldAddAndRetrieveFromResourceTransforms()
        {
            var resource = new Resource("My_resource", "My resource");
            var replacementResource = new Resource("My_new_resource", "My replacement resource");

            var resourceReplacementTransform = new ResourceReplacementTransform(replacementResource);
            resource.Transforms.Add(resourceReplacementTransform);

            ((ResourceReplacementTransform)resource.Transforms.First()).Replacement.ShouldBe(replacementResource);
        }
    }
}
