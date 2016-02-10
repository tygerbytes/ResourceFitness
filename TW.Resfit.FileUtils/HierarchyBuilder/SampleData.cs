// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SampleData.cs" company="Tygertec">
//   Copyright © 2016 Tyrone Walls.
//   All rights reserved.
// </copyright>
// <summary>
//   Defines the SampleData type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TW.Resfit.FileUtils.HierarchyBuilder
{
    using System;
    using System.Globalization;
    using System.IO;

    public static class SampleData
    {
        public const string SampleXmlResourceString = @"<?xml version=""1.0"" encoding=""utf-8""?>
<root>
	<data name=""Resfit_Tests_Banana_Resource_One"" xml:space=""preserve"">
    <value>This is the first resource in the file</value>
  </data>
  <data name=""Resfit_Tests_Banana_Resource_Two"" xml:space=""preserve"">
    <value>This is the second resource in the file</value>
  </data>
  <data name=""Resfit_Tests_Banana_Resource_Three"" xml:space=""preserve"">
    <value>This is the third resource in the file</value>
  </data>
</root>
";

        public const string SampleSourceFile = @"// A sample source file
// Here I am referencing ""Resfit_Tests_Banana_Resource_Three""
public static void DoStuff()
{
    // Does some stuff
    var msg1 = Localization.Resfit_Tests_Banana_Resource_One;
    var msg2 = Localization.Resfit_Tests_Banana_Resource_Two;

    WriteStuff(msg1)
    WriteMoreStuff(msg2)
}";

        public static string SampleFruitySourceFile(string fruitName)
        {
            return SampleSourceFile.Replace("Banana", fruitName);
        }

        public static string SampleXmlFruitResourceString(string fruitName)
        {
            return SampleXmlResourceString.Replace("Banana", fruitName);
        }

        public static void CreateSampleFileHierarchy(IFileSystem fileSystem, string rootPath)
        {
            var builder = new FileHierarchyBuilder(rootPath);
            
            builder.AddFiles(
                new[]
                    {
                        new FileNode("ATextFile.txt", "Some contents"),
                        new FileNode("AnotherTextFile.txt", "Some more contents"),
                        new FileNode("Apples.resx", SampleXmlFruitResourceString("Apple")),
                        new FileNode("Apples.cs", SampleFruitySourceFile("Apple"))
                    })
                .AddDirectory("Folder1")
                .AddFiles(
                    new[]
                        {
                            new FileNode("ASourceFile.cs", @"//Some source code"),
                            new FileNode("Oranges.resx", SampleXmlFruitResourceString("Orange")),
                            new FileNode("Oranges.cs", SampleFruitySourceFile("Orange"))
                        });

            builder.Execute(fileSystem);
        }

        public static string GenerateRandomTempPath(string baseName = null)
        {
            if (baseName == null)
            {
                baseName = string.Empty;
            }

            return Path.Combine(Path.GetTempPath(), "TW.Resfit.Tests", baseName, DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture));
        }
    }
}
