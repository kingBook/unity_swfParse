using System.Xml;

public class DefineMorphShapeTag : SwfTag {

    public ushort characterId;
    public RectangleRecord startBounds;
    public RectangleRecord endBounds;
    public uint offset;
    public MorphFillStyleArrayRecord morphFillStyles;
    public MorphLineStyleArrayRecord morphLineStyles;
    public SHAPE startEdges;
    public SHAPE endEdges;

    public override XmlElement ToXml(XmlDocument doc) {
        var ele = CreateXmlElement(doc, "DefineMorphShape");
        ele.SetAttribute("characterId", characterId.ToString());
        ele.SetAttribute("startBounds", startBounds.ToString());
        ele.SetAttribute("endBounds", endBounds.ToString());
        ele.SetAttribute("offset", offset.ToString());
        ele.AppendChild(morphFillStyles.ToXml(doc));
        ele.AppendChild(morphLineStyles.ToXml(doc));
        ele.AppendChild(startEdges.ToXml(doc));
        ele.AppendChild(endEdges.ToXml(doc));
        return ele;
    }
}