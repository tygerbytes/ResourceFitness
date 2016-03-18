// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="Tygertec">
//   Copyright © 2016 Ty Walls.
//   All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace TW.Resfit.Framework
{
    using System.Text.RegularExpressions;

    public static class StringExtensions
    {
        public static string StripNewLines(this string str)
        {
            return Regex.Replace(str, @"\r\n?|\n", string.Empty);
        }
    }
}
