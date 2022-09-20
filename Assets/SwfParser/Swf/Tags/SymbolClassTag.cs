using System.Xml;

public class SymbolClassTag : SwfTag {

    public ushort numSymbols;
    public SymbolClassRecord[] symbols;

    public override XmlElement ToXml(XmlDocument doc) {
        var ele = CreateXmlElement(doc, "SymbolClass");
        for (int i = 0, len = symbols.Length; i < len; i++) {
            var record = symbols[i];
            var recordEle = CreateXmlElement(doc, "Record");
            recordEle.SetAttribute("tag", record.tag.ToString());
            recordEle.SetAttribute("name", record.name);
            ele.AppendChild(recordEle);
        }
        return ele;
    }
}