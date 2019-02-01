using System.Xml;

public class ProtectTag:SwfTag {

	public override XmlElement toXml(XmlDocument doc) {
		var ele=createXmlElement(doc,"Protect");
		return ele;
	}
}
