using System.Xml;

public class MorphLineStyleArrayRecord {

    public byte lineStyleCount;
    public ushort lineStyleCountExtended;
    public IMorphLineStyleRecord[] lineStyles;

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