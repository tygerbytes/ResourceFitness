// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileTransformer.cs" company="Tygertec">
//   Copyright © 2016 Ty Walls.
//   All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace TW.Resfit.Core
{
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;

    using TW.Resfit.FileUtils;

    public class FileTransformer
    {
        private readonly IFileSystem fileSystem;

        private readonly ResourceList resourceList;

        public FileTransformer(IFileSystem fileSystem, ResourceList resourceList)
        {
            this.fileSystem = fileSystem;
            this.resourceList = new ResourceList(resourceList.Resources.Where(x => x.Transforms.Any()));
        }

        public void TransformDirectory(string path, FileFilter filter)
        {
            foreach (var fileInfo in this.fileSystem.AllFiles(path, filter))
            {
                this.TransformFile(fileInfo);
            }
        }

        public void TransformFile(string path)
        {
            this.TransformFile(new FileInfo(path));
        }

        private void TransformFile(FileSystemInfo fileInfo)
        {
            var fileText = this.fileSystem.LoadFile(fileInfo.FullName);

            if (!this.FileImpactedByTransforms(fileText))
            {
                return;
            }

            this.fileSystem.WriteToFile(fileInfo.FullName, this.TransformFileText(fileInfo, fileText));
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
