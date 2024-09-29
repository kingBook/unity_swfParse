using System.Xml;

public struct GlowFilterRecord {

    public RGBARecord glowColor;
    public float blurX;
    public float blurY;
    public float strength;
    public bool innerGlow;
    public bool knockout;
    public bool compositeSource;
    public byte passes;

    public GlowFilterRecord(SwfByteArray bytes) {
        glowColor = new RGBARecord(bytes);
        blurX = bytes.ReadFixed16_16();
        blurY = bytes.ReadFixed16_16();
        strength = bytes.ReadFixed8_8();
        innerGlow = bytes.ReadFlag();
        knockout = bytes.ReadFlag();
        compositeSource = bytes.ReadFlag();
        passes = (byte)bytes.ReadUB(5);
    }

    public XmlElement ToXml(XmlDocument doc) {
        var ele = doc.CreateElement("GlowFilter");
        ele.SetAttribute("glowColor", glowColor.ToString());
        ele.SetAttribute("blurX", blurX.ToString());
        ele.SetAttribute("blurY", blurY.ToString());
        ele.SetAttribute("strength", strength.ToString());
        ele.SetAttribute("innerGlow", innerGlow.ToString());
        ele.SetAttribute("knockout", knockout.ToString());
        ele.SetAttribute("compositeSource", compositeSource.ToString());
        ele.SetAttribute("passes", passes.ToString());
        return ele;
    }
}