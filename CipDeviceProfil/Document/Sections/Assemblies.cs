using System.Text.RegularExpressions;

using Odva.Document.Attributes;
using Odva.Helpers;

namespace Odva.Document.Sections;

public class Assemblies : ObjectBase
{
    public string Name { get; set; } = string.Empty;
    public string? ClassCode { get; set; }
    public List<Assembly> Items { get; set; } = [];

    public override void ReadString(StringSlice text)
    {
        var lineReader = new LineReader(text.ToString());

        StringSlice line;
        do
        {
            line = lineReader.ReadLine();

            var item = Regex.Match(line.ToString(), @"(?:\s+)?(.+) = (.+);");
            if(item.Success)
            {
                switch(item.Groups[1].Value)
                {
                    case "Object_Name": Name = item.Groups[2].Value; break;
                    case "Object_Class_Code": ClassCode = item.Groups[2].Value; break;
                }
            }

            item = Regex.Match(line.ToString(), @"Assem(\d+) =");
            if(item.Success)
            {
                var assembly = new Assembly
                {
                    Id = uint.Parse(item.Groups[1].Value)
                };
                assembly.ReadString(lineReader);
                Items.Add(assembly);
            }
        } while (!lineReader.IsEnded);
    }

    //public override void WriteString(TextWriter writer)
    //{
    //    throw new NotImplementedException();
    //}
}