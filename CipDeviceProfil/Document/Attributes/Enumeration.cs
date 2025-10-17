using System.Text.RegularExpressions;

using Odva.Helpers;

namespace Odva.Document.Attributes;

public class Enumeration
{
    public uint Id { get; set; }
    public string IdFull { get; set; } = string.Empty;

    public Dictionary<uint, string> Values { get; } = new();

    public void ReadString(LineReader reader)
    {
        StringSlice line;
        Match enumValue;
        do
        {
            line = reader.ReadLine();
            enumValue = Regex.Match(line.ToString(), @"(?:\s+)?(.+),""(.+)""[,;](?:\s+)?(\$(.+))?");
            if (enumValue.Success)
            {
                Values[uint.Parse(enumValue.Groups[1].Value)] = enumValue.Groups[2].Value;
            }
        } while (enumValue.Success);
    }
}
