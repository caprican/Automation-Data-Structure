using System.Text.RegularExpressions;

using Odva.Helpers;

namespace Odva.Document.Sections;

public class Device : ObjectBase
{
    public uint? VendorId { get; set; }
    public string? VendorName { get; set; }
    public uint DeviceType { get; set; }
    public string? DeviceTypeDescription { get; set; }
    public uint DeviceId { get; set; }

    public Version? Revision { get; set; }

    public string ProductName { get; set; } = string.Empty;

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
                    case "VendCode": VendorId = uint.Parse(item.Groups[2].Value); break;
                    case "VendName": VendorName = item.Groups[2].Value; break;
                    case "ProdType": DeviceType = uint.Parse(item.Groups[2].Value); break;
                    case "ProdTypeStr": DeviceTypeDescription = item.Groups[2].Value; break;
                    case "ProdCode": DeviceId = uint.Parse(item.Groups[2].Value); break;
                    case "MajRev": break;
                    case "MinRev": break;
                    case "ProdName": ProductName = item.Groups[2].Value; break;
                }
            }
        } while (!line.IsEmpty);
    }

}
