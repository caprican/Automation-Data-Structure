
using Odva.Document.Attributes;

namespace Odva.Models;

public record InterfaceObject
{
    private Parameter deviceParameter { get; set; }

    public string Name => deviceParameter.Name ?? "No name";
    public Models.DataType DataType => deviceParameter.DataType;
    public int Size => deviceParameter.DataSize;

    public InterfaceObject(Parameter parameter)
    {
        deviceParameter = parameter;
    }
}
