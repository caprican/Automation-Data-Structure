using System.Diagnostics;
using System.Reflection;

using AutomationDataStructure.Contracts.Services;

namespace AutomationDataStructure.Services;

public class ApplicationInfoService : IApplicationInfoService
{
    public Version GetVersion()
    {
        // Set the app version in GSDML Builder > Properties > Package > PackageVersion
        string assemblyLocation = Assembly.GetExecutingAssembly().Location;
        var version = FileVersionInfo.GetVersionInfo(assemblyLocation).FileVersion;
        return new Version(version!);
    }
}
