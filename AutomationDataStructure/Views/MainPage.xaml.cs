using System.Windows.Controls;

using AutomationDataStructure.ViewModels;

namespace AutomationDataStructure.Views;
public partial class MainPage : Page
{
    public MainPage(MainViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
