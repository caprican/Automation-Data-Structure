using Odva;

namespace AutomationDataStructure.Core.Contracts.Services;

public interface IOdvaFileService
{
    public Device OpenDeviceDescription(string path);
}
