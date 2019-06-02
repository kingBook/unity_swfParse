using UnityEngine;
using System.Collections;
using System.Xml;

public class RemoveObject2Tag:SwfTag{
	public ushort depth;

	public override XmlElement toXml(XmlDocument doc){
		var ele=createXmlElement(doc,"RemoveObject2");
		ele.SetAttribute("depth",depth.ToString());
		return ele;
	}
	
}
