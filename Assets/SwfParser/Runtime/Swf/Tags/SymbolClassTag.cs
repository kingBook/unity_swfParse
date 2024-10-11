using System.Xml;

[System.Serializable]
public class SymbolClassTag : Tag {

    public ushort numSymbols;
    public SymbolClassRecord[] symbols;

    public SymbolClassTag(SwfByteArray bytes, TagHeaderRecord header) : base(header) {
        numSymbols = bytes.ReadUI16();
        symbols = new SymbolClassRecord[numSymbols];
        for (ushort i = 0; i < numSymbols; i++) {
            symbols[i] = new SymbolClassRecord(bytes);
        }
    }

    public override XmlElement ToXml(XmlDocument doc) {
        var ele = CreateXmlElement(doc, "SymbolClass");
        for (int i = 0, len = symbols.Length; i < len; i++) {
            var record = symbols[i];
            var recordEle = CreateXmlElement(doc, "Record");
            recordEle.SetAttribute("tag", record.tagId.ToString());
            recordEle.SetAttribute("name", record.name);
            ele.AppendChild(recordEle);
        }
        return ele;
    }

}