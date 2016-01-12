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
    public class Resource
    {
        private string key;

        private string value;

        public Resource(string resourceKey, ResourceFormat format)
        {
            format.Validate(resourceKey);

            this.ResourceFormat = format;

            this.key = resourceKey;
        }

        public Resource(string resourceKey) : this(resourceKey, ResourceFormat.Default)
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
    }
}
