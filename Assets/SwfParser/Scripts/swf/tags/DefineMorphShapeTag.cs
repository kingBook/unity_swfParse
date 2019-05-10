
using System.Xml;

public class DefineMorphShapeTag:SwfTag {
	public ushort characterId;
	public RectangleRecord startBounds;
	public RectangleRecord endBounds;
	public uint offset;
	public MorphFillStyleArrayRecord morphFillStyles;
	public MorphLineStyleArrayRecord morphLineStyles;
	public SHAPE startEdges;
	public SHAPE endEdges;

	public override XmlElement toXml(XmlDocument doc){
		var ele=createXmlElement(doc,"DefineMorphShape");
		ele.SetAttribute("characterId",characterId.ToString());
		ele.SetAttribute("startBounds",startBounds.ToString());
		ele.SetAttribute("endBounds",endBounds.ToString());
		ele.SetAttribute("offset",offset.ToString());
		ele.AppendChild(morphFillStyles.toXml(doc));
		ele.AppendChild(morphLineStyles.toXml(doc));
		ele.AppendChild(startEdges.toXml(doc));
		ele.AppendChild(endEdges.toXml(doc));
		return ele;
	}
}
