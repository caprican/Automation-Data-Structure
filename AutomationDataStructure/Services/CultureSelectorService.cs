using System.Globalization;

using AutomationDataStructure.Contracts.Services;

namespace AutomationDataStructure.Services;

public class CultureSelectorService(ISettingsService settingsService) : ICultureSelectorService
{
    private readonly ISettingsService settingsService = settingsService;

    public void InitializeCulture()
    {
        var culture = GetCurrentCulture();
        SetCulture(culture.Name);
    }

    public void SetCulture(string culture)
    {
        var info = CultureInfo.CreateSpecificCulture(culture);
        Thread.CurrentThread.CurrentCulture = info;
        Thread.CurrentThread.CurrentUICulture = info;
    }

    public CultureInfo GetCurrentCulture()
    {
        if (System.Windows.Application.Current.Properties.Contains(nameof(settingsService.CurrentCulture)))
        {
            var cultureName = System.Windows.Application.Current.Properties[nameof(settingsService.CurrentCulture)]?.ToString();
            if (!string.IsNullOrEmpty(cultureName))
                return CultureInfo.CreateSpecificCulture(cultureName);
            else
                return CultureInfo.CurrentCulture;
        }

        return CultureInfo.CurrentCulture;
    }
}