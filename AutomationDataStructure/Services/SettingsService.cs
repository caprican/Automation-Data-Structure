using AutomationDataStructure.Contracts.Services;

namespace AutomationDataStructure.Services;

public class SettingsService : ISettingsService
{
    public string? Theme => System.Windows.Application.Current.Properties[nameof(Theme)]?.ToString();

    public string? CurrentCulture => System.Windows.Application.Current.Properties[nameof(CurrentCulture)]?.ToString();

    public string? NavigationFolder => System.Windows.Application.Current.Properties[nameof(NavigationFolder)]?.ToString();
    public string? ExportFolder => System.Windows.Application.Current.Properties[nameof(ExportFolder)]?.ToString();
    public string DefaultFolder => Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

    public void InitializeSettings()
    {
        if (!System.Windows.Application.Current.Properties.Contains(nameof(NavigationFolder)) || string.IsNullOrEmpty(System.Windows.Application.Current.Properties[nameof(NavigationFolder)]?.ToString()))
        {
            System.Windows.Application.Current.Properties[nameof(NavigationFolder)] = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }

        if (!System.Windows.Application.Current.Properties.Contains(nameof(ExportFolder)) || string.IsNullOrEmpty(System.Windows.Application.Current.Properties[nameof(ExportFolder)]?.ToString()))
        {
            System.Windows.Application.Current.Properties[nameof(ExportFolder)] = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }


    }

    public void SaveNavigationFolder(string navigationFolder)
    {
        System.Windows.Application.Current.Properties[nameof(NavigationFolder)] = navigationFolder;
    }

    public void SaveExportFolder(string exportFolder)
    {
        System.Windows.Application.Current.Properties[nameof(ExportFolder)] = exportFolder;
    }
}
