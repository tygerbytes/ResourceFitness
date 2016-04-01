// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFileSystem.cs" company="Tygertec">
//   Copyright © 2016 Ty Walls.
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
        IEnumerable<FileInfo> AllFiles(
            string rootPath,
            Regex directoryBlackList = null,
            Regex fileExtensionWhitelist = null);

        void CreateDirectory(string path);

        string LoadFile(string fileName);

        XElement LoadXmlFile(string path);

        void WriteToFile(string fileName, string content);
    }
}
