// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileHierarchyDirectoryNode.cs" company="Tygertec">
//   Copyright © 2016 Ty Walls.
//   All rights reserved.
// </copyright>
// <summary>
//   Defines the FileHierarchyDirectoryNode type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TW.Resfit.FileUtils.HierarchyBuilder
{
    using System.Collections.Generic;
    using System.IO;

    public class FileHierarchyDirectoryNode : IFileHierarchyBuilder
    {
        private readonly string fullDirectoryPath;

        private readonly List<IFileHierarchyBuilder> children;

        public FileHierarchyDirectoryNode(string fullDirectoryPath)
        {
            this.fullDirectoryPath = fullDirectoryPath;
            this.children = new List<IFileHierarchyBuilder>();
        }

        public void Execute(IFileSystem fileSystem)
        {
            fileSystem.CreateDirectory(this.fullDirectoryPath);

            foreach (var fileHierarchyNode in this.children)
            {
                fileHierarchyNode.Execute(fileSystem);
            }
        }

        public FileHierarchyDirectoryNode AddDirectory(string subdirectory)
        {
            var pathToDirectory = Path.Combine(this.fullDirectoryPath, subdirectory);
            var builderNode = new FileHierarchyDirectoryNode(pathToDirectory);
            this.children.Add(builderNode);
            return builderNode;
        }

        public FileHierarchyDirectoryNode AddFiles(FileNode[] files)
        {
            foreach (var fileNode in files)
            {
                var pathToFile = Path.Combine(this.fullDirectoryPath, fileNode.Name);
                this.children.Add(new FileHierarchyFileNode(pathToFile, fileNode.Contents));
            }

            return this;
        }
    }
}
