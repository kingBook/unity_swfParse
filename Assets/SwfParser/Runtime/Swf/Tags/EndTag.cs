using System.Xml;

[System.Serializable]
public class EndTag : SwfTag {

    public EndTag(SwfByteArray bytes, TagHeaderRecord header) : base(header) {

    }

    public override XmlElement ToXml(XmlDocument doc) {
        return CreateXmlElement(doc, "End");
    }

}