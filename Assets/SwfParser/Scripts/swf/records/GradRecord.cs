using System.Xml;

public struct GradRecord {
	public byte ratio;
	public object color;//RGB(Shape1 or Shape2); RGBA(Shape3)

	public XmlElement toXml(XmlDocument doc){
		var ele=doc.CreateElement("GradRecord");
		ele.SetAttribute("ratio",ratio.ToString());
		ele.SetAttribute("color",color.ToString());
		return ele;
	}
	
}
