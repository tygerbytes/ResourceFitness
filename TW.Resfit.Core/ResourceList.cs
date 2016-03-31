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

    using TW.Resfit.Framework;

    public class ResourceList : ListDecoratorBase<Resource>
    {
        public ResourceList()
        {
        }

        public ResourceList(IEnumerable<Resource> resources) : this()
        {
            this.Items.AddRange(resources);
        }

        /// <summary>
        /// Gets the underlying Items collection.
        /// It is just a proxy for the protected Items property.
        /// Its only purpose is to make the code read easier.
        /// </summary>
        public ICollection<Resource> Resources
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
