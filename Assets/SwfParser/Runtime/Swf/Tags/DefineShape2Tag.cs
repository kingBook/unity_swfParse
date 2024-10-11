using System.Xml;

[System.Serializable]
public class DefineShape2Tag : DefineShapeTag {

    //public ushort shapeId;
    //public RectangleRecord shapeBounds;
    //public ShapeWithStyleRecord shapes;

    public DefineShape2Tag(TagHeaderRecord header) : base(header) {
        // empty constructor
    }

    public DefineShape2Tag(SwfByteArray bytes, TagHeaderRecord header) : base(header) {
        shapeId = bytes.ReadUI16();
        shapeBounds = new RectangleRecord(bytes);
        shapes = new ShapeWithStyleRecord(bytes, 2);
    }

    public override XmlElement ToXml(XmlDocument doc) {
        var ele = CreateXmlElement(doc, "DefineShape2");
        ele.SetAttribute("shapeId", shapeId.ToString());
        ele.SetAttribute("shapeBounds", shapeBounds.ToString());
        ele.AppendChild(shapes.ToXml(doc));
        return ele;
    }

}