using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;

using AutomationDataStructure.Contracts.Services;
using AutomationDataStructure.Core.Contracts.Services;
using AutomationDataStructure.Properties;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

using Microsoft.Extensions.Options;
using Microsoft.Win32;

namespace AutomationDataStructure.ViewModels;

public class ShellViewModel(INavigationService navigationService, IDialogCoordinator dialogCoordinator,
                            IOptions<Core.Models.AppConfig> appConfig, ISettingsService settingsService,
                            ITranscriptService transcriptService) : ObservableObject
{
    private readonly INavigationService navigationService = navigationService;
    private readonly IDialogCoordinator dialogCoordinator = dialogCoordinator;
    private readonly ISettingsService settingsService = settingsService;
    private readonly ITranscriptService transcriptService = transcriptService;

    private readonly Core.Models.AppConfig appConfig = appConfig.Value;
    private readonly string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

    private HamburgerMenuItem? selectedMenuItem;
    private HamburgerMenuItem? selectedOptionsMenuItem;

    private RelayCommand? goBackCommand;
    private ICommand? menuItemInvokedCommand;
    private ICommand? optionsMenuItemInvokedCommand;
    private ICommand? loadedCommand;
    private ICommand? unloadedCommand;

    private ICommand? openCommand;

    public HamburgerMenuItem? SelectedMenuItem
    {
        get { return selectedMenuItem; }
        set { SetProperty(ref selectedMenuItem, value); }
    }

    public HamburgerMenuItem? SelectedOptionsMenuItem
    {
        get { return selectedOptionsMenuItem; }
        set { SetProperty(ref selectedOptionsMenuItem, value); }
    }

    public ObservableCollection<HamburgerMenuItem> MenuItems { get; } =
    [
        //new HamburgerMenuGlyphItem() { Label = Resources.ShellDevicesPage, Glyph = "\uE968", TargetPageType = typeof(DevicesViewModel) },
        //new HamburgerMenuIconItem() { Label = Resources.ShellProfinetDevicePage, Icon = "/Assets/profinet.png", TargetPageType = typeof(ProfinetDeviceViewModel) },
    ];

    public ObservableCollection<HamburgerMenuItem> OptionMenuItems { get; } =
    [
        new HamburgerMenuGlyphItem() { Label = Resources.ShellSettingsPage, Glyph = "\uE713", TargetPageType = typeof(SettingsViewModel) }
    ];

    public RelayCommand GoBackCommand => goBackCommand ??= new RelayCommand(OnGoBack, CanGoBack);

    public ICommand MenuItemInvokedCommand => menuItemInvokedCommand ??= new RelayCommand(OnMenuItemInvoked);
    public ICommand OptionsMenuItemInvokedCommand => optionsMenuItemInvokedCommand ??= new RelayCommand(OnOptionsMenuItemInvoked);
    public ICommand LoadedCommand => loadedCommand ??= new RelayCommand(OnLoaded);
    public ICommand UnloadedCommand => unloadedCommand ??= new RelayCommand(OnUnloaded);

    public ICommand OpenCommand => openCommand ??= new RelayCommand(OnOpen);


    private void OnLoaded()
    {
        navigationService.Navigated += OnNavigated;
    }

    private void OnUnloaded()
    {
        navigationService.Navigated -= OnNavigated;
    }

    private bool CanGoBack() => navigationService.CanGoBack;

    private void OnGoBack() => navigationService.GoBack();

    private void OnMenuItemInvoked() => NavigateTo(SelectedMenuItem!.TargetPageType!);

    private void OnOptionsMenuItemInvoked() => NavigateTo(SelectedOptionsMenuItem!.TargetPageType!);

    private void NavigateTo(Type targetViewModel)
    {
        if (targetViewModel is not null)
        {
            navigationService.NavigateTo(targetViewModel.FullName!);
        }
    }

    private void OnNavigated(object? sender, string? viewModelName)
    {
        var item = MenuItems
                    .OfType<HamburgerMenuItem>()
                    .FirstOrDefault(i => viewModelName == i.TargetPageType?.FullName);
        if (item != null)
        {
            SelectedMenuItem = item;
        }
        else
        {
            SelectedOptionsMenuItem = OptionMenuItems
                    .OfType<HamburgerMenuItem>()
                    .FirstOrDefault(i => viewModelName == i.TargetPageType?.FullName);
        }

        GoBackCommand.NotifyCanExecuteChanged();

#if DEBUG
        var path = @"C:\Users\capri\Downloads\MOD0.eds";
        //var path = @"C:\Users\capri\Downloads\DescriptionFile_AL142x_EDS_EIP_3-1-97\ifm_IOL_Master_AL1420.eds";

        transcriptService.LoadDescriptionFile(path);
#endif
    }

    private void OnOpen()
    {
        var openFile = new OpenFileDialog
        {
            Multiselect = false,
            InitialDirectory = settingsService.NavigationFolder,
            Filter = "Description file (*.eds)|*.eds"
        };

        if (openFile.ShowDialog() == true)
        {
            transcriptService.LoadDescriptionFile(openFile.FileName);
        }
    }
}
