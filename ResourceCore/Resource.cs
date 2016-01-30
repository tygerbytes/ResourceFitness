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
    using System;
    using System.Collections.Generic;
    using System.Xml.Linq;

    public class Resource : IEquatable<Resource>
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

        public Resource(Resource existing)
        {
            this.key = existing.key;
            this.value = existing.value;
            this.ResourceFormat = existing.ResourceFormat;

            this.Transforms = new List<ITransform>();
            foreach (var transform in existing.Transforms)
            {
                this.Transforms.Add(transform);
            }
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

        public static bool operator ==(Resource left, Resource right)
        {
            // ReSharper disable once RedundantNameQualifier
            return object.Equals(left, right);
        }

        public static bool operator !=(Resource left, Resource right)
        {
            // ReSharper disable once RedundantNameQualifier
            return !object.Equals(left, right);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return this.Equals((Resource)obj);
        }

        public bool Equals(Resource other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return string.Equals(this.key, other.key) 
                && string.Equals(this.value, other.value)
                && this.ResourceFormat.Equals(other.ResourceFormat);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = this.key.GetHashCode();
                hashCode = (hashCode * 397) ^ this.value.GetHashCode();
                hashCode = (hashCode * 397) ^ this.ResourceFormat.GetHashCode();
                return hashCode;
            }
        }

        public XElement ToXml()
        {
            var preserveSpace = new XAttribute(XNamespace.Xml + "space", "preserve");

            var element = new XElement(
                "data",
                new XAttribute("name", this.key),
                preserveSpace,
                new XElement("value", this.value));

            return element;
        }

        public override string ToString()
        {
            return this.key;
        }
    }
}
