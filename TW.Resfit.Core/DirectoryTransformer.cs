// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DirectoryTransformer.cs" company="Tygertec">
//   Copyright © 2016 Ty Walls.
//   All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace TW.Resfit.Core
{
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Xml.Linq;

    using TW.Resfit.FileUtils;

    public class DirectoryTransformer
    {
        private readonly IFileSystem fileSystem;

        public DirectoryTransformer(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public void TransformDirectory(string path, ResourceList resourceList)
        {
            var whiteList = new Regex(@"\.(?:resx|cs)$");
            foreach (var fileInfo in FileSystem.Instance.AllFiles(path, null, whiteList))
            {
                var fileText = this.fileSystem.LoadFile(fileInfo.FullName);

                // Does this file require changes?
                var makeChanges =
                    resourceList.Resources.Where(x => x.Transforms.Any())
                        .Any(
                            resource =>
                            resource.Transforms.Any(transform => transform.WillAffect(ref fileText, resource)));

                if (!makeChanges)
                {
                    continue;
                }

                XElement xml = null;
                if (fileInfo.Extension == ".resx")
                {
                    xml = this.fileSystem.LoadXmlFile(fileInfo.FullName);
                }

                foreach (var resource in resourceList.Resources.Where(x => x.Transforms.Any()))
                {
                    if (fileInfo.Extension == ".resx")
                    {
                        resource.Transforms.ForEach(x => x.Transform(ref xml, resource));
                    }
                    else
                    {
                        resource.Transforms.ForEach(x => x.Transform(ref fileText, resource));
                    }
                }

                if (xml != null)
                {
                    fileText = xml.ToString();
                }

                this.fileSystem.WriteToFile(fileInfo.FullName, fileText);
            }
        }

    }
}
