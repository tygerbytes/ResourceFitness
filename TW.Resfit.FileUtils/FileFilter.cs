// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileFilter.cs" company="Tygertec">
//   Copyright © 2016 Ty Walls.
//   All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace TW.Resfit.FileUtils
{
    using System.Text.RegularExpressions;

    public class FileFilter
    {
        public static readonly FileFilter NoFilter = new FileFilter();

        /// <summary>
        /// Typical file filter for use with resources.
        /// Matches (*.cs;*.resx;*.xaml.).
        /// </summary>
        public static readonly FileFilter Typical = new FileFilter
                                                        {
                                                            FileExtensionWhitelist =
                                                                new Regex(@"\.(?:cs|resx|xaml)$")
                                                        };

        public FileFilter()
        {
            this.DirectoryBlacklist = new Regex(@"DON'T BLACKLIST ANYTHING");
            this.FileExtensionWhitelist = new Regex(@".*");
        }

        public Regex DirectoryBlacklist { get; set; }

        public Regex FileExtensionWhitelist { get; set; }

        public bool IsMatch(string path)
        {
            return !this.PathOnDirectoryBlacklist(path) && this.PathOnFileExtensionWhitelist(path);
        }

        private bool PathOnDirectoryBlacklist(string path)
        {
            return this.DirectoryBlacklist.IsMatch(path);
        }

        private bool PathOnFileExtensionWhitelist(string path)
        {
            return this.FileExtensionWhitelist.IsMatch(path);
        }
    }
}
