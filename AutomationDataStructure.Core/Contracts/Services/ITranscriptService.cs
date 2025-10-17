using Odva;

namespace AutomationDataStructure.Core.Contracts.Services;

public interface ITranscriptService
{
    public event EventHandler<object?>? SourceLoaded;

    public void LoadDescriptionFile(string path);
    public void SetSource(Device device);

    public Device GetSource();

    public (string, string, string, string) ConvertToCodesys(string deviceName);
}
