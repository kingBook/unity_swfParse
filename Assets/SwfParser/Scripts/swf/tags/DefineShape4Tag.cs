using System.Xml;

public class DefineShape4Tag:SwfTag{
	public ushort shapeId;
	public RectangleRecord shapeBounds;
	public RectangleRecord edgeBounds;
	public byte reserved;
	public bool usesFillWindingRule;
	public bool usesNonScalingStrokes;
	public bool usesScalingStrokes;
	public ShapeWithStyleRecord shapes;

	public override XmlElement toXml(XmlDocument doc) {
		var ele=doc.CreateElement("DefineShape4");
		ele.SetAttribute("shapeId",shapeId.ToString());
		ele.SetAttribute("shapeBounds",shapeBounds.ToString());
		ele.SetAttribute("edgeBounds",edgeBounds.ToString());
		ele.SetAttribute("reserved",reserved.ToString());
		ele.SetAttribute("usesFillWindingRule",usesFillWindingRule.ToString());
		ele.SetAttribute("usesNonScalingStrokes",usesNonScalingStrokes.ToString());
		ele.SetAttribute("usesScalingStrokes",usesScalingStrokes.ToString());
		ele.AppendChild(shapes.toXml(doc));
		return ele;
	}

}
