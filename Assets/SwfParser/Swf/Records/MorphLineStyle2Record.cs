using System.Xml;

public struct MorphLineStyle2Record : IMorphLineStyleRecord {

    public ushort startWidth;
    public ushort endWidth;
    public byte startCapStyle;
    public byte joinStyle;
    public bool hasFillFlag;
    public bool noHScaleFlag;
    public bool noVScaleFlag;
    public bool pixelHintingFlag;
    public byte reserved;
    public bool noClose;
    public byte endCapStyle;
    public ushort miterLimitFactor;
    public RGBARecord startColor;
    public RGBARecord endColor;
    public MorphFillStyleRecord fillType;

    public MorphLineStyle2Record(SwfByteArray bytes) {
        // default value
        miterLimitFactor = 0;
        startColor = new RGBARecord();
        endColor = new RGBARecord();
        fillType = new MorphFillStyleRecord();
        //
        startWidth = bytes.ReadUI16();
        endWidth = bytes.ReadUI16();
        startCapStyle = (byte)bytes.ReadUB(2);
        joinStyle = (byte)bytes.ReadUB(2);
        hasFillFlag = bytes.ReadFlag();
        noHScaleFlag = bytes.ReadFlag();
        noVScaleFlag = bytes.ReadFlag();
        pixelHintingFlag = bytes.ReadFlag();
        reserved = (byte)bytes.ReadUB(5);
        noClose = bytes.ReadFlag();
        endCapStyle = (byte)bytes.ReadUB(2);
        if (joinStyle == 2) {
            miterLimitFactor = bytes.ReadUI16();
        }
        if (!hasFillFlag) {
            startColor = new RGBARecord(bytes);
            endColor = new RGBARecord(bytes);
        } else {
            fillType = new MorphFillStyleRecord(bytes);
        }
    }

    public XmlElement ToXml(XmlDocument doc) {
        var ele = doc.CreateElement("MorphLineStyle2");
        ele.SetAttribute("startWidth", startWidth.ToString());
        ele.SetAttribute("endWidth", endWidth.ToString());
        ele.SetAttribute("startCapStyle", startCapStyle.ToString());
        ele.SetAttribute("joinStyle", joinStyle.ToString());
        ele.SetAttribute("hasFillFlag", hasFillFlag.ToString());
        ele.SetAttribute("noHScaleFlag", noHScaleFlag.ToString());
        ele.SetAttribute("noVScaleFlag", noVScaleFlag.ToString());
        ele.SetAttribute("pixelHintingFlag", pixelHintingFlag.ToString());
        ele.SetAttribute("reserved", reserved.ToString());
        ele.SetAttribute("noClose", noClose.ToString());
        ele.SetAttribute("endCapStyle", endCapStyle.ToString());
        if (joinStyle == 2) {
            ele.SetAttribute("miterLimitFactor", miterLimitFactor.ToString());
        }
        if (!hasFillFlag) {
            ele.SetAttribute("startColor", startColor.ToString());
            ele.SetAttribute("endColor", endColor.ToString());
        } else {
            ele.AppendChild(fillType.ToXml(doc));
        }
        return ele;
    }

}