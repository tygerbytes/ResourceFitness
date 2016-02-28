// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NewResourceList.cs" company="Tygertec">
//   Copyright © 2016 Tyrone Walls.
//   All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace TW.Resfit.Console
{
    using System.Management.Automation;
    using TW.Resfit.Core;

    [Cmdlet(VerbsCommon.New, "ResourceList")]
    public class NewResourceList : PSCmdlet
    {
        [Parameter(Position = 1, ValueFromPipeline = true)]
        public Resource[] Resources { get; set; }

        protected override void ProcessRecord()
        {
            var resourceList = new ResourceList();

            if (this.Resources == null)
            {
                this.WriteObject(resourceList);
                return;
            }

            foreach (var resource in this.Resources)
            {
                resourceList.Add(resource);
                this.WriteVerbose(string.Format("Added {0} to resource list", resource));
            }

            this.WriteObject(resourceList);
        }
    }
}
