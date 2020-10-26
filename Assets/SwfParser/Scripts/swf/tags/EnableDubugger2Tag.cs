using System.Xml;

public class EnableDubugger2Tag:SwfTag {
	public ushort reserved;
	public string password;

	public override XmlElement ToXml(XmlDocument doc) {
		var ele=CreateXmlElement(doc,"EnableDubugger2");
		ele.SetAttribute("reserved",reserved.ToString());
		ele.SetAttribute("password",password);
		return ele;
	}

}
