using System.Runtime.CompilerServices;

using Odva.Helpers;

namespace Odva.Document;

public class ItemBase<T>
{
    public T? Value { get; set; }

    public string Comment { get; set; } = string.Empty;

    internal static ItemBase<T> GetValue(ref LineReader reader)
    {
        var lineText = reader.ReadLine().ToString();
        var comment = lineText.Contains('$') ? lineText[(lineText.IndexOf('$') + 1)..] : string.Empty;

        var text = lineText.Contains('$') ? lineText[..lineText.IndexOf('$')] : lineText;
        var contains = text.Trim().Split(',', ';');

        while (reader.PeekLine().ToString().Trim().StartsWith('$'))
        {
            comment += reader.ReadLine().ToString().Trim()[1..];
        }

        if (contains[0].StartsWith("0x"))
        {
            contains[0] = uint.Parse(contains[0].Trim().Substring(2), System.Globalization.NumberStyles.HexNumber).ToString();
        }

        return new ItemBase<T>
            {
                Value = typeof(T) switch
                {
                    Type t when t == typeof(uint) => (T)Convert.ChangeType(uint.Parse(contains[0]), typeof(T)),
                    Type t when t == typeof(ulong) => (T)Convert.ChangeType(ulong.Parse(contains[0]), typeof(T)),
                    Type t when t == typeof(string[]) => (T)Convert.ChangeType(contains[..(contains.Length - 1)], typeof(T)),
                    Type t when t == typeof(string) => (T)Convert.ChangeType(contains[0], typeof(T)),
                    _ => default,
                },
                Comment = comment
            };
    }
}
