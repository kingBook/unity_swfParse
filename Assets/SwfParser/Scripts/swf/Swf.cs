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
		var sw=new System.Diagnostics.Stopwatch();
		for(int i=0;i<tags.Count;i++){
			var tag=tags[i];

			sw.Restart();
			var tagXml=tag.toXml(doc);
			sw.Stop();
			Debug.LogFormat("type:{0},time:{1}",tag.header.type,sw.ElapsedMilliseconds);
			

			swfElement.AppendChild(tagXml);
		}
		return doc;
	}
	
}
