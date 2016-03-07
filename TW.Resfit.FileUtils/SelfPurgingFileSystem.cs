// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SelfPurgingFileSystem.cs" company="Tygertec">
//   Copyright © 2016 Ty Walls.
//   All rights reserved.
// </copyright>
// <summary>
//   Defines the SelfPurgingFileSystem type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TW.Resfit.FileUtils
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class SelfPurgingFileSystem : FileSystem
    {
        private readonly List<string> filesToDelete = new List<string>();

        private readonly List<string> directoriesToDelete = new List<string>();

        private readonly string testingRoot;

        public SelfPurgingFileSystem(string rootDirectory)
        {
            this.testingRoot = rootDirectory;
        }

        public override void WriteToFile(string fileName, string content)
        {
            base.WriteToFile(fileName, content);

            this.filesToDelete.Add(fileName);
        }

        public override void CreateDirectory(string path)
        {
            base.CreateDirectory(path);

            var directoryToDelete = path;

            while (directoryToDelete != null 
                && directoryToDelete.Length > this.testingRoot.Length)
            {
                this.directoriesToDelete.Add(directoryToDelete);
                directoryToDelete = Path.GetDirectoryName(directoryToDelete);
            }
        }

        public void Purge()
        {
            this.PurgeFiles();

            this.PurgeDirectories();
        }

        private void PurgeDirectories()
        {
            var directories =
                this.directoriesToDelete
                    .Distinct()
                    .Where(x => x.StartsWith(this.testingRoot))
                    .Where(x => x.Length > this.testingRoot.Length)
                    .OrderByDescending(x => x)
                    .ThenByDescending(x => x.Length);

            foreach (var directory in directories)
            {
                if (!Directory.Exists(directory))
                {
                    continue;
                }

                try
                {
                    Directory.Delete(directory, true);
                }
                catch (DirectoryNotFoundException)
                {
                    // Swallow
                }
            }
        }

        private void PurgeFiles()
        {
            if (!this.filesToDelete.Any())
            {
                return;
            }

            var files = this.filesToDelete.Distinct().Where(x => x.StartsWith(this.testingRoot));

            foreach (var file in files)
            {
                if (!File.Exists(file))
                {
                    continue;
                }

                try
                {
                    File.Delete(file);
                }
                catch (DirectoryNotFoundException)
                {
                    // Swallow.
                }
            }
        }
    }
}