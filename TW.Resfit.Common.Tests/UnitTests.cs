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
    using System;
    using System.Globalization;
    using System.IO;

    using TW.Resfit.FileUtils;

    public abstract class UnitTests
    {
        protected UnitTests()
        {
            this.FileSystem = new FileSystem();
        }

        protected IFileSystem FileSystem { get; set; }

        protected string GenerateRandomTempPath(string baseName)
        {
            return Path.Combine(Path.GetTempPath(), "TW.Resfit", baseName, DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture));
        }
    }
}