// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetResource.cs" company="Tygertec">
//   Copyright © 2016 Tyrone Walls.
//   All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace TW.Resfit.Console
{
    using System;
    using System.Management.Automation;
    using TW.Resfit.Core;
    using TW.Resfit.FileUtils;

    [Cmdlet(VerbsCommon.Get, "Resource")]
    public class GetResource : PSCmdlet
    {
        [Parameter(
            ParameterSetName = ParameterSet.ResourceList,
            Position = 0,
            Mandatory = true)]
        public ResourceList ResourceList { get; set; }

        [Parameter(
            ParameterSetName = ParameterSet.ResxFile,
            Position = 0,
            Mandatory = true)]
        [Alias("File")]
        public string ResxFile { get; set; }

        protected override void ProcessRecord()
        {
            switch (this.ParameterSetName)
            {
                case ParameterSet.ResourceList:
                    this.WriteResourceListItems();
                    break;

                case ParameterSet.ResxFile:
                    this.LoadResourcesFromResxFile();
                    break;

                default:
                    throw new ArgumentException(string.Format("Bad ParameterSet Name \"{0}\"", this.ParameterSetName));
            }
        }

        private void LoadResourcesFromResxFile()
        {
            var fs = new FileSystem();

            var xml = fs.LoadXmlFile(this.ResxFile);

            var resourceList = XmlResourceParser.ParseAsResourceList(xml);

            this.WriteObject(resourceList);
        }

        private void WriteResourceListItems()
        {
            foreach (var resource in this.ResourceList.Items)
            {
                this.WriteObject(resource);
            }
        }

        private static class ParameterSet
        {
            public const string ResourceList = "ResourceList";

            public const string ResxFile = "ResxFile";
        }
    }
}
