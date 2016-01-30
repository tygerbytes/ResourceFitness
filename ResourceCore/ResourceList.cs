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
    using System;
    using System.Collections.Generic;

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
            // TODO: Break this behavior out into a separate helper class

            throw new NotImplementedException();
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
                this.Items.Add(new Resource(resource));
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
