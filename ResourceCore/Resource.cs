// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Resource.cs" company="Tygertec">
//   Copyright © 2016 Tyrone Walls.
//   All rights reserved.
// </copyright>
// <summary>
//   Defines the Resource type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TW.Resfit.Core
{
    using System.Collections.Generic;

    public class Resource
    {
        private readonly string key;

        private readonly string value;

        public Resource(string resourceKey, string value, ResourceFormat format)
        {
            format.Validate(resourceKey);

            this.ResourceFormat = format;
            this.value = value;

            this.key = resourceKey;

            this.Transforms = new List<ITransform>();
        }

        public Resource(string resourceKey, string value) : this(resourceKey, value, ResourceFormat.Default)
        {
        }

        public Resource(string resourceKey) : this(resourceKey, string.Empty, ResourceFormat.Default)
        {
        }

        public ResourceFormat ResourceFormat { get; private set; }

        public string Key
        {
            get
            {
                return this.key;
            }
        }

        public string Value
        {
            get
            {
                return this.value;
            }
        }

        public List<ITransform> Transforms { get; private set; }
    }
}
