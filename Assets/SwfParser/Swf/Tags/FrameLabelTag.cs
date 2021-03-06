﻿using System.Xml;

public class FrameLabelTag:SwfTag {

	public string name;
	public byte namedAnchorFlag;

	public override XmlElement ToXml(XmlDocument doc) {
		var ele=CreateXmlElement(doc,"FrameLabel");
		ele.SetAttribute("name",name);
		ele.SetAttribute("namedAnchorFlag",namedAnchorFlag.ToString());
		return ele;
	}

}
