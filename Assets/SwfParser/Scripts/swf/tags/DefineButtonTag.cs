using UnityEngine;
using System.Collections;
using System.Xml;

public class DefineButtonTag:SwfTag{
	public ushort buttonId;
	public ButtonRecord[] characters;
	public byte characterEndFlag;
	//public actions;
	//public actionEndFlag;

	public override XmlElement toXml(XmlDocument doc) {
		return base.toXml(doc);
	}

}
