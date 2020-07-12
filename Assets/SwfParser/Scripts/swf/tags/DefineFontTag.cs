﻿using UnityEngine;
using System.Collections;
using System.Xml;
using System.Text;

public class DefineFontTag:SwfTag{
	public ushort fontID;
	public ushort[] offsetTable;
	public SHAPE[] glyphShapeTable;

	public override XmlElement toXml(XmlDocument doc){
		var ele=createXmlElement(doc,"DefineFont");
		ele.SetAttribute("fontID",fontID.ToString());

		var len=offsetTable.Length;
		var maxId=len-1;
		var strBuilder=new StringBuilder();
		for(var i=0;i<len;i++){
			strBuilder.Append(offsetTable[i].ToString());
			if(i<maxId)strBuilder.Append(',');
		}
		ele.SetAttribute("offsetTable",strBuilder.ToString());

		len=glyphShapeTable.Length;
		maxId=len-1;
		strBuilder.Clear();
		for(var i=0;i<len;i++){
			ele.AppendChild(glyphShapeTable[i].toXml(doc));
		}
		return ele;
	}
}