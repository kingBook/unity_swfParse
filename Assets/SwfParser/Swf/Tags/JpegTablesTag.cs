using System.Xml;

public class JPEGTablesTag : SwfTag {

    public byte[] jpegData;

    public JPEGTablesTag(SwfByteArray bytes, TagHeaderRecord header) : base(header) {
        int length = (int)header.length;
        if (length > 0) {
            jpegData = bytes.ReadBytes(length);
        }
    }

    public override XmlElement ToXml(XmlDocument doc) {
        var ele = CreateXmlElement(doc, "JPEGTables");
        string jpegDataStr = "";
        if (jpegData != null) {
            for (int i = 0; i < jpegData.Length; i++) {
                jpegDataStr += jpegData[i].ToString();
                if (i < jpegData.Length - 1) {
                    jpegDataStr += ",";
                }
            }
        }
        ele.SetAttribute("jpegData", jpegDataStr);
        return ele;
    }

}