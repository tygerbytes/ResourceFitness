// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XmlResourceParserTests.cs" company="Tygertec">
//   Copyright © 2016 Ty Walls.
//   All rights reserved.
// </copyright>
// <summary>
//   Defines the XmlResourceParserTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TW.Resfit.Core.Tests
{
    using System.Linq;
    using System.Xml.Linq;
    using NUnit.Framework;
    using Shouldly;
    using TW.Resfit.Core;
    using TW.Resfit.FileUtils.HierarchyBuilder;
    using TW.Resfit.Framework.Testing;

    [TestFixture]
    public class XmlResourceParserTests : UnitTests
    {
        [Test]
        public void ShouldParseXmlAsAResourceList()
        {
            var xmlDoc = XElement.Parse(SampleData.SampleXmlResourceString);

            var resourceList = XmlResourceParser.ParseAsResourceList(xmlDoc);

            resourceList.Count.ShouldBe(5);
            resourceList.Last().Key.ShouldBe("Resfit_Tests_Kiwi_Resource_Two");
            resourceList.Last().Value.ShouldBe("This is the second Kiwi resource in the file");
        }

        [Test]
        public void ParsesAllResourcesInPath()
        {
            var path = SampleData.GenerateRandomTempPath("TransformFilesTests");
            SampleData.CreateSampleFileHierarchy(this.FileSystem, path);

            var resources = XmlResourceParser.ParseAllResourceFiles(this.FileSystem, path);

            resources.Count.ShouldBe(8);
            var appleTwoResource = resources.First(x => x.Key == "Resfit_Tests_Apple_Resource_Two");
            appleTwoResource.Value.ShouldBe("This is the second Apple resource in the file");
        }
    }
}
