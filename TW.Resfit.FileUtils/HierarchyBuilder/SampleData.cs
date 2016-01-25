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
                        new FileNode("Resources.resx", SampleXmlFruitResourceString("Apple"))
                    })
                .AddDirectory("Folder1")
                .AddFiles(
                    new[]
                        {
                            new FileNode("ASourceFile.cs", @"//Some source code"),
                            new FileNode("AResourceFile.resx", SampleXmlFruitResourceString("Orange"))
                        });

            builder.Execute(fileSystem);
        }
    }
}
