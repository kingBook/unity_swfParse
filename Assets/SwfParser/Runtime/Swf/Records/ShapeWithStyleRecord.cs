using System.Collections.Generic;
using System.Xml;

[System.Serializable]
public struct ShapeWithStyleRecord {

    public FillStyleArrayRecord fillStyles;
    public LineStyleArrayRecord lineStyles;
    public byte numFillBits;
    public byte numLineBits;
    public IShapeRecord[] shapeRecords;

    public ShapeWithStyleRecord(SwfByteArray bytes, byte shapeType) {
        fillStyles = new FillStyleArrayRecord(bytes, shapeType);
        lineStyles = new LineStyleArrayRecord(bytes, shapeType);
        bytes.AlignBytes();
        numFillBits = (byte)bytes.ReadUB(4);
        numLineBits = (byte)bytes.ReadUB(4);
        var list = new List<IShapeRecord>();
        while (true) {
            var shapeRecord = ShapeRecordReader.ReadShapeRecord(bytes, numFillBits, numLineBits, shapeType);
            list.Add(shapeRecord);
            if (shapeRecord is StyleChangeRecord changeRecord) {
                if (changeRecord.stateNewStyles) {
                    numFillBits = changeRecord.numFillBits;
                    numLineBits = changeRecord.numLineBits;
                }
            }
            if (shapeRecord is EndShapeRecord) break;
        }
        shapeRecords = list.ToArray();
    }

    public XmlElement ToXml(XmlDocument doc) {
        var ele = doc.CreateElement("ShapeWithStyle");
        ele.AppendChild(fillStyles.ToXml(doc));
        ele.AppendChild(lineStyles.ToXml(doc));
        // numFillBits
        var numFillBitsEle = doc.CreateElement("NumFillBits");
        numFillBitsEle.InnerText = numFillBits.ToString();
        ele.AppendChild(numFillBitsEle);
        // numLineBits
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