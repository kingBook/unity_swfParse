using System.Xml;

public class SetBackgroundColorTag : SwfTag {

    public RGBRecord backgroundColor;

    public SetBackgroundColorTag(SwfByteArray bytes, TagHeaderRecord header) : base(header) {
        backgroundColor = new RGBRecord(bytes);
    }

    public override XmlElement ToXml(XmlDocument doc) {
        var ele = CreateXmlElement(doc, "SetBackgroundColor");
        ele.SetAttribute("backgroundColor", backgroundColor.ToString());
        return ele;
    }
}