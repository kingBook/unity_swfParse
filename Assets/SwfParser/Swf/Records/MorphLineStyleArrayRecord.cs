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
        for (var i = 0; i < lineStyles.Length; i++) {
            ele.AppendChild(lineStyles[i].ToXml(doc));
        }
        return ele;
    }

}