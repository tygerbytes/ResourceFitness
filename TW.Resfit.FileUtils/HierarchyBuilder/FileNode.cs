// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileNode.cs" company="Tygertec">
//   Copyright © 2016 Tyrone Walls.
//   All rights reserved.
// </copyright>
// <summary>
//   Defines the FileNode type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TW.Resfit.FileUtils.HierarchyBuilder
{
    public class FileNode
    {
        public FileNode(string name, string contentsOfFile)
        {
            this.Name = name;
            this.Contents = contentsOfFile;
        }

        public string Name { get; private set; }

        public string Contents { get; private set; }
    }
}