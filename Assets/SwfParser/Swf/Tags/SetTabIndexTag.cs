using System.Xml;

public class SetTabIndexTag : SwfTag {

    public ushort depth;
    public ushort tabIndex;

    public override XmlElement ToXml(XmlDocument doc) {
        var ele = CreateXmlElement(doc, "SetTabIndex");
        ele.SetAttribute("depth", depth.ToString());
        ele.SetAttribute("tabIndex", tabIndex.ToString());
        return ele;
    }

}