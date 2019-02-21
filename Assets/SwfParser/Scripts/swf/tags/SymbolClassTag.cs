using System.Xml;

public class SymbolClassTag:SwfTag {
	public ushort numSymbols;
	public SymbolClassRecord[] list;

	public override XmlElement toXml(XmlDocument doc) {
		var ele=createXmlElement(doc,"SymbolClass");
		for(int i=0;i<list.Length;i++){
			var record=list[i];
			var recordEle=createXmlElement(doc,"Record");
			recordEle.SetAttribute("tag",record.tag.ToString());
			recordEle.SetAttribute("name",record.name);
			ele.AppendChild(recordEle);
		}
		return ele;
	}
}
