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
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Xml.Linq;

    using TW.Resfit.FileUtils;

    public class ResourceList : IList<Resource>
    {
        private readonly List<Resource> resources = new List<Resource>();

        public int Count
        {
            get
            {
                return this.resources.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public Resource this[int index]
        {
            get
            {
                return this.resources[index];
            }
            set
            {
                this.resources[index] = value;
            }
        }

        public void Add(Resource item)
        {
            this.resources.Add(item);
        }

        public void Clear()
        {
            this.resources.Clear();
        }

        public ResourceList Clone()
        {
            var clonedList = new ResourceList();

            foreach (var resource in this)
            {
                clonedList.Add(new Resource(resource));
            }

            return clonedList;
        }

        public bool Contains(Resource item)
        {
            return this.resources.Contains(item);
        }

        public void CopyTo(Resource[] array, int arrayIndex)
        {
            this.resources.CopyTo(array, arrayIndex);
        }

        public IEnumerator<Resource> GetEnumerator()
        {
            return this.resources.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.resources.GetEnumerator();
        }

        public int IndexOf(Resource item)
        {
            return this.resources.IndexOf(item);
        }

        public void Insert(int index, Resource item)
        {
            this.resources.Insert(index, item);
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

        public bool Remove(Resource item)
        {
            return this.resources.Remove(item);
        }

        public void RemoveAt(int index)
        {
            this.resources.RemoveAt(index);
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
