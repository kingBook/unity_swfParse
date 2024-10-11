using System.Xml;

[System.Serializable]
public struct FocalGradientRecord {

    public byte spreadMode;
    public byte interpolationMode;

    public byte numGradients;
    public GradRecord[] gradientRecords;
    public float focalPoint;

    public FocalGradientRecord(SwfByteArray bytes, byte shapeType) {
        spreadMode = (byte)bytes.ReadUB(2);
        interpolationMode = (byte)bytes.ReadUB(2);
        numGradients = (byte)bytes.ReadUB(4);
        var list = new GradRecord[numGradients];
        for (byte i = 0; i < numGradients; i++) {
            list[i] = new GradRecord(bytes, shapeType);
        }
        gradientRecords = list;
        focalPoint = bytes.ReadFixed8_8();
    }

    public XmlElement ToXml(XmlDocument doc) {
        var ele = doc.CreateElement("FocalGradient");
        ele.SetAttribute("spreadMode", spreadMode.ToString());
        ele.SetAttribute("interpolationMode", interpolationMode.ToString());
        ele.SetAttribute("numGradients", numGradients.ToString());
        for (int i = 0; i < gradientRecords.Length; i++) {
            ele.AppendChild(gradientRecords[i].ToXml(doc));
        }
        ele.SetAttribute("focalPoint", focalPoint.ToString());
        return ele;
    }
}