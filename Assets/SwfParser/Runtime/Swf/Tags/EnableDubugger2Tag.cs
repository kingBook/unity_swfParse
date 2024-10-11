using System.Xml;

[System.Serializable]
public class EnableDubugger2Tag : Tag {

    public ushort reserved;
    public string password;

    public EnableDubugger2Tag(SwfByteArray bytes, TagHeaderRecord header) : base(header) {
        reserved = bytes.ReadUI16();
        password = bytes.ReadString();
    }

    public override XmlElement ToXml(XmlDocument doc) {
        var ele = CreateXmlElement(doc, "EnableDubugger2");
        ele.SetAttribute("reserved", reserved.ToString());
        ele.SetAttribute("password", password);
        return ele;
    }

}