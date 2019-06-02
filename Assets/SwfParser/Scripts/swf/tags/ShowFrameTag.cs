using UnityEngine;
using System.Collections;
using System.Xml;

public class ShowFrameTag:SwfTag{
	public override XmlElement toXml(XmlDocument doc) {
		return createXmlElement(doc,"ShowFrame");
	}
}
