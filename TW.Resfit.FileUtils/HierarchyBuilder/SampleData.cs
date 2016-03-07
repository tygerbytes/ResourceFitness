// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SampleData.cs" company="Tygertec">
//   Copyright © 2016 Ty Walls.
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
    <xsd:schema id=""root"" xmlns="""" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:msdata=""urn:schemas-microsoft-com:xml-msdata"">
    <xsd:import namespace=""http://www.w3.org/XML/1998/namespace"" />
    <xsd:element name=""root"" msdata:IsDataSet=""true"">
      <xsd:complexType>
        <xsd:choice maxOccurs=""unbounded"">
          <xsd:element name=""metadata"">
            <xsd:complexType>
              <xsd:sequence>
                <xsd:element name=""value"" type=""xsd:string"" minOccurs=""0"" />
              </xsd:sequence>
              <xsd:attribute name=""name"" use=""required"" type=""xsd:string"" />
              <xsd:attribute name=""type"" type=""xsd:string"" />
              <xsd:attribute name=""mimetype"" type=""xsd:string"" />
              <xsd:attribute ref=""xml:space"" />
            </xsd:complexType>
          </xsd:element>
          <xsd:element name=""assembly"">
            <xsd:complexType>
              <xsd:attribute name=""alias"" type=""xsd:string"" />
              <xsd:attribute name=""name"" type=""xsd:string"" />
            </xsd:complexType>
          </xsd:element>
          <xsd:element name=""data"">
            <xsd:complexType>
              <xsd:sequence>
                <xsd:element name=""value"" type=""xsd:string"" minOccurs=""0"" msdata:Ordinal=""1"" />
                <xsd:element name=""comment"" type=""xsd:string"" minOccurs=""0"" msdata:Ordinal=""2"" />
              </xsd:sequence>
              <xsd:attribute name=""name"" type=""xsd:string"" use=""required"" msdata:Ordinal=""1"" />
              <xsd:attribute name=""type"" type=""xsd:string"" msdata:Ordinal=""3"" />
              <xsd:attribute name=""mimetype"" type=""xsd:string"" msdata:Ordinal=""4"" />
              <xsd:attribute ref=""xml:space"" />
            </xsd:complexType>
          </xsd:element>
          <xsd:element name=""resheader"">
            <xsd:complexType>
              <xsd:sequence>
                <xsd:element name=""value"" type=""xsd:string"" minOccurs=""0"" msdata:Ordinal=""1"" />
              </xsd:sequence>
              <xsd:attribute name=""name"" type=""xsd:string"" use=""required"" />
            </xsd:complexType>
          </xsd:element>
        </xsd:choice>
      </xsd:complexType>
    </xsd:element>
  </xsd:schema>
  <resheader name=""resmimetype"">
    <value>text/microsoft-resx</value>
  </resheader>
  <resheader name=""version"">
    <value>2.0</value>
  </resheader>
  <resheader name=""reader"">
    <value>System.Resources.ResXResourceReader, System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</value>
  </resheader>
  <resheader name=""writer"">
    <value>System.Resources.ResXResourceWriter, System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</value>
  </resheader>
  <data name=""Resfit_Tests_Banana_Resource_One"" xml:space=""preserve"">
    <value>This is the first Banana resource in the file</value>
  </data>
  <data name=""Resfit_Tests_Banana_Resource_Two"" xml:space=""preserve"">
    <value>This is the second Banana resource in the file</value>
  </data>
  <data name=""Resfit_Tests_Banana_Resource_Three"" xml:space=""preserve"">
    <value>This is the third Banana resource in the file</value>
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

        public static string GenerateRandomTempPath(string baseName = null, string fileName = null)
        {
            if (baseName == null)
            {
                baseName = string.Empty;
            }

            if (fileName == null)
            {
                fileName = string.Empty;
            }

            return Path.Combine(TestingPath(), baseName, DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture), fileName);
        }

        public static string TestingPath()
        {
            return Path.Combine(Path.GetTempPath(), "TW.Resfit.Tests");
        }
    }
}
