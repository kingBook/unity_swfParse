using System.Xml;

public struct FocalGradientRecord{
	public byte spreadMode;
	public byte interpolationMode;

	public byte numGradients;
	public GradRecord[] gradientRecords;
	public float focalPoint;

	public XmlElement ToXml(XmlDocument doc){
		var ele=doc.CreateElement("FocalGradient");
		ele.SetAttribute("spreadMode",spreadMode.ToString());
		ele.SetAttribute("interpolationMode",interpolationMode.ToString());
		ele.SetAttribute("numGradients",numGradients.ToString());
		for(int i=0;i<gradientRecords.Length;i++){
			ele.AppendChild(gradientRecords[i].ToXml(doc));
		}
		ele.SetAttribute("focalPoint",focalPoint.ToString());
		return ele;
	}
}
