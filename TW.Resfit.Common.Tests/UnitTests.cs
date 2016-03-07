// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnitTests.cs" company="Tygertec">
//   Copyright © 2016 Ty Walls.
//   All rights reserved.
// </copyright>
// <summary>
//   Defines the UnitTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TW.Resfit.Framework.Testing
{
    using NUnit.Framework;
    using TW.Resfit.FileUtils;
    using TW.Resfit.FileUtils.HierarchyBuilder;

    public abstract class UnitTests
    {
        private SelfPurgingFileSystem fileSystem;

        protected IFileSystem FileSystem
        {
            get
            {
                return this.fileSystem ?? (this.fileSystem = new SelfPurgingFileSystem(SampleData.TestingPath()));
            }
        }

        [OneTimeTearDown]
        protected void TearDownFixture()
        {
            if (this.fileSystem != null)
            {
                this.fileSystem.Purge();
            }
        }
    }
}