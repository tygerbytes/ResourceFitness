// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileSystem.cs" company="Tygertec">
//   Copyright © 2016 Tyrone Walls.
//   All rights reserved.
// </copyright>
// <summary>
//   Defines the FileSystem type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TW.Resfit.FileUtils
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Xml.Linq;

    public class FileSystem : IFileSystem
    {
        private static FileSystem instance;

        public static FileSystem Instance
        {
            get
            {
                return instance ?? (instance = new FileSystem());
            }
        }

        public void WriteToFile(string fileName, string content)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException(fileName, "A filename must be provided.");
            }

            File.WriteAllText(fileName, content);
        }

        public string LoadFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException(fileName, "A filename must be provided.");
            }

            return File.ReadAllText(fileName);
        }

        public XElement LoadXmlFile(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path");
            }

            return XElement.Load(path);
        }

        public IEnumerable<FileInfo> AllFiles(
            string rootPath,
            Regex directoryBlackList = null,
            Regex fileExtensionWhitelist = null)
        {
            if (string.IsNullOrEmpty(rootPath))
            {
                throw new ArgumentNullException("rootPath", "Must provide a root path.");
            }

            if (directoryBlackList == null)
            {
                directoryBlackList = new Regex(@"DON'T BLACKLIST ANYTHING");
            }

            if (fileExtensionWhitelist == null)
            {
                fileExtensionWhitelist = new Regex(@".*");
            }

            // Iterate through all files from provided path
            var root = new DirectoryInfo(rootPath);

            var directories = root.GetDirectories("*.*", SearchOption.AllDirectories);

            // Walk all of the directories
            foreach (var dir in directories)
            {
                if (directoryBlackList.IsMatch(dir.FullName))
                {
                    continue;
                }

                var files = dir.GetFiles("*.*");

                foreach (var file in files)
                {
                    if (!fileExtensionWhitelist.IsMatch(file.Name))
                    {
                        continue;
                    }

                    yield return file;
                }
            }
        }

        public void CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }
    }
}
