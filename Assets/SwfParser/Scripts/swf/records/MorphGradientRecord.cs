
using System.Xml;

public struct MorphGradientRecord{
	public byte numGradients;
	public MorphGradRecord[] gradientRecords;

	public XmlElement toXml(XmlDocument doc){
		var ele=doc.CreateElement("MorphGradient");
		ele.SetAttribute("numGradients",numGradients.ToString());
		Debug2.Log("gradientRecords:",gradientRecords);
		for(var i=0;i<gradientRecords.Length;i++){
			ele.AppendChild(gradientRecords[i].toXml(doc));
		}
		return ele;
	}
	
}
