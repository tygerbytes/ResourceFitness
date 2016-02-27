// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NewResource.cs" company="Tygertec">
//   Copyright © 2016 Tyrone Walls.
//   All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace TW.Resfit.Console
{
    using System.Collections.Generic;
    using System.Management.Automation;
    using System.Text;

    using TW.Resfit.Core;

    [Cmdlet(VerbsCommon.New, "Resource")]
    public class NewResource : Cmdlet
    {
        [Parameter(Position = 1, Mandatory = true, ValueFromPipeline = true)]
        public string Key { get; set; }

        [Parameter(Position = 2)]
        public string Value { get; set; }

        [Parameter(Position = 3)]
        public ResourceFormat Format { get; set; }

        [Parameter(Position = 4)]
        public List<ITransform> Transforms { get; set; }

        protected override void ProcessRecord()
        {
            var r = new Resource(this.Key, this.Value ?? string.Empty, this.Format ?? ResourceFormat.Default);

            this.WriteVerbose(string.Format(
                "Created new resource:\n\tKey: {0}\n\tValue: {1}\n\tFormat: {2}\n\tTransforms: {3}",
                r.Key,
                r.Value,
                r.ResourceFormat.Name,
                TransformsToString(r)));

            this.WriteObject(r);
        }

        private static string TransformsToString(Resource resource)
        {
            var sb = new StringBuilder();

            foreach (var transform in resource.Transforms)
            {
                sb.AppendLine(string.Format("\t\t{0}", transform));
            }

            return sb.ToString();
        }
    }
}
