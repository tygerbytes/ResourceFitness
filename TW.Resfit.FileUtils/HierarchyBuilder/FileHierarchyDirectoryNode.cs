// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileHierarchyDirectoryNode.cs" company="Tygertec">
//   Copyright © 2016 Tyrone Walls.
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
        private readonly string fullFolderPath;

        private readonly List<IFileHierarchyBuilder> children;

        public FileHierarchyDirectoryNode(string fullFolderPath)
        {
            this.fullFolderPath = fullFolderPath;
            this.children = new List<IFileHierarchyBuilder>();
        }

        public void Execute(IFileSystem fileSystem)
        {
            fileSystem.CreateDirectory(this.fullFolderPath);

            foreach (var fileHierarchyNode in this.children)
            {
                fileHierarchyNode.Execute(fileSystem);
            }
        }

        public FileHierarchyDirectoryNode AddDirectory(string subfolder)
        {
            var pathToFolder = Path.Combine(this.fullFolderPath, subfolder);
            var builderNode = new FileHierarchyDirectoryNode(pathToFolder);
            this.children.Add(builderNode);
            return builderNode;
        }

        public FileHierarchyDirectoryNode AddFiles(FileNode[] files)
        {
            foreach (var fileNode in files)
            {
                var pathToFile = Path.Combine(this.fullFolderPath, fileNode.Name);
                this.children.Add(new FileHierarchyFileNode(pathToFile, fileNode.Contents));
            }

            return this;
        }
    }
}