using System.Xml;

[System.Serializable]
public struct DropShadowFilterRecord {

    public RGBARecord dropShadowColor;
    public float blurX;
    public float blurY;
    public float angle;
    public float distance;
    public float strength;
    public bool innerShadow;
    public bool knockout;
    public bool compositeSource;
    public byte passes;

    public DropShadowFilterRecord(SwfByteArray bytes) {
        dropShadowColor = new RGBARecord(bytes);
        blurX = bytes.ReadFixed16_16();
        blurY = bytes.ReadFixed16_16();
        angle = bytes.ReadFixed16_16();
        distance = bytes.ReadFixed16_16();
        strength = bytes.ReadFixed8_8();
        innerShadow = bytes.ReadFlag();
        knockout = bytes.ReadFlag();
        compositeSource = bytes.ReadFlag();
        passes = (byte)bytes.ReadUB(5);
    }

    public XmlElement ToXml(XmlDocument doc) {
        var ele = doc.CreateElement("DropShadowFilter");
        ele.SetAttribute("dropShadowColor", dropShadowColor.ToString());
        ele.SetAttribute("blurX", blurX.ToString());
        ele.SetAttribute("blurY", blurY.ToString());
        ele.SetAttribute("angle", angle.ToString());
        ele.SetAttribute("distance", distance.ToString());
        ele.SetAttribute("strength", strength.ToString());
        ele.SetAttribute("innerShadow", innerShadow.ToString());
        ele.SetAttribute("knockout", knockout.ToString());
        ele.SetAttribute("compositeSource", compositeSource.ToString());
        ele.SetAttribute("passes", passes.ToString());
        return ele;
    }

}