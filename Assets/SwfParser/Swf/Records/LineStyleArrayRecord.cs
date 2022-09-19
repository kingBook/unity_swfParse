using System.Collections;
using System.Xml;

public struct LineStyleArrayRecord {
    public byte lineStyleCount;
    public ushort lineStyleCountExtended;

    /*If Shape1,Shape2, or Shape3, LINESTYLE[count]. 
      If Shape4,LINESTYLE2[count] */
    public ILineStyleRecord[] lineStyles;

    public XmlElement ToXml(XmlDocument doc) {
        var ele = doc.CreateElement("LineStyleArray");
        ele.SetAttribute("lineStyleCount", lineStyleCount.ToString());
        ele.SetAttribute("lineStyleCountExtended", lineStyleCountExtended.ToString());
        for (int i = 0; i < lineStyles.Length; i++) {
            ele.AppendChild(lineStyles[i].ToXml(doc));
        }
        return ele;
    }


}