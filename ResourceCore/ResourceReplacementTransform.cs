// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResourceReplacementTransform.cs" company="Tygertec">
//   Copyright © 2016 Tyrone Walls.
//   All rights reserved.
// </copyright>
// <summary>
//   Defines the ResourceReplacementTransform type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TW.Resfit.Core
{
    using System;
    using System.Runtime.Remoting.Messaging;
    using System.Text.RegularExpressions;
    using System.Xml.Linq;

    public class ResourceReplacementTransform : ITransform
    {
        private readonly Resource replacementResource;

        public ResourceReplacementTransform(Resource replacementResource)
        {
            this.replacementResource = replacementResource;
        }

        public Resource Replacement
        {
            get
            {
                return this.replacementResource;
            }
        }
        
        public void Transform(ref string sourceFile, Resource originalResource)
        {
            // Since it's a source file (*.cs), we only have to replace the key
            sourceFile = Regex.Replace(sourceFile, string.Format(@"\b{0}\b", originalResource.Key), this.replacementResource.Key);
        }

        public void Transform(ref XElement resourcesXml, Resource originalResource)
        {
            resourcesXml.Elements(originalResource.Key).Remove();
            resourcesXml.Add(this.replacementResource.ToXml());
        }

        public Resource Transform(Resource originalResource)
        {
            return this.replacementResource;
        }
    }
}