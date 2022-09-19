using System.Xml;

public class MorphLineStyleRecord : IMorphLineStyleRecord {

    public ushort startWidth;
    public ushort endWidth;
    public RGBARecord startColor;
    public RGBARecord endColor;

    public XmlElement ToXml(XmlDocument doc) {
        var ele = doc.CreateElement("MorphLineStyle");
        ele.SetAttribute("startWidth", startWidth.ToString());
        ele.SetAttribute("endWidth", endWidth.ToString());
        ele.SetAttribute("startColor", startColor.ToString());
        ele.SetAttribute("endColor", endColor.ToString());
        return ele;
    }
}