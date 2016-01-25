// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileHierarchyFileNode.cs" company="Tygertec">
//   Copyright © 2016 Tyrone Walls.
//   All rights reserved.
// </copyright>
// <summary>
//   Defines the FileHierarchyFileNode type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TW.Resfit.FileUtils.FileHierarchyBuilder
{
    public class FileHierarchyFileNode : IFileHierarchyBuilder
    {
        private readonly string fullPath;

        private readonly string contents;

        public FileHierarchyFileNode(string fullPath, string contents)
        {
            this.fullPath = fullPath;
            this.contents = contents;
        }

        public void Execute(IFileSystem fileSystem)
        {
            fileSystem.WriteToFile(this.fullPath, this.contents);
        }
    }
}