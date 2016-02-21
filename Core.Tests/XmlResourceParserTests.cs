// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XmlResourceParserTests.cs" company="Tygertec">
//   Copyright © 2016 Tyrone Walls.
//   All rights reserved.
// </copyright>
// <summary>
//   Defines the XmlResourceParserTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TW.Resfit.Core.Tests
{
    using System;
    using System.Linq;
    using System.Xml.Linq;
    using NUnit.Framework;
    using Shouldly;
    using TW.Resfit.Core;
    using TW.Resfit.FileUtils.HierarchyBuilder;

    [TestFixture]
    public class XmlResourceParserTests
    {
        [Test]
        public void ShouldParseXmlAsAResourceList()
        {
            var xmlDoc = XElement.Parse(SampleData.SampleXmlResourceString);

            var resourceList = XmlResourceParser.ParseAsResourceList(xmlDoc);

            resourceList.Items.Count.ShouldBe(3);
            resourceList.Items.Last().Key.ShouldBe("Resfit_Tests_Banana_Resource_Three");
            resourceList.Items.Last().Value.ShouldBe("This is the third resource in the file");
        }

        [Test]
        public void ParsesAllResourcesInPath()
        {
            // TODO: Consider moving this functionality to FileUtils
            throw new NotImplementedException();
        }
    }
}
