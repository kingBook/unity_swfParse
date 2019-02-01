using UnityEngine;
using System.Collections;
using System.Xml;

public class SwfTag{

	public TagHeaderRecord header;

	protected string getClassName(){
		var className=System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;
		return className;
	}

	protected XmlElement createXmlElement(XmlDocument doc,string elementName=null){
		if(elementName==null)elementName=getClassName();
		var ele=doc.CreateElement(elementName);
		ele.SetAttribute("type",header.type.ToString());
		ele.SetAttribute("length",header.type.ToString());
		return ele;
	}

	virtual public XmlElement toXml(XmlDocument doc){
		return createXmlElement(doc);
	}



}
