// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResourceFormat.cs" company="Tygertec">
//   Copyright © 2016 Ty Walls.
//   All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace TW.Resfit.Core
{
    using System.IO;
    using System.Text.RegularExpressions;

    public class ResourceFormat
    {
        static ResourceFormat()
        {
            Default = new ResourceFormat('_', "NoFilter");
        }

        public ResourceFormat(char separator, string name = null)
        {
            this.Separator = separator;
            this.Name = name ?? this.GetType().Name;
        }

        public ResourceFormat()
            : this('_')
        {
        }

        public static ResourceFormat Default { get; set; }

        public string Name { get; private set; }

        public char Separator { get; private set; }

        public bool IsValidKey(string key)
        {
            var pattern = string.Format("^[A-Za-z0-9{0},/\\-()}}{{]+$", this.Separator);

            return Regex.IsMatch(key, pattern);
        }

        public override string ToString()
        {
            return this.Name;
        }

        public void Validate(string key)
        {
            if (!this.IsValidKey(key))
            {
                throw new InvalidDataException(string.Format("Key \"{0}\" fails validation for resource format ({1})", key, this.Name));
            }
        }
    }
}
