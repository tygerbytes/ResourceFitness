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
    using System;
    using System.Linq;
    using System.Xml.Linq;

    using NUnit.Framework;
    using Shouldly;
    using TW.Resfit.Core;
    using TW.Resfit.FileUtils.HierarchyBuilder;
    using TW.Resfit.Framework.Testing;

    [TestFixture]
    public class ResourceReplacementTransformTests : UnitTests
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

        [Test]
        public void ShouldReplaceKeyInText()
        {
            const string BeforeReplace = 
@"Line1: Some stuff with My_Old_Resource
Line2: Some more stuff with My_Old_Resource in it
My_Old_Resource snuck in at the beginning of Line3;";

            var oldResource = new Resource("My_Old_Resource", "This old resource, he played one.");
            var newResource = new Resource(
                "My_New_Resource",
                "This is a new resource, he played knick knack on my shoe.");
            
            oldResource.Transforms.Add(new ResourceReplacementTransform(newResource));

            var fileContent = BeforeReplace;
            oldResource.Transforms.First().Transform(ref fileContent, oldResource);

            fileContent.ShouldBe(BeforeReplace.Replace("My_Old_Resource", "My_New_Resource"));
        }

        [Test]
        public void ShouldReplaceXmlNodeInResourcesXml()
        {
            var xml = XElement.Parse(SampleData.SampleXmlResourceString);

            var oldResource = new Resource("Resfit_Tests_Banana_Resource_Two", "This is the second resource in the file");
            var newResource = new Resource(
                "Resfit_Tests_Orange_Resource_Two",
                "This is still the second resource in the file");

            oldResource.Transforms.Add(new ResourceReplacementTransform(newResource));

            oldResource.Transforms.First().Transform(ref xml, oldResource);

            // TODO: Finish test
            throw new NotImplementedException();
        }

        [Test]
        public void ShouldReplaceOriginalResourceWithNew()
        {
            var oldResource = new Resource("Resfit_Tests_Banana_Resource_Two", "This is the second resource in the file");
            var newResource = new Resource(
                "Resfit_Tests_Orange_Resource_Two",
                "This is still the second resource in the file");

            oldResource.Transforms.Add(new ResourceReplacementTransform(newResource));

            var replacedResource = oldResource.Transforms.First().Transform(oldResource);

            replacedResource.ShouldBe(newResource);
        }
    }
}
