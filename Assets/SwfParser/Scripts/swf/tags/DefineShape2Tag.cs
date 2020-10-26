using System.Xml;

public class DefineShape2Tag:SwfTag{
	public ushort shapeId;
	public RectangleRecord shapeBounds;
	public ShapeWithStyleRecord shapes;

	public override XmlElement ToXml(XmlDocument doc) {
		var ele=CreateXmlElement(doc,"DefineShape2");
		ele.SetAttribute("shapeId",shapeId.ToString());
		ele.SetAttribute("shapeBounds",shapeBounds.ToString());
		ele.AppendChild(shapes.ToXml(doc));
		return ele;
	}

}
