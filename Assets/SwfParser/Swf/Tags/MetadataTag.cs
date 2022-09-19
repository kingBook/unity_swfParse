using System.Xml;

public class MetadataTag : SwfTag {

    public string metadata;

    public override XmlElement ToXml(XmlDocument doc) {
        var ele = CreateXmlElement(doc, "Metadata");
        ele.SetAttribute("metadata", metadata);
        return ele;
    }

}