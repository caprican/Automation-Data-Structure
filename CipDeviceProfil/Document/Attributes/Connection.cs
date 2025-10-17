using Odva.Helpers;

namespace Odva.Document.Attributes;

public class Connection
{
    public uint Id { get; set; }

    public ItemBase<string> Name { get; set; }
    public ItemBase<string> Help { get; set; }
    public ItemBase<ulong> TransportClass { get; set; }
    public ItemBase<ulong> ConnexionType { get; set; }
    public ItemBase<string[]> OTList { get; set; }
    public ItemBase<string[]> TOList { get; set; }

    public ItemBase<string> Path { get; set; }

    public void ReadString(LineReader reader)
    {
        TransportClass = ItemBase<ulong>.GetValue(ref reader);
        ConnexionType = ItemBase<ulong>.GetValue(ref reader);

        OTList = ItemBase<string[]>.GetValue(ref reader);
        TOList = ItemBase<string[]>.GetValue(ref reader);

        reader.ReadLine();
        reader.ReadLine();

        Name = ItemBase<string>.GetValue(ref reader);
        Help = ItemBase<string>.GetValue(ref reader);

        Path = ItemBase<string>.GetValue(ref reader);
    }
}
