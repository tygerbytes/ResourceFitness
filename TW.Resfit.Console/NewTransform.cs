// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NewTransform.cs" company="Tygertec">
//   Copyright © 2016 Ty Walls.
//   All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace TW.Resfit.Console
{
    using System;
    using System.Management.Automation;
    using TW.Resfit.Core;

    [Cmdlet(VerbsCommon.New, "Transform")]
    public class NewTransform : PSCmdlet
    {
        [Parameter(ParameterSetName = ParameterSet.ResourceReplacement)]
        [Alias("Replace")]
        public SwitchParameter ResourceReplacement { get; set; }

        [Parameter(
            ParameterSetName = ParameterSet.ResourceReplacement,
            Position = 1,
            Mandatory = true,
            ValueFromPipeline = true)]
        [Alias("Replacement")]
        public Resource ReplacementResource { get; set; }

        protected override void ProcessRecord()
        {
            switch (this.ParameterSetName)
            {
                case ParameterSet.ResourceReplacement:
                    this.WriteNewResourceReplacementTransform();
                    break;

                default:
                    throw new ArgumentException(string.Format("Bad ParameterSet Name \"{0}\"", this.ParameterSetName));
            }
        }

        private void WriteNewResourceReplacementTransform()
        {
            var transform = new ResourceReplacementTransform(this.ReplacementResource);

            this.WriteVerbose(string.Format("Created new resource replacement transform. Replacement key is \"{0}\"", transform.Replacement));

            this.WriteObject(transform);
        }

        private static class ParameterSet
        {
            public const string ResourceReplacement = "ResourceReplacement";
        }
    }
}
