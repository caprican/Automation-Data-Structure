using AutomationDataStructure.Core.Contracts.Services;

using Odva;

namespace AutomationDataStructure.Core.Services;

public class TranscriptService(IOdvaFileService odvaFileService, IPlcOpenFileService plcOpenFileService,
                               ICodesysFileService codesysFileService) : ITranscriptService
{
    private readonly IOdvaFileService odvaFileService = odvaFileService;
    private readonly IPlcOpenFileService plcOpenFileService = plcOpenFileService;
    private readonly ICodesysFileService codesysFileService = codesysFileService;

    private Device odvaDevice;

    public event EventHandler<object?>? SourceLoaded;

    public void LoadDescriptionFile(string path)
    {
        if (File.Exists(path))
        {
            switch(Path.GetExtension(path).ToLower())
            {
                case ".eds":
                    var device = Odva.Renderers.Device.FromFile(path);
                    SetSource(device);
                    break;
            }
        }
    }

    public void SetSource(Device device)
    {
        odvaDevice = device;
        SourceLoaded?.Invoke(this, new EventArgs());
    }

    public Device GetSource() => odvaDevice;

    public (string, string, string, string) ConvertToCodesys(string deviceName)
    {
        var inputStruct = $"TYPE S_{deviceName.ToUpper()}_I :\r\n\tSTRUCT\r\n";
        foreach(var item in odvaDevice.Inputs)
        {
            inputStruct += item.DataType switch
            {
                Odva.Models.DataType.BOOL => $"       i{CodesysTagNormalise(item.Name, Odva.Models.DataType.BYTE)} : {Odva.Models.DataType.BYTE};\r\n",
                _ => $"\t\ti{CodesysTagNormalise(item.Name, item.DataType)} : {item.DataType};\r\n",
            };
        }
        inputStruct += "\tEND_STRUCT\r\nEND_TYPE";

        var outputStruct = $"TYPE S_{deviceName.ToUpper()}_Q :\r\n\tSTRUCT\r\n";
        foreach(var item in odvaDevice.Outputs)
        {

            outputStruct += item.DataType switch
            {
                Odva.Models.DataType.BOOL => $"\t\tq{CodesysTagNormalise(item.Name, Odva.Models.DataType.BYTE)} : {Odva.Models.DataType.BYTE};\r\n",
                _ => $"\t\tq{CodesysTagNormalise(item.Name, item.DataType)} : {item.DataType};\r\n",
            };
        }
        outputStruct += "\tEND_STRUCT\r\nEND_TYPE";

        //var deviceStruct = "{attribute 'qualified_only'}\r\nVAR_GLOBAL\r\n";
        //deviceStruct += $"   {deviceName}_I AT %IBxx : st{deviceName}_I;\r\n";
        //deviceStruct += $"   {deviceName}_Q AT %QWxx : st{deviceName}_Q;\r\n";
        //deviceStruct += "END_VAR";


        var deviceStruct = $"TYPE S_{deviceName.ToUpper()} :\r\n\tSTRUCT\r\n\t\tsI : S_{deviceName.ToUpper()}_I;\r\n\t\tsQ : S_{deviceName.ToUpper()}_Q;\r\n\tEND_STRUCT\r\nEND_TYPE";

        var comment = $"\r\nDans GVL, ajouter \r\n{deviceName} : S_{deviceName};";
        comment += $"\r\n\r\nMappage des variables :\r\n Entrées : Application.GVL.{deviceName}.I\r\n Sorties : Application.GVL.{deviceName}.Q";
        return (deviceStruct, inputStruct, outputStruct, comment);
    }

    private string CodesysTagNormalise(string tag, Odva.Models.DataType type)
    {
        var name = tag.Trim([' ', '"']);

        switch (type)
        {
            case Odva.Models.DataType.BOOL: name = "x" + name; break;
            case Odva.Models.DataType.BYTE: name = "b" + name; break;
            case Odva.Models.DataType.WORD: name = "w" + name; break;
            case Odva.Models.DataType.INT: name = "i" + name; break;
            case Odva.Models.DataType.UINT: name = "ui" + name; break;
            case Odva.Models.DataType.UDINT: name = "udi" + name; break;
        }

        return name;
    }

    public void ExportToCodesys()
    {
        var convertResult = new PlcOpenML.Project.Project();

    }
}
