// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileHelper.cs" company="Tygertec">
//   Copyright © 2016 Tyrone Walls.
//   All rights reserved.
// </copyright>
// <summary>
//   Defines the FileHelper type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TW.Resfit.FileUtils
{
    using System;
    using System.IO;
    using System.Xml.Linq;

    using TW.Resfit.Core;

    public static class FileHelper
    {
        public static void WriteToFile(string fileName, string content)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException(fileName, "A filename must be provided.");
            }

            File.WriteAllText(fileName, content);
        }

        public static string LoadFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException(fileName, "A filename must be provided.");
            }

            return File.ReadAllText(fileName);
        }

        public static XElement LoadXmlFile(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path");
            }

            return XElement.Load(path);
        }

        public static ResourceList LoadAllResourcesFromPath(string folderPath)
        {
            throw new NotImplementedException();
        }
    }
}
