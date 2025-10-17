using System.Text.RegularExpressions;

using Odva.Helpers;
using Odva.Models;

namespace Odva.Document.Attributes;

public class Parameter
{
    public uint Id { get; set; }
    public string IdFull { get; set; } = string.Empty;
    public string? LinkPath { get; set; } 
    public uint Descriptor { get; set; } = 0;
    public DataType DataType { get; set; }
    public int DataSize { get; set; }
    public string? Name { get; set; }

    public string Units { get; set; } = string.Empty;
    public string Help { get; set; } = string.Empty;

    public ulong? Min { get; set; }
    public ulong? Max { get; set; }
    public ulong? Default { get; set; }

    public ulong? ScalingMultiplier { get; set; }
    public ulong? ScalingDivisor { get; set; }
    public ulong? ScalingBase { get; set; }
    public ulong? ScalingOffset { get; set; }

    public ulong? LinkMultiplier { get; set; }
    public ulong? LinkDivisor { get; set; }
    public ulong? LinkBase { get; set; }
    public ulong? LinkOffset { get; set; }

    public uint? DecimalPlaces { get; set; }

    public void ReadString(LineReader reader)
    {
        var line = reader.ReadLine().ToString();
        if (!line.Contains("0,") && !line.Contains("$ Reserved, shall equal 0"))
            throw new NotImplementedException();

        // Link Path Size, Link Path
        var regLine = Regex.Match(reader.ReadLine().ToString(), @"(?:\s+)?(.+),(?:\s+)?(\$(.+))?");
        if(regLine.Success)
        {
            var val = regLine.Groups[1].Value.Trim().Split(',');
            if (!string.IsNullOrEmpty(val[0]))
            {
                LinkPath = val[1].Trim();
            }
        }

        // Descriptor
        regLine = Regex.Match(reader.ReadLine().ToString(), @"(?:\s+)?(.+),(?:\s+)?(\$(.+))?");
        if(regLine.Success)
        {
            Descriptor = uint.Parse(regLine.Groups[1].Value.Trim().Substring(2), System.Globalization.NumberStyles.HexNumber);
        }

        // Data Type
        regLine = Regex.Match(reader.ReadLine().ToString(), @"(?:\s+)?(.+),(?:\s+)?(\$(.+))?");
        if(regLine.Success)
        {
            DataType = (DataType)uint.Parse(regLine.Groups[1].Value.Trim().Substring(2), System.Globalization.NumberStyles.HexNumber);
        }

        // Data Size in bytes
        regLine = Regex.Match(reader.ReadLine().ToString(), @"(?:\s+)?(.+),(?:\s+)?(\$(.+))?");
        if(regLine.Success)
        {
            DataSize = int.Parse(regLine.Groups[1].Value.Trim());
        }

        // Parameter name
        regLine = Regex.Match(reader.ReadLine().ToString(), @"(?:\s+)?(.+),(?:\s+)?(\$(.+))?");
        if(regLine.Success)
        {
            Name = regLine.Groups[1].Value.Trim();
        }

        // Units
        regLine = Regex.Match(reader.ReadLine().ToString(), @"(?:\s+)?(.+),(?:\s+)?(\$(.+))?");
        if(regLine.Success)
        {
            Units = regLine.Groups[1].Value.Trim();
        }

        // Help string
        regLine = Regex.Match(reader.ReadLine().ToString(), @"(?:\s+)?(.+),(?:\s+)?(\$(.+))?");
        if(regLine.Success)
        {
            Help = regLine.Groups[1].Value.Trim();
        }

        // min, max, default data values
        regLine = Regex.Match(reader.ReadLine().ToString(), @"(?:\s+)?(.+),(?:\s+)?(\$(.+))?");
        if(regLine.Success)
        {
            var val = regLine.Groups[1].Value.Trim().Split(',');

            if(ulong.TryParse(val[0].Trim(), out var min))
                Min = min;

            if(ulong.TryParse(val[1].Trim(), out var max))
                Max = max;

            if(ulong.TryParse(val[2].Trim(), out var def))
                Default = def;
        }

        // mult, div, base, offset scaling
        regLine = Regex.Match(reader.ReadLine().ToString(), @"(?:\s+)?(.+),(?:\s+)?(\$(.+))?");
        if (regLine.Success)
        {
            var val = regLine.Groups[1].Value.Trim().Split(',');
            if (ulong.TryParse(val[0].Trim(), out var scalingMultiplier))
                ScalingMultiplier = scalingMultiplier;

            if(ulong.TryParse(val[1].Trim(), out var scalingDivisor))
                ScalingDivisor = scalingDivisor;
            
            if(ulong.TryParse(val[2].Trim(), out var scalingBase))
                ScalingBase = scalingBase;
            
            if(ulong.TryParse(val[3].Trim(), out var scalingOffset))
                ScalingOffset = scalingOffset;
        }

        // mult, div, base, offset links
        regLine = Regex.Match(reader.ReadLine().ToString(), @"(?:\s+)?(.+),(?:\s+)?(\$(.+))?");
        if (regLine.Success)
        {
            var val = regLine.Groups[1].Value.Trim().Split(',');

            if(ulong.TryParse(val[0].Trim(), out var linkMultiplier))
                LinkMultiplier = linkMultiplier;
            
            if(ulong.TryParse(val[1].Trim(), out var linkDivisor))
                LinkDivisor = linkDivisor;
            
            if(ulong.TryParse(val[2].Trim(), out var linkBase))
                LinkBase = linkBase;

            if(ulong.TryParse(val[3].Trim(), out var linkOffset))
                LinkOffset = linkOffset;
        }

        // Decimal places
        regLine = Regex.Match(reader.ReadLine().ToString(), @"(?:\s+)?(.+);(?:\s+)?(\$(.+))?");
        if (regLine.Success)
        {
            if (uint.TryParse(regLine.Groups[1].Value.Trim(), out var decimalPlaces))
                DecimalPlaces = decimalPlaces;
        }
        else
        {
            throw new NotImplementedException();
        }
    }
}