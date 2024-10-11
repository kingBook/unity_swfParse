using System.Xml;

[System.Serializable]
public class EndTag : Tag {

    public EndTag(SwfByteArray bytes, TagHeaderRecord header) : base(header) {

    }

    public override XmlElement ToXml(XmlDocument doc) {
        return CreateXmlElement(doc, "End");
    }

}