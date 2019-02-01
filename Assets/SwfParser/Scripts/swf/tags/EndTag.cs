
using System.Xml;

public class EndTag:SwfTag {

	public override XmlElement toXml(XmlDocument doc) {
		return createXmlElement(doc,"End");
	}

}
