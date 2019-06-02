
using System.Xml;

public struct FilterListRecord{
	public byte numberOfFilters;
	public FilterRecord[] filters;

	public XmlElement toXml(XmlDocument doc){
		var ele=doc.CreateElement("FilterList");
		for(var i=0;i<numberOfFilters;i++){
			ele.AppendChild(filters[i].toXml(doc));
		}
		return ele;
	}
}
