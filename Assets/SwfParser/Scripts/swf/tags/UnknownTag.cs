

using System.Xml;

public class UnknownTag:SwfTag {
	public byte[] content;

	public override XmlElement ToXml(XmlDocument doc) {
		return CreateXmlElement(doc,"Unknown");
	}

}
