// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResourceList.cs" company="Tygertec">
//   Copyright © 2016 Ty Walls.
//   All rights reserved.
// </copyright>
// <summary>
//   Defines the ResourceList type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TW.Resfit.Core
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Xml.Linq;

    using TW.Resfit.FileUtils;
    using TW.Resfit.Framework;

    public class ResourceList : ListDecoratorBase<Resource>
    {
        /// <summary>
        /// Gets the underlying Items collection.
        /// It is just a proxy for the protected Items property.
        /// Its only purpose is to make the code read easier.
        /// </summary>
        private IEnumerable<Resource> Resources
        {
            get
            {
                return this.Items;
            }
        }

        public ResourceList Clone(ResourceFilter filter = null)
        {
            if (filter == null)
            {
                filter = ResourceFilter.NoFilter;
            }

            var clonedList = new ResourceList();

            foreach (var resource in this.Where(resource => filter.IsMatch(resource)))
            {
                clonedList.Add(new Resource(resource));
            }

            return clonedList;
        }

        public void Merge(ResourceList resourceListToAbsorb)
        {
            foreach (var resource in resourceListToAbsorb)
            {
                if (this.All(x => x.Key != resource.Key))
                {
                    this.Add(new Resource(resource));
                }
            }
        }

        public void TransformFolder(string folderPath)
        {
            //// TODO: Break this behavior out into a separate "helper" class
            var fileSystem = FileSystem.Instance;

            var whiteList = new Regex(@"\.(?:resx|cs)$");
            foreach (var fileInfo in FileSystem.Instance.AllFiles(folderPath, null, whiteList))
            {
                var fileText = fileSystem.LoadFile(fileInfo.FullName);

                // Does this file require changes?
                var makeChanges =
                    this.Resources.Where(x => x.Transforms.Any())
                        .Any(
                            resource =>
                            resource.Transforms.Any(transform => transform.WillAffect(ref fileText, resource)));

                if (!makeChanges)
                {
                    continue;
                }

                XElement xml = null;
                if (fileInfo.Extension == ".resx")
                {
                    xml = fileSystem.LoadXmlFile(fileInfo.FullName);
                }

                foreach (var resource in this.Resources.Where(x => x.Transforms.Any()))
                {
                    if (fileInfo.Extension == ".resx")
                    {
                        resource.Transforms.ForEach(x => x.Transform(ref xml, resource));
                    }
                    else
                    {
                        resource.Transforms.ForEach(x => x.Transform(ref fileText, resource));
                    }
                }

                if (xml != null)
                {
                    fileText = xml.ToString();
                }

                fileSystem.WriteToFile(fileInfo.FullName, fileText);
            }
        }

        public ResourceList TransformSelfIntoNewList()
        {
            var newList = new ResourceList();

            foreach (var resource in this)
            {
                var newResource = new Resource(resource);

                foreach (var transform in resource.Transforms)
                {
                    newResource = transform.Transform(resource);
                }

                if (newResource != null)
                {
                    newList.Add(newResource);
                }
            }

            return newList;
        }
    }
}
