using System.Collections;
using System.IO;

using AutomationDataStructure.Contracts.Services;
using AutomationDataStructure.Core.Contracts.Services;

using Microsoft.Extensions.Options;

namespace AutomationDataStructure.Services;

public class PersistAndRestoreService(IFileService fileService, IOptions<Core.Models.AppConfig> appConfig) : IPersistAndRestoreService
{
    private readonly IFileService fileService = fileService;
    private readonly Core.Models.AppConfig appConfig = appConfig.Value;
    private readonly string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

    public void PersistData()
    {
        if (System.Windows.Application.Current.Properties is not null)
        {
            var folderPath = Path.Combine(localAppData, appConfig.ConfigurationsFolder);
            var fileName = appConfig.AppPropertiesFileName;
            fileService.Save(folderPath, fileName, System.Windows.Application.Current.Properties);
        }
    }

    public void RestoreData()
    {
        var folderPath = Path.Combine(localAppData, appConfig.ConfigurationsFolder);
        var fileName = appConfig.AppPropertiesFileName;
        var properties = fileService.Read<IDictionary>(folderPath, fileName);
        if (properties is not null)
        {
            foreach (DictionaryEntry property in properties)
            {
                System.Windows.Application.Current.Properties.Add(property.Key, property.Value);
            }
        }
    }
}