using System.Xml;

public class MorphLineStyleRecord : IMorphLineStyleRecord {

    public ushort startWidth;
    public ushort endWidth;
    public RGBARecord startColor;
    public RGBARecord endColor;

    public MorphLineStyleRecord(SwfByteArray bytes) {
        startWidth = bytes.ReadUI16();
        endWidth = bytes.ReadUI16();
        startColor = new RGBARecord(bytes);
        endColor = new RGBARecord(bytes);
    }

    public XmlElement ToXml(XmlDocument doc) {
        var ele = doc.CreateElement("MorphLineStyle");
        ele.SetAttribute("startWidth", startWidth.ToString());
        ele.SetAttribute("endWidth", endWidth.ToString());
        ele.SetAttribute("startColor", startColor.ToString());
        ele.SetAttribute("endColor", endColor.ToString());
        return ele;
    }
}