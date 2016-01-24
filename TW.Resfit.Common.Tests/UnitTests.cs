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
        protected UnitTests()
        {
            this.FileSystem = new FileSystem();
        }

        protected IFileSystem FileSystem { get; set; }
    }
}