using System.Xml;

public class ProtectTag : SwfTag {

    public override XmlElement ToXml(XmlDocument doc) {
        var ele = CreateXmlElement(doc, "Protect");
        return ele;
    }
}