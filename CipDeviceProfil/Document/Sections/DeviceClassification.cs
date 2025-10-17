
using System.Text.RegularExpressions;

using Odva.Helpers;

namespace Odva.Document.Sections;

public class DeviceClassification : ObjectBase
{
    public string? Class1 { get; set; }
    public string? Class2 { get; set; }
    public string? Class3 { get; set; }

    public override void ReadString(StringSlice text)
    {
        var lineReader = new LineReader(text.ToString());

        StringSlice line;
        do
        {
            line = lineReader.ReadLine();
            var item = Regex.Match(line.ToString(), @"\s+(.+) = (.+);");
            if (item.Success)
            {
                switch (item.Groups[1].Value)
                {
                    case "Class1": Class1 = item.Groups[2].Value; break;
                }
            }
        } while (!line.IsEmpty);
    }

    //public override void WriteString(TextWriter writer)
    //{
    //    throw new NotImplementedException();
    //}
}
