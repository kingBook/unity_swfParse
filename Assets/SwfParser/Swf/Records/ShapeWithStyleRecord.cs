using System.Collections;
using System.Xml;

public struct ShapeWithStyleRecord {

    public FillStyleArrayRecord fillStyles;
    public LineStyleArrayRecord lineStyles;
    public byte numFillBits;
    public byte numLineBits;
    public IShapeRecord[] shapeRecords;

    public XmlElement ToXml(XmlDocument doc) {
        var ele = doc.CreateElement("ShapeWithStyle");
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
        //
        for (int i = 0, len = shapeRecords.Length; i < len; i++) {
            ele.AppendChild(shapeRecords[i].ToXml(doc));
        }
        return ele;
    }
}