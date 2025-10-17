using System.Diagnostics;
using System.Text.RegularExpressions;

using Odva.Document.Sections;
using Odva.Helpers;

namespace Odva.Document;

[Serializable]
public class Document
{
    public string? Comment { get; set; }

    public Sections.File File { get; set; } = new();

    public Sections.Device Device { get; set; } = new();

    public Sections.DeviceClassification? DeviceClassification { get; set; }

    public Params Parameters { get; set; } = new();
    public Sections.Assemblies Assemblies { get; set; } = new();

    public Sections.ConnectionManager? ConnectionManager { get; set; }

    public Document()
    {
        
    }

    public Document(string filePath)
    {
        using var reader = new StreamReader(filePath);
        var documentText = reader.ReadToEnd();
        var lineReader = new LineReader(documentText);

        StringSlice sectionText;
        do
        {
            sectionText = lineReader.ReadSection();

            var section = Regex.Match(sectionText.ToString(), @"\[(.+)\]");
            Debug.WriteLine($"Section read : {section.Groups[1].Value}");
            switch (section.Groups[1].Value)
            {
                case "File":                            // Describes the contents and revision of the file
                    File.ReadString(sectionText);
                    break;
                case "Device":
                    Device.ReadString(sectionText);
                    break;
                case "Device Classification":          // Describes what network the device can be connected to. This section is optional for DeviceNet, required for ControlNet, EtherNet / IP and CompoNet
                    DeviceClassification = new Sections.DeviceClassification();
                    DeviceClassification.ReadString(sectionText);
                    break;
                case "ParamClass":                      // Describes configuration details in addition to class-level attributes of the parameter object

                    break;
                case "Params":                          // Identifies all configuration parameters in the device, follows the parameter object definition
                    Parameters.ReadString(sectionText);
                    break;

                case "Groups":                          //  Identifies all parameter groups in the device and lists group name and parameter numbers
                    break;

                case "Assembly":                        //  Describes the structure of data items
                    Assemblies.ReadString(sectionText);
                    break;
                case "Connection Manager":              // Describes connections supported by the device. Typically used in ControlNet and EtherNet/IP
                    ConnectionManager = new Sections.ConnectionManager();
                    ConnectionManager.ReadString(sectionText);
                    break;
                case "Connection ManagerN":             // Same as the [Connection Manager] section, but only for connection entries that do not apply to all CIP ports of the device
                    break;
                case "Port":                            // Describes the various network ports a device may have
                    
                    break;
                case "Capacity":                        // Specifies the communication capacity of EtherNet/IP and ControlNet devices
                    break;
                case "Connection Configuration":        // This section defines the characteristics of the connection configuration object implemented in this device, if a connection configuration object  implementation exists. It is used for EDS based I/O Scanner configuration
                case "Event Enumeration":               // The Event Enumeration section associates specific event or status codes within a device with an international string
                case "Symbolic Translation":            // This section is used to publicize the translation between a Symbolic Segment or an ANSI Extended Symbol Segment encoded EPATH specification to the equivalent ParamN or AssemN entry keywords
                case "Internationalization":            // This section allows the representation of all strings within an EDS in multiple languages
                case "Modular":                         // Describes modular structures inside a device
                case "IO_Info":                         // Describes I/O connection methods & I/O sizes. Allowed for DeviceNet only
                case "Variant_IO_Info":                 // Describes multiple IO_Info data sets. Allowed for DeviceNet only
                case "EnumPar":                         // Enumeration list of parameter choices to present to the user. This is an old enumeration meth\r\nod specified for DeviceNet only
                case "ControlNet Physical Layer":       // Describes details of the ControlNet physical layer. Allowed for ControlNet only
                case "CompoNet_Device":                 // Describes the type of CompoNet device. Allowed for CompoNet only
                case "CompoNet_IO":                     // Describes the I/O connection details of CompoNet slaves. Allowed for CompoNet only
                case "Modbus Mapper":                   // Used to provide a description of individual Modbus items that correspond to a specific CIP object attribute
                    break;
                case "TCP/IP Interface Class":
                    break;
                case "Ethernet Link Class":
                    break;
            }


        } while (!sectionText.IsEmpty);
    }
}
