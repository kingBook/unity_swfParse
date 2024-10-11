using System.Xml;

[System.Serializable]
public class SetTabIndexTag : SwfTag {

    public ushort depth;
    public ushort tabIndex;

    public SetTabIndexTag(SwfByteArray bytes, TagHeaderRecord header) : base(header) {
        depth = bytes.ReadUI16();
        tabIndex = bytes.ReadUI16();
    }

    public override XmlElement ToXml(XmlDocument doc) {
        var ele = CreateXmlElement(doc, "SetTabIndex");
        ele.SetAttribute("depth", depth.ToString());
        ele.SetAttribute("tabIndex", tabIndex.ToString());
        return ele;
    }

}