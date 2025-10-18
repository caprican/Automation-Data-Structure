using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Xml;
using System.Xml.Serialization;

using AutomationDataStructure.Contracts.ViewModels;
using AutomationDataStructure.Core.Contracts.Services;
using AutomationDataStructure.Core.Services;
using AutomationDataStructure.Helpers;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using MahApps.Metro.Controls.Dialogs;

namespace AutomationDataStructure.ViewModels;

public class MainViewModel(Contracts.Services.ISettingsService settingsService, IDialogCoordinator dialogCoordinator,
                          ITranscriptService transcriptService) : ObservableObject, INavigationAware
{
    private readonly Contracts.Services.ISettingsService settingsService = settingsService;
    private readonly IDialogCoordinator dialogCoordinator = dialogCoordinator;

    private readonly ITranscriptService transcriptService = transcriptService;

    private ICommand? convertCommand;
    private ICommand? copyCommand;
    private ICommand? exportCommand;

    private Odva.Device? deviceSource;
    private Visibility convertResultView = Visibility.Collapsed;
    private string? convertInputResult;
    private string? convertOutputResult;
    private string? convertGvlResult;
    private string? convertComment;

    public Odva.Device? DeviceSource 
    {
        get => deviceSource;
        set => SetProperty(ref deviceSource, value);
    }

    public Visibility ConvertResultView
    {
        get => convertResultView;
        set => SetProperty(ref convertResultView, value);
    }

    public string? ConvertInputResult
    {
        get => convertInputResult;
        set => SetProperty(ref convertInputResult, value);
    }

    public string? ConvertOutputResult
    {
        get => convertOutputResult;
        set => SetProperty(ref convertOutputResult, value);
    }

    public string? ConvertGvlResult
    {
        get => convertGvlResult;
        set => SetProperty(ref convertGvlResult, value);
    }

    public string? ConvertComment
    {
        get => convertComment;
        set => SetProperty(ref convertComment, value);
    }

    public ICommand ConvertCommand => convertCommand ??= new AsyncRelayCommand(OnConvertAsync);
    public ICommand CopyCommand => copyCommand ??= new RelayCommand<string?>(CopyToClipboard);
    public ICommand ExportCommand => exportCommand ??= new AsyncRelayCommand(OnExportAsync);

    public void OnNavigatedFrom()
    {
        transcriptService.SourceLoaded -= TranscriptService_SourceLoaded;

    }

    public void OnNavigatedTo(object parameter)
    {
        DeviceSource = transcriptService.GetSource();

        transcriptService.SourceLoaded += TranscriptService_SourceLoaded;
        ConvertResultView = Visibility.Collapsed;
    }

    private void TranscriptService_SourceLoaded(object? sender, object? e)
    {
        DeviceSource = transcriptService.GetSource();
        ConvertResultView = Visibility.Collapsed;

    }

    private async Task OnConvertAsync()
    {
        var name = await dialogCoordinator.ShowInputAsync(App.Current.MainWindow.DataContext, "Device name", "");

        (ConvertGvlResult, ConvertInputResult, ConvertOutputResult, ConvertComment) = transcriptService.ConvertToCodesys(name);

        ConvertResultView = Visibility.Visible;
    }

    private PlcOpenML.Project.Project? LoadPlcOpenFile()
    {
        var path = @"C:\Users\capri\Desktop\GVL.xml";
        if (File.Exists(path))
        {
            
        }
        return null;
    }

    private void CopyToClipboard(string? text)
    {
        Clipboard.SetText(text);
    }

    private async Task OnExportAsync()
    {
        //var project = LoadPlcOpenFile();

        //if (project?.Datas?.Count > 0)
        //{
        //    foreach (var data in project.Datas)
        //    {
        //        switch(data.Name)
        //        {
        //            case "http://www.3s-software.com/plcopenxml/globalvars":

        //                var serialize = new XmlSerializer(typeof(PlcOpenML.Project.Type.Pou.Interface.GlobalVars), new XmlRootAttribute
        //                {
        //                    ElementName = "globalVars",
        //                    Namespace = project.Namespace
        //                });

        //                var globalVars = serialize.Deserialize(new StringReader(data.Any.OuterXml));

        //                break;
        //        }
        //    }
        //}


        //writePlcOpenFile(project);



    }
}