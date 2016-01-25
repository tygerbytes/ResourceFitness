// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFileSystem.cs" company="Tygertec">
//   Copyright © 2016 Tyrone Walls.
//   All rights reserved.
// </copyright>
// <summary>
//   Defines the IFileSystem type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TW.Resfit.FileUtils
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Xml.Linq;

    public interface IFileSystem
    {
        void WriteToFile(string fileName, string content);

        string LoadFile(string fileName);

        XElement LoadXmlFile(string path);

        IEnumerable<FileInfo> AllFiles(
            string rootPath,
            Regex directoryBlackList = null,
            Regex fileExtensionWhitelist = null);

        void CreateDirectory(string path);
    }
}