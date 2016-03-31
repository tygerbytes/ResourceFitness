// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResourceReplacementTransformTests.cs" company="Tygertec">
//   Copyright © 2016 Ty Walls.
//   All rights reserved.
// </copyright>
// <summary>
//   Defines the ResourceReplacementTransformTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TW.Resfit.Core.Tests
{
    using System.Linq;
    using System.Xml.Linq;

    using NUnit.Framework;
    using Shouldly;
    using TW.Resfit.Core;
    using TW.Resfit.Core.Transforms;
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

            // -- Verify transformation
            var matchingAppleElements =
                xml.Elements("data")
                    .Where(element => (string)element.Attribute("name") == "Resfit_Tests_Banana_Resource_Two");

            matchingAppleElements.Count().ShouldBe(0, "The XML should no longer contain the key, but still does.");

            var matchingOrangeElements =
                xml.Elements("data")
                    .Where(element => (string)element.Attribute("name") == "Resfit_Tests_Orange_Resource_Two")
                    .ToArray(/* StyleCop */);

            matchingOrangeElements.Count().ShouldBe(1, "There should only be one matching key after the transformation");
            matchingOrangeElements.First().Value.ShouldBe("This is still the second resource in the file");
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

        [Test]
        public void ShouldCheckThatTransformWillChangeAGivenFile()
        {
            var resourceFileText = SampleData.SampleFruitySourceFile("Peach");

            var oldResource = new Resource("Resfit_Tests_Peach_Resource_Two");
            var newResource = new Resource("Resfit_Tests_Pineapple_Resource_Two");
            oldResource.Transforms.Add(new ResourceReplacementTransform(newResource));

            oldResource.Transforms.First().WillAffect(resourceFileText, oldResource).ShouldBeTrue();
        }
    }
}
