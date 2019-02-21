using System.Xml;

public class SetTabIndexTag:SwfTag {
	public ushort depth;
	public ushort tabIndex;

	public override XmlElement toXml(XmlDocument doc) {
		var ele=createXmlElement(doc,"SetTabIndex");
		ele.SetAttribute("depth",depth.ToString());
		ele.SetAttribute("tabIndex",tabIndex.ToString());
		return ele;
	}

}
