using System.Xml;

[System.Serializable]
public class MetadataTag : SwfTag {

    public string metadata;

    public MetadataTag(SwfByteArray bytes, TagHeaderRecord header) : base(header) {
        metadata = bytes.ReadString();
    }

    public override XmlElement ToXml(XmlDocument doc) {
        var ele = CreateXmlElement(doc, "Metadata");
        ele.SetAttribute("metadata", metadata);
        return ele;
    }

}