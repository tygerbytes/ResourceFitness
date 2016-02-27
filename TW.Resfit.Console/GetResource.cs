// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetResource.cs" company="Tygertec">
//   Copyright © 2016 Tyrone Walls.
//   All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace TW.Resfit.Console
{
    using System;
    using System.IO;
    using System.Management.Automation;
    using TW.Resfit.Core;
    using TW.Resfit.FileUtils;

    [Cmdlet(VerbsCommon.Get, "Resource")]
    public class GetResource : PSCmdlet
    {
        [Parameter(
            ParameterSetName = ParameterSet.Directory,
            Position = 0,
            Mandatory = true)]
        [Alias("Folder")]
        public string Directory { get; set; }

        [Parameter(
            ParameterSetName = ParameterSet.ResourceList,
            Position = 0,
            Mandatory = true,
            ValueFromPipeline = true)]
        public ResourceList ResourceList { get; set; }

        [Parameter(
            ParameterSetName = ParameterSet.ResxFile,
            Position = 0,
            Mandatory = true)]
        [Alias("File")]
        public string ResxFile { get; set; }

        private FileSystem FileSystem { get; set; }

        protected override void ProcessRecord()
        {
            switch (this.ParameterSetName)
            {
                case ParameterSet.Directory:
                    this.LoadAllResourcesFromPath();
                    break;

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

        protected override void BeginProcessing()
        {
            this.FileSystem = new FileSystem();
        }

        private void LoadAllResourcesFromPath()
        {
            var directoryPath = Path.GetFullPath(this.Directory);
            this.WriteVerbose(string.Format("Loading all resources from path \"{0}\"", directoryPath));

            var resourceList = XmlResourceParser.ParseAllResourceFiles(this.FileSystem, directoryPath);

            this.WriteObject(resourceList);
        }

        private void LoadResourcesFromResxFile()
        {
            var xml = this.FileSystem.LoadXmlFile(this.ResxFile);

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
            public const string Directory = "Directory";

            public const string ResourceList = "ResourceList";

            public const string ResxFile = "ResxFile";
        }
    }
}
