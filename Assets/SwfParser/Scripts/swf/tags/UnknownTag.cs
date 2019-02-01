

using System.Xml;

public class UnknownTag:SwfTag {
	public byte[] content;

	public override XmlElement toXml(XmlDocument doc) {
		return createXmlElement(doc,"Unknown");
	}

}
