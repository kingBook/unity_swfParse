using System.Xml;

[System.Serializable]
public struct ColorMatrixFilterRecord {

    public float[] matrix;

    public ColorMatrixFilterRecord(SwfByteArray bytes) {
        matrix = new float[20];
        for (byte i = 0; i < 20; i++) {
            float value = bytes.ReadFloat();
            matrix[i] = value;
        }
    }

    public XmlElement ToXml(XmlDocument doc) {
        var ele = doc.CreateElement("ColorMatrixFilter");
        var str = "";
        for (int i = 0; i < 20; i++) {
            str += matrix[i].ToString();
            if (i < 19) str += ',';
        }
        ele.SetAttribute("matrix", str);
        return ele;
    }
}