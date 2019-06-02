using UnityEngine;
using System.Collections;
using System.Xml;

public class RemoveObjectTag:SwfTag{
	public ushort characterId;
	public ushort depth;

	public override XmlElement toXml(XmlDocument doc){
		var ele=createXmlElement(doc,"RemoveObject");
		ele.SetAttribute("characterId",characterId.ToString());
		ele.SetAttribute("depth",depth.ToString());
		return ele;
	}
}
