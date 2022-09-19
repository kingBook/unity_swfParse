using System.Xml;

public class DefineScalingGridTag : SwfTag {

    public ushort characterId;
    public RectangleRecord splitter;

    public override XmlElement ToXml(XmlDocument doc) {
        var ele = CreateXmlElement(doc, "DefineScalingGrid");
        ele.SetAttribute("characterId", characterId.ToString());
        ele.SetAttribute("splitter", splitter.ToString());
        return ele;
    }
}