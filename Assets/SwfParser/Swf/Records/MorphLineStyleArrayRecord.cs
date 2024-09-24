using System.Xml;

public class MorphLineStyleArrayRecord {

    public byte lineStyleCount;
    public ushort lineStyleCountExtended;
    public IMorphLineStyleRecord[] lineStyles;

    public MorphLineStyleArrayRecord(SwfByteArray bytes, byte morphShapeType) {
        lineStyleCount = bytes.ReadUI8();
        if (lineStyleCount == 0xFF) {
            lineStyleCountExtended = bytes.ReadUI16();
        }
        if (morphShapeType == 1) {
            lineStyles = new MorphLineStyleRecord[lineStyleCount];
            for (var i = 0; i < lineStyleCount; i++) {
                lineStyles[i] = new MorphLineStyleRecord(bytes);
            }
        } else if (morphShapeType == 2) {
            lineStyles = new MorphLineStyle2Record[lineStyleCount];
            for (var i = 0; i < lineStyleCount; i++) {
                lineStyles[i] = new MorphLineStyle2Record(bytes);
            }
        }
    }

    public XmlElement ToXml(XmlDocument doc) {
        var ele = doc.CreateElement("MorphLineStyleArray");
        ele.SetAttribute("lineStyleCount", lineStyleCount.ToString());
        if (lineStyleCount == 0xFF) {
            ele.SetAttribute("lineStyleCountExtended", lineStyleCountExtended.ToString());
        }
        for (int i = 0, len = lineStyles.Length; i < len; i++) {
            ele.AppendChild(lineStyles[i].ToXml(doc));
        }
        return ele;
    }

}