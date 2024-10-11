using System.Xml;

[System.Serializable]
public class ShowFrameTag : SwfTag {

    public ShowFrameTag(SwfByteArray bytes, TagHeaderRecord header) : base(header) {

    }

    public override XmlElement ToXml(XmlDocument doc) {
        return CreateXmlElement(doc, "ShowFrame");
    }

}