using System.Xml;

public struct LineStyleRecord{
	public ushort width;
	public object color;//RGB(Shape1 or Shape2); RGBA(Shape3)

	public XmlElement toXml(XmlDocument doc){
		var ele=doc.CreateElement("LineStyle");
		ele.SetAttribute("width",width.ToString());
		ele.SetAttribute("color",color.ToString());
		return ele;
	}
}
