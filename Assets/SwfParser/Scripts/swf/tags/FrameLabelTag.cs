using System.Xml;

public class FrameLabelTag:SwfTag {

	public string name;
	public byte namedAnchorFlag;

	public override XmlElement toXml(XmlDocument doc) {
		var ele=createXmlElement(doc,"FrameLabel");
		ele.SetAttribute("name",name);
		ele.SetAttribute("namedAnchorFlag",namedAnchorFlag.ToString());
		return base.toXml(doc);
	}

}
