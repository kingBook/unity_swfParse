using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Text;

public class Swf {
	public SwfHeader header;
	public List<SwfTag> tags;
	
	public XmlDocument toXml(){
		var doc=new XmlDocument();
		XmlDeclaration declaration = doc.CreateXmlDeclaration("1.0","UTF-8",null);
		doc.AppendChild(declaration);

		var swfElement=doc.CreateElement("Swf");
		doc.AppendChild(swfElement);

		for(int i=0;i<tags.Count;i++){
			var tag=tags[i];
			var tagXml=tag.toXml(doc);
			swfElement.AppendChild(tagXml);
		}
		return doc;
	}
	
}
