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
    using System.Xml.Linq;

    public class XmlResourceParser
    {
        public ResourceList Parse(XElement xmlDocument)
        {
            var resourceList = new ResourceList();

            foreach (var resourceElement in xmlDocument.Elements())
            {
                var key = resourceElement.Attribute("name").Value;
                var valueElement = resourceElement.Element("value");

                string value = string.Empty;

                if (valueElement != null)
                {
                    value = valueElement.Value;
                }

                resourceList.AddResource(new Resource(key, value));
            }

            return resourceList;
        }

        public ResourceList Parse(string xmlString)
        {
            var xmlDocument = XElement.Parse(xmlString);

            return this.Parse(xmlDocument);
        }
    }
}
