using System.Xml;

public struct GradRecord {

    public byte ratio;
    public object color; //RGB(Shape1 or Shape2); RGBA(Shape3)

    public GradRecord(SwfByteArray bytes, byte shapeType) {
        ratio = bytes.ReadUI8();
        if (shapeType == 1 || shapeType == 2) { //RGB(Shape1 or Shape2)
            color = new RGBRecord(bytes);
        } else { //RGBA(Shape3,4)
            color = new RGBARecord(bytes);
        }
    }

    public XmlElement ToXml(XmlDocument doc) {
        var ele = doc.CreateElement("GradRecord");
        ele.SetAttribute("ratio", ratio.ToString());
        ele.SetAttribute("color", color.ToString());
        return ele;
    }

}