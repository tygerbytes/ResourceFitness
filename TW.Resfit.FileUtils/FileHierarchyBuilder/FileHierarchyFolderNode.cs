// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileHierarchyFolderNode.cs" company="Tygertec">
//   Copyright © 2016 Tyrone Walls.
//   All rights reserved.
// </copyright>
// <summary>
//   Defines the FileHierarchyFolderNode type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TW.Resfit.FileUtils.FileHierarchyBuilder
{
    using System.Collections.Generic;
    using System.IO;

    public class FileHierarchyFolderNode : IFileHierarchyBuilder
    {
        private readonly string fullFolderPath;

        private readonly List<IFileHierarchyBuilder> children;

        public FileHierarchyFolderNode(string fullFolderPath)
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

        public FileHierarchyFolderNode AddFolder(string subfolder)
        {
            var pathToFolder = Path.Combine(this.fullFolderPath, subfolder);
            var builderNode = new FileHierarchyFolderNode(pathToFolder);
            this.children.Add(builderNode);
            return builderNode;
        }

        public FileHierarchyFolderNode AddFiles(FileNode[] files)
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