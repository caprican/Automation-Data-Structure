using System.Windows;
using System.Windows.Controls;

using AutomationDataStructure.Contracts.Views;
using AutomationDataStructure.ViewModels;

using MahApps.Metro.Controls;

namespace AutomationDataStructure.Views;

public partial class ShellWindow : MetroWindow, IShellWindow
{
    public ShellWindow(ShellViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }

    public Frame GetNavigationFrame() => shellFrame;

    public void ShowWindow() => Show();

    public void CloseWindow() => Close();
}
