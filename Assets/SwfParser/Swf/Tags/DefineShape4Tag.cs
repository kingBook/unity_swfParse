using System.Xml;

public class DefineShape4Tag : DefineShape3Tag {

    //public ushort shapeId;
    //public RectangleRecord shapeBounds;
    public RectangleRecord edgeBounds;
    public byte reserved;
    public bool usesFillWindingRule;
    public bool usesNonScalingStrokes;
    public bool usesScalingStrokes;
    //public ShapeWithStyleRecord shapes;

    public DefineShape4Tag(SwfReader swfReader, SwfByteArray bytes, TagHeaderRecord header) : base(header) {
        shapeId = bytes.ReadUI16();
        shapeBounds = new RectangleRecord(bytes);
        edgeBounds = new RectangleRecord(bytes);
        reserved = (byte)bytes.ReadUB(5);
        usesFillWindingRule = bytes.ReadFlag();
        usesNonScalingStrokes = bytes.ReadFlag();
        usesScalingStrokes = bytes.ReadFlag();
        shapes = new ShapeWithStyleRecord(swfReader, bytes, 4);
    }

    public override XmlElement ToXml(XmlDocument doc) {
        var ele = doc.CreateElement("DefineShape4");
        ele.SetAttribute("shapeId", shapeId.ToString());
        ele.SetAttribute("shapeBounds", shapeBounds.ToString());
        ele.SetAttribute("edgeBounds", edgeBounds.ToString());
        ele.SetAttribute("reserved", reserved.ToString());
        ele.SetAttribute("usesFillWindingRule", usesFillWindingRule.ToString());
        ele.SetAttribute("usesNonScalingStrokes", usesNonScalingStrokes.ToString());
        ele.SetAttribute("usesScalingStrokes", usesScalingStrokes.ToString());
        ele.AppendChild(shapes.ToXml(doc));
        return ele;
    }

}