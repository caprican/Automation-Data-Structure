using System.Globalization;
using System.Windows.Input;

using AutomationDataStructure.Contracts.Services;
using AutomationDataStructure.Contracts.ViewModels;
using AutomationDataStructure.Models;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.Extensions.Options;

namespace AutomationDataStructure.ViewModels;

public class SettingsViewModel(IOptions<Core.Models.AppConfig> appConfig, ISettingsService settingsService,
    IThemeSelectorService themeSelectorService, IApplicationInfoService applicationInfoService) : ObservableObject, INavigationAware
{
    private readonly Core.Models.AppConfig appConfig = appConfig.Value;
    private readonly ISettingsService settingsService = settingsService;
    private readonly IThemeSelectorService themeSelectorService = themeSelectorService;

    private readonly IApplicationInfoService applicationInfoService = applicationInfoService;
    private AppTheme theme;
    private string? versionDescription;

    private LanguageItem? language;
    private ICommand? setThemeCommand;
    private ICommand? privacyStatementCommand;
    private ICommand? gotoFolderCommand;

    private readonly string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

    public AppTheme Theme
    {
        get => theme;
        set { SetProperty(ref theme, value); }
    }

    public string? VersionDescription
    {
        get => versionDescription;
        set { SetProperty(ref versionDescription, value); }
    }

    public LanguageItem[] Languages
    {
        get =>
        [
            new LanguageItem{ Name = "English", Culture = "en-US" },
            //new LanguageItem{ Name = "Deutsch", Culture = "de-DE" },
            new LanguageItem{ Name = "Français", Culture = "fr-FR" },
            //new LanguageItem{ Name = "Español", Culture = "es-ES" },
            //new LanguageItem{ Name = "Italiano", Culture = "it-IT" },
            //new LanguageItem{ Name = "Português", Culture = "de-DE" },
            //new LanguageItem{ Name = "рyсский", Culture = "de-DE" },
            //new LanguageItem{ Name = "日本語", Culture = "de-DE" },
            //new LanguageItem{ Name = "한국어", Culture = "de-DE" },
        ];
    }

    public LanguageItem? Language
    {
        get => language;
        set
        {
            SetProperty(ref language, value);
            var info = CultureInfo.CreateSpecificCulture(Language?.Culture ?? "en-US");
            App.Current.Properties[nameof(settingsService.CurrentCulture)] = info.Name;
            Thread.CurrentThread.CurrentCulture = info;
            Thread.CurrentThread.CurrentUICulture = info;

            //App.ChangeCulture(language.Culture);
        }
    }

    public ICommand SetThemeCommand => setThemeCommand ??= new RelayCommand<string>(OnSetTheme);

    public ICommand PrivacyStatementCommand => privacyStatementCommand ??= new RelayCommand(OnPrivacyStatement);

    public ICommand GotoFolderCommand => gotoFolderCommand ??= new RelayCommand<string>(OnGotoFolder);

    public void OnNavigatedTo(object parameter)
    {
        VersionDescription = $"{Properties.Resources.AppDisplayName} - {applicationInfoService.GetVersion()}";
        Theme = themeSelectorService.GetCurrentTheme();
        language = Languages.SingleOrDefault(s => s.Culture == settingsService.CurrentCulture);
    }

    public void OnNavigatedFrom()
    {
    }

    private void OnSetTheme(string? themeName)
    {
        if (!string.IsNullOrEmpty(themeName))
        {
            var theme = (AppTheme)Enum.Parse(typeof(AppTheme), themeName);
            themeSelectorService.SetTheme(theme);
        }
    }

    private void OnPrivacyStatement() { }// => systemService.OpenInWebBrowser(appConfig.PrivacyStatement);

    private void OnGotoFolder(string? path)
    {
        System.Diagnostics.Process.Start("explorer.exe", @$"""{path}""");
    }
}