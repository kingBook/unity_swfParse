using System.Xml;

public struct MorphFillStyleArrayRecord {

    public byte fillStyleCount;
    public ushort fillStyleCountExtended;
    public MorphFillStyleRecord[] fillStyles;

    public MorphFillStyleArrayRecord(SwfByteArray bytes) {
        // default value
        fillStyleCountExtended = 0;
        //
        fillStyleCount = bytes.ReadUI8();
        if (fillStyleCount == 0xFF) {
            fillStyleCountExtended = bytes.ReadUI16();
        }
        fillStyles = new MorphFillStyleRecord[fillStyleCount];
        for (var i = 0; i < fillStyleCount; i++) {
            fillStyles[i] = new MorphFillStyleRecord(bytes);
        }
    }

    public XmlElement ToXml(XmlDocument doc) {
        var ele = doc.CreateElement("MorphFillStyleArray");
        ele.SetAttribute("fillStyleCount", fillStyleCount.ToString());
        if (fillStyleCount == 0xFF) {
            ele.SetAttribute("fillStyleCountExtended", fillStyleCountExtended.ToString());
        }
        for (int i = 0, len = fillStyles.Length; i < len; i++) {
            ele.AppendChild(fillStyles[i].ToXml(doc));
        }
        return ele;
    }

}