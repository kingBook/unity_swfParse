using System.Xml;

public struct LineStyleArrayRecord {
    public byte lineStyleCount;
    public ushort lineStyleCountExtended;

    /*If Shape1,Shape2, or Shape3, LINESTYLE[count]. 
      If Shape4,LINESTYLE2[count] */
    public ILineStyleRecord[] lineStyles;

    public LineStyleArrayRecord(SwfByteArray bytes, byte shapeType) {
        // default value
        lineStyleCountExtended = 0;
        //
        lineStyleCount = bytes.ReadUI8();
        if (lineStyleCount == 0xFF) {
            lineStyleCountExtended = bytes.ReadUI16();
        }
        var list = new ILineStyleRecord[lineStyleCount];
        if (shapeType == 1 || shapeType == 2 || shapeType == 3) {
            for (int i = 0; i < lineStyleCount; i++) {
                list[i] = new LineStyleRecord(bytes, shapeType);
            }
        } else if (shapeType == 4) {
            for (int i = 0; i < lineStyleCount; i++) {
                list[i] = new LineStyle2Record(bytes, shapeType);
            }
        }
        lineStyles = list;
    }

    public XmlElement ToXml(XmlDocument doc) {
        var ele = doc.CreateElement("LineStyleArray");
        ele.SetAttribute("lineStyleCount", lineStyleCount.ToString());
        ele.SetAttribute("lineStyleCountExtended", lineStyleCountExtended.ToString());
        for (int i = 0, len = lineStyles.Length; i < len; i++) {
            ele.AppendChild(lineStyles[i].ToXml(doc));
        }
        return ele;
    }


}