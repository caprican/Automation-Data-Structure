using System.Text.RegularExpressions;

using Odva.Helpers;

namespace Odva.Document;

public abstract class ObjectBase
{
    public string? Comment { get; set; }

    public void RemoveComment(ref string line)
    {
        var commLine = Regex.Match(line, @"\$(.+)");
        if (commLine.Success)
        {
            Comment = commLine.Value.Substring(1).Trim();
            line.Remove(commLine.Groups[1].Index - 1).Trim();
        }
    }

    public abstract void ReadString(StringSlice text);
    //public abstract void WriteString(TextWriter writer);
}
