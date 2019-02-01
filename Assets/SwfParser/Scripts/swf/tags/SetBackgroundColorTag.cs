using UnityEngine;
using System.Collections;
using System.Xml;

public class SetBackgroundColorTag:SwfTag {
	public RGBRecord backgroundColor;

	public override XmlElement toXml(XmlDocument doc) {
		var ele=createXmlElement(doc,"SetBackgroundColor");
		ele.SetAttribute("backgroundColor",backgroundColor.ToString());
		return ele;
	}
}
