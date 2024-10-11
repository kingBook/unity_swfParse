using System.Xml;

[System.Serializable]
public struct MorphGradientRecord {

    public byte numGradients;
    public MorphGradRecord[] gradientRecords;

    public MorphGradientRecord(SwfByteArray bytes) {
        numGradients = bytes.ReadUI8();
        gradientRecords = new MorphGradRecord[numGradients];
        for (var i = 0; i < numGradients; i++) {
            gradientRecords[i] = new MorphGradRecord(bytes);
        }
    }

    public XmlElement ToXml(XmlDocument doc) {
        var ele = doc.CreateElement("MorphGradient");
        ele.SetAttribute("numGradients", numGradients.ToString());
        Debug2.Log("gradientRecords:", gradientRecords);
        for (var i = 0; i < gradientRecords.Length; i++) {
            ele.AppendChild(gradientRecords[i].ToXml(doc));
        }
        return ele;
    }

}