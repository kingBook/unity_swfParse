
using System.Xml;

public class EndTag:SwfTag {

	public override XmlElement ToXml(XmlDocument doc) {
		return CreateXmlElement(doc,"End");
	}

}
