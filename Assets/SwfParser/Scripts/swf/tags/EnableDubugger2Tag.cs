using System.Xml;

public class EnableDubugger2Tag:SwfTag {
	public ushort reserved;
	public string password;

	public override XmlElement toXml(XmlDocument doc) {
		var ele=createXmlElement(doc,"EnableDubugger2");
		ele.SetAttribute("reserved",reserved.ToString());
		ele.SetAttribute("password",password);
		return ele;
	}

}
