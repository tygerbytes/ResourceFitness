// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResfitSteps.cs" company="Tygertec">
//   Copyright © 2016 Tyrone Walls.
//   All rights reserved.
// </copyright>
// <summary>
//   Defines the ResfitSteps type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TW.Core.Requirements
{
    using StronglyTypedContext;

    using TechTalk.SpecFlow;

    public abstract class ResfitSteps : BaseBinding
    {
        [BeforeScenario]
        protected void ScenarioSetup()
        {
        }

        [AfterScenario]
        protected void ScenarioTeardown()
        {
        }

        [AfterStep]
        protected void AfterStep()
        {
        }
    }
}
