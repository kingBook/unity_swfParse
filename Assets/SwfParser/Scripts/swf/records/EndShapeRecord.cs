using System.Xml;

public struct EndShapeRecord:IShapeRecord{//Shape Record
	public bool typeFlag;
	public byte endOfShape;

	public XmlElement toXml(XmlDocument doc) {
		var ele=doc.CreateElement("EndShapeRecord");
		ele.SetAttribute("typeFlag",typeFlag.ToString());
		ele.SetAttribute("endOfShape",endOfShape.ToString());
		return ele;
	}
}
