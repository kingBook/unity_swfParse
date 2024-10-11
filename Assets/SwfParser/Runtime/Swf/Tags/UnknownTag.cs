using System.Xml;

[System.Serializable]
public class UnknownTag : Tag {

    [System.NonSerialized]
    public byte[] content;

    public UnknownTag(SwfByteArray bytes, TagHeaderRecord header) : base(header) {
        if (header.length > 0) {
            content = bytes.ReadBytes((int)header.length);
        }
    }

    public override XmlElement ToXml(XmlDocument doc) {
        return CreateXmlElement(doc, "Unknown");
    }

}