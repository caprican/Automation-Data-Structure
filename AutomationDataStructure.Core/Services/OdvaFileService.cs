using AutomationDataStructure.Core.Contracts.Services;

using Odva;

namespace AutomationDataStructure.Core.Services;

public class OdvaFileService : IOdvaFileService
{
    public Device OpenDeviceDescription(string path)
    {
        return Odva.Renderers.Device.FromFile(path);
    }
}
