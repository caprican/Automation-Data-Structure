using AutomationDataStructure.Contracts.Services;
using AutomationDataStructure.Contracts.Views;
using AutomationDataStructure.ViewModels;

using Microsoft.Extensions.Hosting;

namespace AutomationDataStructure.Services;

public class ApplicationHostService(IServiceProvider serviceProvider, INavigationService navigationService,
                                    ISettingsService settingsService, IPersistAndRestoreService persistAndRestoreService,
                                    IThemeSelectorService themeSelectorService, ICultureSelectorService cultureSelectorService) : IHostedService
{
    private readonly IServiceProvider serviceProvider = serviceProvider;
    private readonly INavigationService navigationService = navigationService;
    private readonly IPersistAndRestoreService persistAndRestoreService = persistAndRestoreService;
    private readonly ISettingsService settingsService = settingsService;
    private readonly IThemeSelectorService themeSelectorService = themeSelectorService;
    private readonly ICultureSelectorService cultureSelectorService = cultureSelectorService;

    private bool isInitialized;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        // Initialize services that you need before app activation
        await InitializeAsync();

        await HandleActivationAsync();

        // Tasks after activation
        await StartupAsync();
        isInitialized = true;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        persistAndRestoreService.PersistData();
        await Task.CompletedTask;
    }

    private async Task InitializeAsync()
    {
        if (!isInitialized)
        {
            persistAndRestoreService.RestoreData();
            settingsService.InitializeSettings();
            themeSelectorService.InitializeTheme();
            cultureSelectorService.InitializeCulture();

            await Task.CompletedTask;


            await Task.CompletedTask;

        }
    }

    private async Task StartupAsync()
    {
        if (!isInitialized)
        {
            await Task.CompletedTask;
        }
    }

    private async Task HandleActivationAsync()
    {
        if (!System.Windows.Application.Current.Windows.OfType<IShellWindow>().Any())
        {
            // Default activation that navigates to the apps default page
            if (serviceProvider.GetService(typeof(IShellWindow)) is IShellWindow shellWindow)
            {
                navigationService.Initialize(shellWindow.GetNavigationFrame());
                shellWindow.ShowWindow();
                navigationService.NavigateTo(typeof(MainViewModel).FullName!);
            }

            await Task.CompletedTask;
        }
    }
}
