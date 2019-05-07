using System.Xml;

public class DefineShape3Tag:SwfTag{
	public ushort shapeId;
	public RectangleRecord shapeBounds;
	public ShapeWithStyleRecord shapes;

	public override XmlElement toXml(XmlDocument doc){
		var ele=createXmlElement(doc,"DefineShape3");
		ele.SetAttribute("shapeId",shapeId.ToString());
		ele.SetAttribute("shapeBounds",shapeBounds.ToString());
		ele.AppendChild(shapes.toXml(doc));
		return ele;
	}
	
}
