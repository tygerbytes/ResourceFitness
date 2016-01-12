﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResourceFormat.cs" company="Tygertec">
//   Copyright © 2016 Tyrone Walls.
//   All rights reserved.
// </copyright>
// <summary>
//   Defines the ResourceFormat type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TW.Resfit.Core
{
    using System.IO;
    using System.Text.RegularExpressions;

    public class ResourceFormat
    {
        public ResourceFormat(char separator)
        {
            this.Separator = separator;
        }

        public ResourceFormat()
            : this('_')
        {
        }

        public static ResourceFormat Default
        {
            get
            {
                return new ResourceFormat();
            }
        }

        public char Separator { get; private set; }

        public bool IsValidKey(string key)
        {
            string pattern = string.Format("^[A-Za-z1-9{0}]+$", this.Separator);

            return Regex.IsMatch(key, pattern);
        }

        public void Validate(string key)
        {
            if (!this.IsValidKey(key))
            {
                throw new InvalidDataException();
            }
        }
    }
}
