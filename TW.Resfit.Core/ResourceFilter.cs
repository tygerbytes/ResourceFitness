// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResourceFilter.cs" company="Tygertec">
//   Copyright © 2016 Ty Walls.
//   All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace TW.Resfit.Core
{
    using System.Text.RegularExpressions;

    public class ResourceFilter
    {
        public static readonly ResourceFilter NoFilter = new ResourceFilter();

        public Regex KeyRegex { get; set; }

        public Regex ValueRegex { get; set; }

        public bool IsMatch(Resource resource)
        {
            return this.KeyIsMatch(resource.Key) && this.ValueIsMatch(resource.Value);
        }

        public bool KeyIsMatch(string subject)
        {
            return this.KeyRegex == null || this.KeyRegex.IsMatch(subject);
        }

        public bool ValueIsMatch(string subject)
        {
            return this.ValueRegex == null || this.ValueRegex.IsMatch(subject);
        }
    }
}
