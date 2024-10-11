using System.Xml;

[System.Serializable]
public struct MorphFillStyleRecord {

    public byte fillStyleType;
    public RGBARecord startColor;
    public RGBARecord endColor;
    public MatrixRecord startGradientMatrix;
    public MatrixRecord endGradientMatrix;
    public MorphGradientRecord gradient;
    public ushort bitmapId;
    public MatrixRecord startBitmapMatrix;
    public MatrixRecord endBitmapMatrix;

    public MorphFillStyleRecord(SwfByteArray bytes) {
        // default value
        startColor = new RGBARecord();
        endColor = new RGBARecord();
        startGradientMatrix = new MatrixRecord();
        endGradientMatrix = new MatrixRecord();
        gradient = new MorphGradientRecord();
        bitmapId = 0;
        startBitmapMatrix = new MatrixRecord();
        endBitmapMatrix = new MatrixRecord();
        //
        fillStyleType = bytes.ReadUI8();

        var type = fillStyleType;
        if (type == 0x00) {
            startColor = new RGBARecord(bytes);
            endColor = new RGBARecord(bytes);
        } else if (type == 0x10 || type == 0x12) {
            startGradientMatrix = new MatrixRecord(bytes);
            endGradientMatrix = new MatrixRecord(bytes);
            gradient = new MorphGradientRecord(bytes);
        } else if (type == 0x40 || type == 0x41 || type == 0x42 || type == 0x43) {
            bitmapId = bytes.ReadUI16();
            startBitmapMatrix = new MatrixRecord(bytes);
            endBitmapMatrix = new MatrixRecord(bytes);
        }
    }

    public XmlElement ToXml(XmlDocument doc) {
        var type = fillStyleType;
        var ele = doc.CreateElement("MorphFillStyle");
        ele.SetAttribute("fillStyleType", fillStyleType.ToString());
        if (type == 0x00) {
            ele.SetAttribute("startColor", startColor.ToString());
            ele.SetAttribute("endColor", endColor.ToString());
        } else if (type == 0x10 || type == 0x12) {
            ele.SetAttribute("startGradientMatrix", startGradientMatrix.ToString());
            ele.SetAttribute("endGradientMatrix", endGradientMatrix.ToString());
            ele.AppendChild(gradient.ToXml(doc));
        } else if (type == 0x40 || type == 0x41 || type == 0x42 || type == 0x43) {
            ele.SetAttribute("bitmapId", bitmapId.ToString());
            ele.SetAttribute("startBitmapMatrix", startBitmapMatrix.ToString());
            ele.SetAttribute("endBitmapMatrix", endBitmapMatrix.ToString());
        }
        return ele;
    }

}