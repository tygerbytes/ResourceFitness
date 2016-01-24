// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XmlResourceParserTests.cs" company="Tygertec">
//   Copyright © 2016 Tyrone Walls.
//   All rights reserved.
// </copyright>
// <summary>
//   Defines the XmlResourceParserTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Core.Tests
{
    using System.Linq;
    using System.Xml.Linq;
    using NUnit.Framework;
    using Shouldly;
    using TW.Resfit.Core;

    [TestFixture]
    public class XmlResourceParserTests
    {
        private const string XmlSample = @"<?xml version=""1.0"" encoding=""utf-8""?>
<root>
	<data name=""Resfit_Tests_LoadFromFile_Resource_One"" xml:space=""preserve"">
    <value>This is the first resource in the file.</value>
  </data>
  <data name=""Resfit_Tests_LoadFromFile_Resource_Two"" xml:space=""preserve"">
    <value>This is the second resource in the file</value>
  </data>
  <data name=""Resfit_Tests_LoadFromFile_Resource_Three"" xml:space=""preserve"">
    <value>This is the third resource in the file</value>
  </data>
</root>";

        [Test]
        public void ShouldParseXmlAsAResourceList()
        {
            var xmlDoc = XElement.Parse(XmlSample);

            var resourceList = XmlResourceParser.ParseAsResourceList(xmlDoc);

            resourceList.Items.Count.ShouldBe(3);
            resourceList.Items.Last().Key.ShouldBe("Resfit_Tests_LoadFromFile_Resource_Three");
            resourceList.Items.Last().Value.ShouldBe("This is the third resource in the file");
        }
    }
}
