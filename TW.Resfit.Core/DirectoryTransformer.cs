// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DirectoryTransformer.cs" company="Tygertec">
//   Copyright © 2016 Ty Walls.
//   All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace TW.Resfit.Core
{
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Xml.Linq;

    using TW.Resfit.FileUtils;

    public class DirectoryTransformer
    {
        private readonly IFileSystem fileSystem;

        private readonly ResourceList resourceList;

        public DirectoryTransformer(IFileSystem fileSystem, ResourceList resourceList)
        {
            this.fileSystem = fileSystem;
            this.resourceList = new ResourceList(resourceList.Resources.Where(x => x.Transforms.Any()));
        }

        public void TransformDirectory(string path)
        {
            var whiteList = new Regex(@"\.(?:resx|cs)$");
            foreach (var fileInfo in FileSystem.Instance.AllFiles(path, null, whiteList))
            {
                var fileText = this.fileSystem.LoadFile(fileInfo.FullName);

                if (!this.FileImpactedByTransforms(fileText))
                {
                    continue;
                }

                this.fileSystem.WriteToFile(fileInfo.FullName, this.TransformFileText(fileInfo, fileText));
            }
        }

        private bool FileImpactedByTransforms(string fileText)
        {
            return
                this.resourceList.Resources.Any(
                    resource => resource.Transforms.Any(transform => transform.WillAffect(fileText, resource)));
        }

        private string TransformFileText(FileSystemInfo fileInfo, string fileText)
        {
            switch (fileInfo.Extension)
            {
                case ".resx":
                    return this.TransformXml(fileText);

                case ".cs":
                    return this.TransformGenericSourceCode(fileText);

                default:
                    return fileText;
            }
        }

        private string TransformGenericSourceCode(string fileText)
        {
            foreach (var resource in this.resourceList.Resources)
            {
                resource.Transforms.ForEach(x => x.Transform(ref fileText, resource));
            }

            return fileText;
        }

        private string TransformXml(string fileText)
        {
            var xml = XElement.Parse(fileText);

            foreach (var resource in this.resourceList.Resources)
            {
                resource.Transforms.ForEach(x => x.Transform(ref xml, resource));
            }

            return xml.ToString();
        }
    }
}
