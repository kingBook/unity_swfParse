using System.Xml;

[System.Serializable]
public class FrameLabelTag : SwfTag {

    public string name;
    public byte namedAnchorFlag;

    public FrameLabelTag(SwfByteArray bytes, TagHeaderRecord header) : base(header) {
        name = bytes.ReadString();
        namedAnchorFlag = bytes.ReadUI8();
    }

    public override XmlElement ToXml(XmlDocument doc) {
        var ele = CreateXmlElement(doc, "FrameLabel");
        ele.SetAttribute("name", name);
        ele.SetAttribute("namedAnchorFlag", namedAnchorFlag.ToString());
        return ele;
    }

}