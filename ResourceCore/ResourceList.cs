// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResourceList.cs" company="Tygertec">
//   Copyright © 2016 Tyrone Walls.
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

    public class ResourceList
    {
        private readonly List<Resource> resources = new List<Resource>();

        public ICollection<Resource> Items
        {
            get
            {
                return this.resources;
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
                    this.resources.Where(x => x.Transforms.Any())
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

                foreach (var resource in this.resources.Where(x => x.Transforms.Any()))
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

            foreach (var resource in this.Items)
            {
                var newResource = new Resource(resource);

                foreach (var transform in resource.Transforms)
                {
                    newResource = transform.Transform(resource);
                }

                if (newResource != null)
                {
                    newList.Items.Add(newResource);
                }
            }

            return newList;
        }

        public void Merge(ResourceList resourceListToAbsorb)
        {
            foreach (var resource in resourceListToAbsorb.Items)
            {
                if (this.Items.All(x => x.Key != resource.Key))
                {
                    this.Items.Add(new Resource(resource));
                }
            }
        }

        public ResourceList Clone()
        {
            var clonedList = new ResourceList();

            foreach (var resource in this.Items)
            {
                clonedList.Items.Add(new Resource(resource));
            }

            return clonedList;
        }
    }
}
