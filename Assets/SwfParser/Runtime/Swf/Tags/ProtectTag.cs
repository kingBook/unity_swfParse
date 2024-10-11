using System.Xml;

[System.Serializable]
public class ProtectTag : Tag {

    public ProtectTag(SwfByteArray bytes, TagHeaderRecord header) : base(header) {
        
    }

    public override XmlElement ToXml(XmlDocument doc) {
        var ele = CreateXmlElement(doc, "Protect");
        return ele;
    }
}