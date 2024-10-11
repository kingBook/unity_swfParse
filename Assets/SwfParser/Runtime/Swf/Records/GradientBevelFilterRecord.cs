using System.Xml;

[System.Serializable]
public struct GradientBevelFilterRecord {

    public byte numColors;
    public RGBARecord[] gradientColors;
    public byte[] gradientRatio;
    public float blurX;
    public float blurY;
    public float angle;
    public float distance;
    public float strength;
    public bool innerShadow;
    public bool knockout;
    public bool compositeSource;
    public bool onTop;
    public byte passes;

    public GradientBevelFilterRecord(SwfByteArray bytes) {
        numColors = bytes.ReadUI8();
        gradientColors = new RGBARecord[numColors];
        gradientRatio = new byte[numColors];
        for (var i = 0; i < numColors; i++) {
            gradientColors[i] = new RGBARecord(bytes);
            gradientRatio[i] = bytes.ReadUI8();
        }
        blurX = bytes.ReadFixed16_16();
        blurY = bytes.ReadFixed16_16();
        angle = bytes.ReadFixed16_16();
        distance = bytes.ReadFixed16_16();
        strength = bytes.ReadFixed8_8();
        innerShadow = bytes.ReadFlag();
        knockout = bytes.ReadFlag();
        compositeSource = bytes.ReadFlag();
        onTop = bytes.ReadFlag();
        passes = (byte)bytes.ReadUB(4);
    }

    public XmlElement ToXml(XmlDocument doc) {
        var ele = doc.CreateElement("GradientBevelFilter");
        ele.SetAttribute("numColors", numColors.ToString());

        var gradientColorsStr = "";
        var gradientRatioStr = "";
        var maxId = numColors - 1;
        for (byte i = 0; i < numColors; i++) {
            gradientColorsStr += gradientColors[i].ToString();
            gradientRatioStr += gradientRatio[i].ToString();
            if (i < maxId) {
                gradientColorsStr += ",";
                gradientRatioStr += ",";
            }
        }
        ele.SetAttribute("gradientColors", gradientColorsStr);
        ele.SetAttribute("gradientRatio", gradientRatioStr);
        ele.SetAttribute("blurX", blurX.ToString());
        ele.SetAttribute("blurY", blurY.ToString());
        ele.SetAttribute("angle", angle.ToString());
        ele.SetAttribute("distance", distance.ToString());
        ele.SetAttribute("strength", strength.ToString());
        ele.SetAttribute("innerShadow", innerShadow.ToString());
        ele.SetAttribute("knockout", knockout.ToString());
        ele.SetAttribute("compositeSource", compositeSource.ToString());
        ele.SetAttribute("onTop", onTop.ToString());
        ele.SetAttribute("passes", passes.ToString());
        return ele;
    }
}