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

    public class ResourceList
    {
        private readonly List<Resource> resources = new List<Resource>();

        public int Count
        {
            get
            {
                return this.resources.Count;
            }
        }

        public Resource First
        {
            get
            {
                return this.resources.First();
            }
        }

        public Resource Last
        {
            get
            {
                return this.resources.Last();
            }
        }

        public void AddResource(Resource resource)
        {
            this.resources.Add(resource);
        }
    }
}
