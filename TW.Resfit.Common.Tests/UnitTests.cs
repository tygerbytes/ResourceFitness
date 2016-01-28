// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnitTests.cs" company="Tygertec">
//   Copyright © 2016 Tyrone Walls.
//   All rights reserved.
// </copyright>
// <summary>
//   Defines the UnitTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TW.Resfit.Framework.Testing
{
    using TW.Resfit.FileUtils;

    public abstract class UnitTests
    {
        private IFileSystem fileSystem;

        protected IFileSystem FileSystem
        {
            get
            {
                return this.fileSystem ?? (this.fileSystem = new FileSystem());
            }
        }
    }
}