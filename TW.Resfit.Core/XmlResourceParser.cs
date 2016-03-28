// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XmlResourceParser.cs" company="Tygertec">
//   Copyright © 2016 Ty Walls.
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
        public static ResourceList ParseAsResourceList(XElement xmlDocument, ResourceFilter filter = null)
        {
            if (filter == null)
            {
                filter = ResourceFilter.NoFilter;
            }

            var resourceList = new ResourceList();

            foreach (var resourceElement in xmlDocument.Elements())
            {
                if (resourceElement.Name.LocalName != "data")
                {
                    continue;
                }

                var key = resourceElement.Attribute("name").Value;

                if (!filter.KeyIsMatch(key))
                {
                    continue;
                }

                var valueElement = resourceElement.Element("value");

                var value = string.Empty;

                if (valueElement != null)
                {
                    value = valueElement.Value;
                }

                if (!filter.ValueIsMatch(value))
                {
                    continue;
                }

                resourceList.Add(new Resource(key, value));
            }

            return resourceList;
        }

        public static ResourceList ParseAsResourceList(string xmlString, ResourceFilter filter = null)
        {
            var xmlDocument = XElement.Parse(xmlString);

            return ParseAsResourceList(xmlDocument, filter);
        }

        public static ResourceList ParseAllResourceFiles(IFileSystem fileSystem, string path, ResourceFilter filter = null)
        {
            var resourceList = new ResourceList();

            var fileExtensionWhitelist = new Regex(@"\.resx$");

            foreach (var file in fileSystem.AllFiles(path, null, fileExtensionWhitelist))
            {
                var xml = fileSystem.LoadXmlFile(file.FullName);
                resourceList.Merge(ParseAsResourceList(xml, filter));
            }

            return resourceList;
        }
    }
}
