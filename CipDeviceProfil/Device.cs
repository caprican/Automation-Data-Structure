namespace Odva;

public class Device
{
    internal Document.Document Document { get; set; } = new();

    public string Name { get; set; } = string.Empty;

    public List<Models.InterfaceObject> Inputs { get; set; } = [];
    public List<Models.InterfaceObject> Outputs { get; set; } = [];
}
