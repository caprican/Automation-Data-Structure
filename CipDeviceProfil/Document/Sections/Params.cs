
using System.Text.RegularExpressions;

using Odva.Document.Attributes;
using Odva.Helpers;

namespace Odva.Document.Sections;

public class Params : ObjectBase
{
    public List<Parameter> Parameters { get; } = [];
    public List<Enumeration> Enums { get; } = [];

    public override void ReadString(StringSlice text)
    {
        var lineReader = new LineReader(text.ToString());

        //RemoveComment(ref line);

        StringSlice line;
        do
        {
            line = lineReader.ReadLine();
            var paramaterAttributes = Regex.Match(line.ToString(), @"Param(\d+) =");
            if (paramaterAttributes.Success)
            {
                var parameter = new Parameter
                { 
                    Id = uint.Parse(paramaterAttributes.Groups[1].Value),
                    IdFull = paramaterAttributes.Groups[1].Value
                };
                parameter.ReadString(lineReader);
                Parameters.Add(parameter);
            }
            var enumAttributes = Regex.Match(line.ToString(), @"Enum(\d+) =");
            if (enumAttributes.Success)
            {
                var enumeration = new Enumeration
                {
                    Id = uint.Parse(enumAttributes.Groups[1].Value),
                    IdFull = paramaterAttributes.Groups[1].Value
                };
                enumeration.ReadString(lineReader);
                Enums.Add(enumeration);
            }

        } while (!lineReader.IsEnded);
    }

    //public override void WriteString(TextWriter writer)
    //{
    //    throw new NotImplementedException();
    //}
}
