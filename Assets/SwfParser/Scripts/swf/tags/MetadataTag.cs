using System.Xml;

public class MetadataTag:SwfTag {
	public string metadata;

	public override XmlElement toXml(XmlDocument doc) {
		var ele=createXmlElement(doc,"Metadata");
		ele.SetAttribute("metadata",metadata);
		return ele;
	}

}
