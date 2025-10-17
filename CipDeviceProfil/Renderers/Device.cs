namespace Odva.Renderers;

public static class Device
{
    public static Odva.Device FromFile(string filePath)
    {
        var device = new Odva.Device
        {
            Document = new Document.Document(filePath)
        };

        device.Name = device.Document.Device.ProductName;

        var connection = (device.Document.ConnectionManager?.Items.FirstOrDefault()) ?? throw new Exception("");
        if (device.Document.Assemblies?.Items?.Count > 0)
        {
            var assemblageName = connection?.TOList.Value.FirstOrDefault(f => f.StartsWith("Assem"));
            var assIn = device.Document.Assemblies.Items.Find(f => f.Id == uint.Parse(assemblageName?.Substring(5)));
            if (assIn?.Items.Count > 0)
            {
                foreach (var ass in assIn.Items) 
                {
                    var param = device.Document.Parameters.Parameters.Find(f => f.Id == uint.Parse(ass.ParameterId.Substring(5)));

                    device.Inputs.Add(new Models.InterfaceObject(param));
                }

            }


            assemblageName = connection?.OTList.Value.FirstOrDefault(f => f.StartsWith("Assem"));
            var assOut = device.Document.Assemblies.Items.Find(f => f.Id == uint.Parse(assemblageName?.Substring(5)));
            if (assOut?.Items.Count > 0)
            {
                foreach (var ass in assOut.Items)
                {
                    var param = device.Document.Parameters.Parameters.Find(f => f.Id == uint.Parse(ass.ParameterId.Substring(5)));

                    device.Outputs.Add(new Models.InterfaceObject(param));
                }

            }
        }

        return device;
    }
}
