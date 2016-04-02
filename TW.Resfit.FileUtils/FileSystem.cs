// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileSystem.cs" company="Tygertec">
//   Copyright © 2016 Ty Walls.
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
    using System.Linq;
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

        public virtual void WriteToFile(string fileName, string content)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException(fileName, "A filename must be provided.");
            }

            var directoryName = Path.GetDirectoryName(fileName);

            this.CreateDirectory(directoryName);

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
            FileFilter filter)
        {
            if (string.IsNullOrEmpty(rootPath))
            {
                throw new ArgumentNullException("rootPath", "Must provide a root path.");
            }

            // Iterate through all files from provided path
            var root = new DirectoryInfo(rootPath);

            var subDirectories = root.GetDirectories("*.*", SearchOption.AllDirectories);

            // Make sure we include the root when enumerating the files of each directory
            var directories = new List<DirectoryInfo>(capacity: subDirectories.Count() + 1) { root };
            directories.AddRange(subDirectories);

            // Walk all of the directories
            foreach (var dir in directories)
            {
                if (filter.DirectoryBlacklist.IsMatch(dir.FullName))
                {
                    continue;
                }

                var files = dir.GetFiles("*.*");

                foreach (var file in files)
                {
                    if (!filter.IsMatch(file.Name))
                    {
                        continue;
                    }

                    yield return file;
                }
            }
        }

        public virtual void CreateDirectory(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(path, "The directory name must not be null or empty");
            }

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}
