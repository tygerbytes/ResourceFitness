// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFileHierarchyBuilder.cs" company="Tygertec">
//   Copyright © 2016 Tyrone Walls.
//   All rights reserved.
// </copyright>
// <summary>
//   Defines the IFileHierarchyBuilder type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TW.Resfit.FileUtils
{
    public interface IFileHierarchyBuilder
    {
        void Execute(IFileSystem fileSystem);
    }
}