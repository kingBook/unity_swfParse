using System.Xml;

public struct GradientRecord {

    public byte spreadMode;
    public byte interpolationMode;
    public byte numGradients;
    public GradRecord[] gradientRecords;

    public XmlElement ToXml(XmlDocument doc) {
        var ele = doc.CreateElement("Gradient");
        ele.SetAttribute("spreadMode", spreadMode.ToString());
        ele.SetAttribute("interpolationMode", interpolationMode.ToString());
        ele.SetAttribute("numGradients", numGradients.ToString());
        for (int i = 0; i < gradientRecords.Length; i++) {
            ele.AppendChild(gradientRecords[i].ToXml(doc));
        }
        return ele;
    }
}