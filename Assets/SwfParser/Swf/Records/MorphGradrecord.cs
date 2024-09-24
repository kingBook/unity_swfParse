using System.Xml;

public struct MorphGradRecord {

    public byte startRatio;
    public RGBARecord startColor;
    public byte endRatio;
    public RGBARecord endColor;

    public MorphGradRecord(SwfByteArray bytes) {
        startRatio = bytes.ReadUI8();
        startColor = new RGBARecord(bytes);
        endRatio = bytes.ReadUI8();
        endColor = new RGBARecord(bytes);
    }

    public XmlElement ToXml(XmlDocument doc) {
        var ele = doc.CreateElement("MorphGradRecord");
        ele.SetAttribute("startRatio", startRatio.ToString());
        ele.SetAttribute("startColor", startColor.ToString());
        ele.SetAttribute("endRatio", endRatio.ToString());
        ele.SetAttribute("endColor", endColor.ToString());
        return ele;
    }
}