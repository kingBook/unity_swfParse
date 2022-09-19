using System.Text;
using System.Xml;

public class DefineBitsTag : SwfTag {

    public ushort characterID;
    public byte[] jpegData;

    public override XmlElement ToXml(XmlDocument doc) {
        var ele = CreateXmlElement(doc, "DefineBits");
        ele.SetAttribute("characterID", characterID.ToString());

        var jpegDataStrBuilder = new StringBuilder("");
        if (jpegData != null) {
            for (int i = 0; i < jpegData.Length; i++) {
                jpegDataStrBuilder.Append(jpegData[i]);
                if (i < jpegData.Length - 1) {
                    jpegDataStrBuilder.Append(',');
                }
            }
        }
        ele.SetAttribute("jpegData", jpegDataStrBuilder.ToString());
        return ele;
    }

}