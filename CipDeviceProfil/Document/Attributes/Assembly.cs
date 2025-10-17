using System.Text.RegularExpressions;

using Odva.Helpers;

namespace Odva.Document.Attributes;

public class Assembly
{
    public uint Id { get; set; }
    public ItemBase<string>? Name { get; set; }
    public ItemBase<string>? LinkPath { get; set; }

    public ItemBase<uint>? Size { get; set; }
    public ItemBase<uint>? Descriptor { get; set; }

    public List<(string ParameterId, ItemBase<uint> Size)> Items { get; set; } = new();

    public void ReadString(LineReader reader)
    {
        string lineText;

        // Name of the assembly
        var regLine = Regex.Match(reader.ReadLine().ToString(), @"(?:\s+)?""(.+)"",(?:\s+)?(\$(.+))?");
        if (regLine.Success)
        {
            var comment = regLine.Groups[2].Value.Trim();

            Name ??= new ItemBase<string> 
            { 
                Value = regLine.Groups[1].Value.Trim(),
                Comment = comment
            };
        }

        // Link Path
        regLine = Regex.Match(reader.ReadLine().ToString(), @"(?:\s+)?""(.+)"",(?:\s+)?(\$(.+))?");
        if (regLine.Success)
        {
            LinkPath ??= new ItemBase<string>
            {
               Value = regLine.Groups[1].Value.Trim() 
            };
        }

        // Size of the Data Block in bytes
        regLine = Regex.Match(reader.ReadLine().ToString(), @"(?:\s+)?(.+),(?:\s+)?(\$(.+))?");
        if (regLine.Success)
        {
            Size ??= new ItemBase<uint>
            {
                Value = uint.Parse(regLine.Groups[1].Value.Trim()),
                Comment = regLine.Groups[2].Value.TrimStart()
            };
        }

        // Descriptor
        Descriptor ??= ItemBase<uint>.GetValue(ref reader);

        if (!reader.ReadLine().ToString().Contains(",,"))
            throw new NotImplementedException();

        do
        {
            lineText = reader.ReadLine().ToString();
            var comment = lineText.Contains('$') ? lineText[(lineText.IndexOf('$') + 1)..] : string.Empty;

            var text = lineText.Contains('$') ? lineText[..lineText.IndexOf('$')] : lineText;
            var contains = text.Trim().Split(',', ';');

            Items.Add((contains[1], new ItemBase<uint>
            {
                Value = uint.Parse(contains[0]),
                Comment = comment
            }));

        } while (!lineText.Contains(';'));
    }
}
