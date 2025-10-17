using System.Xml;

namespace AutomationDataStructure.Helpers;

internal class NamespaceIgnorantXmlTextReader : XmlTextReader
{
    public NamespaceIgnorantXmlTextReader(System.IO.TextReader reader) : base(reader) { }

    public override string NamespaceURI
    {
        get { return ""; }
    }
}
