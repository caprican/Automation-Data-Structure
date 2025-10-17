using System.Text.RegularExpressions;

using Odva.Helpers;

namespace Odva.Document.Sections;

public class File : ObjectBase
{
    /// <summary>
    /// DescText
    /// </summary>
    public string? Desciption { get; set; }

    /// <summary>
    /// CreateDate + CreateTime
    /// </summary>
    public DateTime Created { get; set; } = DateTime.MinValue;

    /// <summary>
    /// ModDate + ModTime
    /// </summary>
    public DateTime Modified { get; set; } = DateTime.MinValue;

    public string? Revision { get; set; }

    public string? HomeURL { get; set; }

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
                    case "DescText": Desciption = item.Groups[2].Value; break;
                    case "CreateDate":
                        if (Created == DateTime.MinValue)
                            Created = DateTime.ParseExact(item.Groups[2].Value, "dd-MM-yyyy", null);
                        else
                            Created = Created.Add(TimeSpan.ParseExact(item.Groups[2].Value, "dd-MM-yyyy", null));
                        break;
                    case "CreateTime":
                        if (Created == DateTime.MinValue)
                            Created = DateTime.ParseExact(item.Groups[2].Value, "HH:mm:ss", null);
                        else
                            Created = Created.Add(TimeSpan.Parse(item.Groups[2].Value));
                        break;
                    case "ModDate":
                        if (Modified == DateTime.MinValue)
                            Modified = DateTime.ParseExact(item.Groups[2].Value, "dd-MM-yyyy", null);
                        else
                            Modified = Modified.Add(TimeSpan.ParseExact(item.Groups[2].Value, "dd-MM-yyyy", null));
                        break;
                    case "ModTime":
                        if (Modified == DateTime.MinValue)
                            Modified = DateTime.ParseExact(item.Groups[2].Value, "HH:mm:ss", null);
                        else
                            Modified = Modified.AddTicks(DateTime.ParseExact(item.Groups[2].Value, "HH:mm:ss", null).Ticks);
                        break;
                    case "Revision": Revision = item.Groups[2].Value; break;
                    case "HomeURL": HomeURL = item.Groups[2].Value; break;
                }
            }
        } while (!line.IsEmpty);
    }

    public  void WriteString(TextWriter writer)
    {
        throw new NotImplementedException();
    }
}
