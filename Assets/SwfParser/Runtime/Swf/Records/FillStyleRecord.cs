using System.Diagnostics;
using System.Xml;

[System.Serializable]
public struct FillStyleRecord {

    public byte fillStyleType;
    public object color; //RGB|RGBA
    public MatrixRecord gradientMatrix;
    public object gradient;
    public ushort bitmapId;
    public MatrixRecord bitmapMatrix;

    public FillStyleRecord(SwfByteArray bytes, byte shapeType) {
        // default value
        color = null;
        gradientMatrix = new MatrixRecord();
        gradient = null;
        bitmapId = 0;
        bitmapMatrix = new MatrixRecord();
        //
        byte type = bytes.ReadUI8();
        fillStyleType = type;

        if (type == 0x00) {
            if (shapeType == 3 || shapeType == 4) {
                color = new RGBARecord(bytes);
            } else if (shapeType == 1 || shapeType == 2) {
                color = new RGBRecord(bytes);
            }
        }
        if (type == 0x10 || type == 0x12 || type == 0x13) {
            gradientMatrix = new MatrixRecord(bytes);
        }

        if (type == 0x10 || type == 0x12) {
            gradient = new GradientRecord(bytes, shapeType);
        } else if (type == 0x13) {
            gradient = new FocalGradientRecord(bytes, shapeType);
        }
        if (type == 0x40 || type == 0x41 || type == 0x42 || type == 0x43) {
            bitmapId = bytes.ReadUI16();
            bitmapMatrix = new MatrixRecord(bytes);
        }
    }

    public XmlElement ToXml(XmlDocument doc) {
        var ele = doc.CreateElement("FillStyle");
        //fillStyleType
        var fillStyleTypeEle = doc.CreateElement("FillStyleType");
        fillStyleTypeEle.InnerText = fillStyleType.ToString();
        ele.AppendChild(fillStyleTypeEle);
        //color
        if (fillStyleType == 0x00) {
            var colorEle = doc.CreateElement("Color");
            colorEle.InnerText = color.ToString();
            ele.AppendChild(colorEle);
        }
        //gradientMatrix
        if (fillStyleType == 0x10 || fillStyleType == 0x12 || fillStyleType == 0x13) {
            var gradientMatrixEle = doc.CreateElement("GradientMatrix");
            gradientMatrixEle.InnerText = gradientMatrix.ToString();
            ele.AppendChild(gradientMatrixEle);
        }
        //gradient
        if (fillStyleType == 0x10 || fillStyleType == 0x12) {
            ele.AppendChild(((GradientRecord)gradient).ToXml(doc));
        } else if (fillStyleType == 0x13) {
            ele.AppendChild(((FocalGradientRecord)gradient).ToXml(doc));
        }
        if (fillStyleType == 0x40 || fillStyleType == 0x41 || fillStyleType == 0x42 || fillStyleType == 0x43) {
            //bitmapId
            var bitmapIdEle = doc.CreateElement("BitmapId");
            bitmapIdEle.InnerText = bitmapId.ToString();
            ele.AppendChild(bitmapIdEle);
            //bitmapMatrix
            var bitmapMatrixEle = doc.CreateElement("BitmapMatrix");
            bitmapMatrixEle.InnerText = bitmapMatrix.ToString();
            ele.AppendChild(bitmapMatrixEle);
        }
        return ele;
    }

}