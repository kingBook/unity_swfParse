using System.Xml;

[System.Serializable]
public struct LineStyle2Record : ILineStyleRecord {
    
    public ushort width;
    public byte startCapStyle;
    public byte joinStyle;
    public bool hasFillFlag;
    public bool noHScaleFlag;
    public bool noVScaleFlag;
    public bool pixelHintingFlag;
    public byte reserved;
    public bool noClose;
    public byte endCapStyle;
    public float miterLimitFactor;
    public RGBARecord color;
    public FillStyleRecord fillType;

    public LineStyle2Record(SwfByteArray bytes, byte shapeType) {
        // default value
        miterLimitFactor = 0;
        color = new RGBARecord();
        fillType = new FillStyleRecord();
        //
        width = bytes.ReadUI16();
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
            miterLimitFactor = bytes.ReadFixed8_8(); //bytes.readUI16();
        }

        if (!hasFillFlag) {
            color = new RGBARecord(bytes);
        } else {
            fillType = new FillStyleRecord(bytes, shapeType);
        }
    }

    public XmlElement ToXml(XmlDocument doc) {
        var ele = doc.CreateElement("LineStyle");
        ele.SetAttribute("width", width.ToString());
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
            ele.SetAttribute("color", color.ToString());
        } else {
            ele.AppendChild(fillType.ToXml(doc));
        }
        ele.SetAttribute("color", color.ToString());
        return ele;
    }
}