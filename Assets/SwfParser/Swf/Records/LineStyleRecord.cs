using System.Xml;

public struct LineStyleRecord : ILineStyleRecord {

    public ushort width;
    public object color; //RGB(Shape1 or Shape2); RGBA(Shape3)

    public LineStyleRecord(SwfByteArray bytes, byte shapeType) {
        width = bytes.ReadUI16();
        if (shapeType == 1 || shapeType == 2) { //RGB(Shape1 or Shape2)
            color = new RGBRecord(bytes);
        } else { //RGBA(Shape3)
            color = new RGBARecord(bytes);
        }
    }

    public XmlElement ToXml(XmlDocument doc) {
        var ele = doc.CreateElement("LineStyle");
        ele.SetAttribute("width", width.ToString());
        ele.SetAttribute("color", color.ToString());
        return ele;
    }
}