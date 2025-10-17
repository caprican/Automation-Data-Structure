using System.Windows.Controls;

using AutomationDataStructure.ViewModels;

namespace AutomationDataStructure.Views;

/// <summary>
/// Logique d'interaction pour SettingsPage.xaml
/// </summary>
public partial class SettingsPage : Page
{
    public SettingsPage(SettingsViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
