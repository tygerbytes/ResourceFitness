// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResfitSteps.cs" company="Tygertec">
//   Copyright © 2016 Tyrone Walls.
//   All rights reserved.
// </copyright>
// <summary>
//   Defines the ResfitSteps type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TW.Resfit.Framework.Requirements
{
    using StronglyTypedContext;

    using TechTalk.SpecFlow;

    using TW.Resfit.FileUtils;
    using TW.Resfit.FileUtils.HierarchyBuilder;

    public abstract class ResfitSteps : BaseBinding
    {
        protected ResfitSteps()
        {
            this.FileSystem = new SelfPurgingFileSystem(SampleData.TestingPath());
        }

        protected SelfPurgingFileSystem FileSystem { get; set; }

        [BeforeScenario]
        protected void ScenarioSetup()
        {
        }

        [AfterScenario]
        protected void ScenarioTeardown()
        {
            this.FileSystem.Purge();
        }

        [AfterStep]
        protected void AfterStep()
        {
        }
    }
}
