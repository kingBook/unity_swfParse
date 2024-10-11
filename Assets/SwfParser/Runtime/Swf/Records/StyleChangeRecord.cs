using System.Xml;

[System.Serializable]
public struct StyleChangeRecord : IShapeRecord { //:Shape Record

    public bool typeFlag;
    public bool stateNewStyles;
    public bool stateLineStyle;
    public bool stateFillStyle1;
    public bool stateFillStyle0;
    public bool stateMoveTo;
    public byte moveBits;
    public int moveDeltaX;
    public int moveDeltaY;
    public uint fillStyle0;
    public uint fillStyle1;
    public uint lineStyle;
    public FillStyleArrayRecord fillStyles;
    public LineStyleArrayRecord lineStyles;
    public byte numFillBits;
    public byte numLineBits;

    public StyleChangeRecord(SwfByteArray bytes, bool typeFlag, bool stateNewStyles, bool stateLineStyle, bool stateFillStyle1, bool stateFillStyle0, bool stateMoveTo, byte numFillBits, byte numLineBits, byte shapeType) {
        // default value
        moveBits = 0;
        moveDeltaX = 0;
        moveDeltaY = 0;
        fillStyle0 = 0;
        fillStyle1 = 0;
        lineStyle = 0;
        fillStyles = new FillStyleArrayRecord();
        lineStyles = new LineStyleArrayRecord();
        this.numFillBits = 0;
        this.numLineBits = 0;
        //
        this.typeFlag = typeFlag;
        this.stateNewStyles = stateNewStyles;
        this.stateLineStyle = stateLineStyle;
        this.stateFillStyle1 = stateFillStyle1;
        this.stateFillStyle0 = stateFillStyle0;
        this.stateMoveTo = stateMoveTo;
        if (stateMoveTo) {
            moveBits = (byte)bytes.ReadUB(5);
            moveDeltaX = bytes.ReadSB(moveBits);
            moveDeltaY = bytes.ReadSB(moveBits);
        }
        if (stateFillStyle0) {
            fillStyle0 = bytes.ReadUB(numFillBits);
        }
        if (stateFillStyle1) {
            fillStyle1 = bytes.ReadUB(numFillBits);
        }
        if (stateLineStyle) {
            lineStyle = bytes.ReadUB(numLineBits);
        }
        //----------------------------------
        if (stateNewStyles) {
            fillStyles = new FillStyleArrayRecord(bytes, shapeType);
            lineStyles = new LineStyleArrayRecord(bytes, shapeType);
            this.numFillBits = (byte)bytes.ReadUB(4);
            this.numLineBits = (byte)bytes.ReadUB(4);
        }
        //----------------------------------
    }

    public XmlElement ToXml(XmlDocument doc) {
        var ele = doc.CreateElement("StyleChangeRecord");
        ele.SetAttribute("typeFlag", typeFlag.ToString());
        ele.SetAttribute("stateNewStyles", stateNewStyles.ToString());
        ele.SetAttribute("stateLineStyle", stateLineStyle.ToString());
        ele.SetAttribute("stateFillStyle1", stateFillStyle1.ToString());
        ele.SetAttribute("stateFillStyle0", stateFillStyle0.ToString());
        ele.SetAttribute("stateMoveTo", stateMoveTo.ToString());
        if (stateMoveTo) {
            var stageMoveToEle = doc.CreateElement("StateMoveTo");
            stageMoveToEle.SetAttribute("moveBits", moveBits.ToString());
            stageMoveToEle.SetAttribute("moveDeltaX", moveDeltaX.ToString());
            stageMoveToEle.SetAttribute("moveDeltaY", moveDeltaY.ToString());
            ele.AppendChild(stageMoveToEle);
        }
        if (stateFillStyle0) {
            var stateFillStyle0Ele = doc.CreateElement("StateFillStyle0");
            stateFillStyle0Ele.InnerText = fillStyle0.ToString();
            ele.AppendChild(stateFillStyle0Ele);
        }
        if (stateFillStyle1) {
            var stateFillStyle1Ele = doc.CreateElement("StateFillStyle1");
            stateFillStyle1Ele.InnerText = fillStyle1.ToString();
            ele.AppendChild(stateFillStyle1Ele);
        }
        if (stateLineStyle) {
            var stateLineStyleEle = doc.CreateElement("LineStyle");
            stateLineStyleEle.InnerText = lineStyle.ToString();
            ele.AppendChild(stateLineStyleEle);
        }
        if (stateNewStyles) {
            ele.AppendChild(fillStyles.ToXml(doc));
            ele.AppendChild(lineStyles.ToXml(doc));
            //numFillBits
            var numFillBitsEle = doc.CreateElement("NumFillBits");
            numFillBitsEle.InnerText = numFillBits.ToString();
            ele.AppendChild(numFillBitsEle);
            //numLineBits
            var numLineBitsEle = doc.CreateElement("NumLineBits");
            numLineBitsEle.InnerText = numLineBits.ToString();
            ele.AppendChild(numLineBitsEle);
        }
        return ele;
    }
}