using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;

using AutomationDataStructure.Contracts.Services;
using AutomationDataStructure.Contracts.Views;
using AutomationDataStructure.Core.Contracts.Services;
using AutomationDataStructure.Core.Services;
using AutomationDataStructure.Services;
using AutomationDataStructure.ViewModels;
using AutomationDataStructure.Views;

using MahApps.Metro.Controls.Dialogs;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AutomationDataStructure;

public partial class App : Application
{
    private IHost? host;

    public T? GetService<T>() where T : class => host?.Services.GetService(typeof(T)) as T;

    private async void OnStartup(object sender, StartupEventArgs e)
    {
        var appLocation = Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location);

        //System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("none");

        // For more information about .NET generic host see  https://docs.microsoft.com/aspnet/core/fundamentals/host/generic-host?view=aspnetcore-3.0
        host = Host.CreateDefaultBuilder(e.Args)
                .ConfigureAppConfiguration(c =>
                {
                    c.SetBasePath(appLocation!);
                })
                .ConfigureServices(ConfigureServices)
                .Build();

        await host.StartAsync();
    }

    private void ConfigureServices(HostBuilderContext context, IServiceCollection services)
    {
        // App Host
        services.AddHostedService<ApplicationHostService>();

        services.AddSingleton<IDialogCoordinator, DialogCoordinator>();

        // Core Services
        services.AddSingleton<IFileService, FileService>();
        services.AddSingleton<ITranscriptService, TranscriptService>();

        services.AddTransient<IOdvaFileService, OdvaFileService>();
        services.AddTransient<IPlcOpenFileService, PlcOpenFileService>();
        services.AddTransient<ICodesysFileService, CodesysFileService>();

        // Services
        services.AddSingleton<ISettingsService, SettingsService>();
        services.AddSingleton<IApplicationInfoService, ApplicationInfoService>();
        //services.AddSingleton<ISystemService, SystemService>();
        services.AddSingleton<IPersistAndRestoreService, PersistAndRestoreService>();
        services.AddSingleton<IThemeSelectorService, ThemeSelectorService>();
        services.AddSingleton<ICultureSelectorService, CultureSelectorService>();
        services.AddSingleton<IPageService, PageService>();
        services.AddSingleton<INavigationService, NavigationService>();

        // Views and ViewModels
        services.AddTransient<IShellWindow, ShellWindow>();
        services.AddTransient<ShellViewModel>();

        services.AddTransient<MainViewModel>();
        services.AddTransient<MainPage>();

        services.AddTransient<SettingsViewModel>();
        services.AddTransient<SettingsPage>();

        // Configuration
        services.Configure<Core.Models.AppConfig>(context.Configuration.GetSection(nameof(Core.Models.AppConfig)));
    }

    private async void OnExit(object sender, ExitEventArgs e)
    {
        await host!.StopAsync();
        host.Dispose();
        host = null;
    }

    private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        // TODO: Please log and handle the exception as appropriate to your scenario
        // For more info see https://docs.microsoft.com/dotnet/api/system.windows.application.dispatcherunhandledexception?view=netcore-3.0
    }
}
