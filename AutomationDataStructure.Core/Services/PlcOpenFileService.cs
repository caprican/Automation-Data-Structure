using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

using AutomationDataStructure.Core.Contracts.Services;

namespace AutomationDataStructure.Core.Services;

public class PlcOpenFileService : IPlcOpenFileService
{
    public PlcOpenML.Project.Project? OpenFile(string path)
    {
        string xml = File.ReadAllText(path);
        var xmlNamespace = Regex.Match(xml, @"xmlns(:\w+)?=""(.+)""");
        if (!xmlNamespace.Success)
        {
            throw new DataException("Namespace not found in XML.");
        }

        var serializer = new XmlSerializer(typeof(PlcOpenML.Project.Project), new XmlRootAttribute
        {
            ElementName = "project",
            Namespace = xmlNamespace.Groups[2].Value
        });

        using var reader = new StringReader(xml);
        if (serializer.Deserialize(reader) is PlcOpenML.Project.Project project)
        {
            return project;
        }

        return null;
    }

    public void Save(PlcOpenML.Project.Project project, string path)
    {
        var serializer = new XmlSerializer(typeof(PlcOpenML.Project.Project), new XmlRootAttribute
        {
            ElementName = "project",
            Namespace = project.Namespace
        });
        var settings = new XmlWriterSettings
        {
            Indent = true,
            OmitXmlDeclaration = false
        };
        using var stringWriter = new StringWriter();
        using (var xmlWriter = XmlWriter.Create(stringWriter, settings))
        {
            serializer.Serialize(xmlWriter, project);
        }
        var xmlOutput = stringWriter.ToString();

        using var outputFile = new StreamWriter(path);
        outputFile.Write(xmlOutput);
    }
}
