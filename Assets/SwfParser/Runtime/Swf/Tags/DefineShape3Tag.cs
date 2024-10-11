using System.Xml;

[System.Serializable]
public class DefineShape3Tag : DefineShape2Tag {

    //public ushort shapeId;
    //public RectangleRecord shapeBounds;
    //public ShapeWithStyleRecord shapes;

    public DefineShape3Tag(TagHeaderRecord header) : base(header) {
        // empty constructor
    }

    public DefineShape3Tag(SwfByteArray bytes, TagHeaderRecord header) : base(header) {
        shapeId = bytes.ReadUI16();
        shapeBounds = new RectangleRecord(bytes);
        shapes = new ShapeWithStyleRecord(bytes, 3);
    }

    public override XmlElement ToXml(XmlDocument doc) {
        var ele = CreateXmlElement(doc, "DefineShape3");
        ele.SetAttribute("shapeId", shapeId.ToString());
        ele.SetAttribute("shapeBounds", shapeBounds.ToString());
        ele.AppendChild(shapes.ToXml(doc));
        return ele;
    }

}