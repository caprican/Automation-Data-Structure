
namespace AutomationDataStructure.Core.Contracts.Services;

public interface IPlcOpenFileService
{
    public PlcOpenML.Project.Project? OpenFile(string path);
    public void Save(PlcOpenML.Project.Project project, string path);
}
