// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileHierarchyBuilder.cs" company="Tygertec">
//   Copyright © 2016 Tyrone Walls.
//   All rights reserved.
// </copyright>
// <summary>
//   Defines the FileHierarchyBuilder type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TW.Resfit.FileUtils.HierarchyBuilder
{
    public class FileHierarchyBuilder : FileHierarchyDirectoryNode
    {
        public FileHierarchyBuilder(string fullFolderPath)
            : base(fullFolderPath)
        {
        }
    }
}
