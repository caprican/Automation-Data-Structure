using System.Windows.Controls;

namespace AutomationDataStructure.Contracts.Services;

public interface IPageService
{
    Type GetPageType(string key);

    Page? GetPage(string key);
}