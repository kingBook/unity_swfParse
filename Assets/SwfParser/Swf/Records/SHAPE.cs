using System.Xml;

public struct SHAPE {

    public byte numFillBits;
    public byte numLineBits;
    public IShapeRecord[] shapeRecords;

    public XmlElement ToXml(XmlDocument doc) {
        var ele = doc.CreateElement("SHAPE");
        ele.SetAttribute("numFillBits", numFillBits.ToString());
        ele.SetAttribute("numLineBits", numLineBits.ToString());
        for (var i = 0; i < shapeRecords.Length; i++) {
            ele.AppendChild(shapeRecords[i].ToXml(doc));
        }
        return ele;
    }
}