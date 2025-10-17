using System.Globalization;

namespace AutomationDataStructure.Contracts.Services;

public interface ICultureSelectorService
{
    void InitializeCulture();

    void SetCulture(string culture);

    CultureInfo GetCurrentCulture();
}