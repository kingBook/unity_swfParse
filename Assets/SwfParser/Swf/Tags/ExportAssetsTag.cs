using UnityEngine;
using System.Collections;
using System.Xml;

public class ExportAssetsTag:SwfTag {
	public ExportAssetRecord[] list;

	public override XmlElement ToXml(XmlDocument doc) {
		var ele=CreateXmlElement(doc,"ExportAssets");
		for(int i=0;i<list.Length;i++){
			var record=list[i];
			var recordEle=CreateXmlElement(doc,"Record");
			recordEle.SetAttribute("tag",record.tag.ToString());
			recordEle.SetAttribute("name",record.name);
			ele.AppendChild(recordEle);
		}
		return ele;
	}
}
