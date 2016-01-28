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
            throw new System.NotImplementedException();
        }

        public ResourceList TransformSelfIntoNewList()
        {
            throw new System.NotImplementedException();
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
