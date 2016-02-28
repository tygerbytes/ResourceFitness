// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XmlResourceParser.cs" company="Tygertec">
//   Copyright © 2016 Tyrone Walls.
//   All rights reserved.
// </copyright>
// <summary>
//   Defines the XmlResourceParser type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TW.Resfit.Core
{
    using System.Text.RegularExpressions;
    using System.Xml.Linq;

    using TW.Resfit.FileUtils;

    public class XmlResourceParser
    {
        public static ResourceList ParseAsResourceList(XElement xmlDocument)
        {
            var resourceList = new ResourceList();

            foreach (var resourceElement in xmlDocument.Elements())
            {
                if (resourceElement.Name.LocalName != "data")
                {
                    continue;
                }

                var key = resourceElement.Attribute("name").Value;
                var valueElement = resourceElement.Element("value");

                var value = string.Empty;

                if (valueElement != null)
                {
                    value = valueElement.Value;
                }

                resourceList.Add(new Resource(key, value));
            }

            return resourceList;
        }

        public static ResourceList ParseAsResourceList(string xmlString)
        {
            var xmlDocument = XElement.Parse(xmlString);

            return ParseAsResourceList(xmlDocument);
        }

        public static ResourceList ParseAllResourceFiles(IFileSystem fileSystem, string folderPath)
        {
            var resourceList = new ResourceList();

            var fileExtensionWhitelist = new Regex(@"\.resx$");

            foreach (var file in fileSystem.AllFiles(folderPath, null, fileExtensionWhitelist))
            {
                var xml = fileSystem.LoadXmlFile(file.FullName);
                resourceList.Merge(ParseAsResourceList(xml));
            }

            return resourceList;
        }
    }
}
