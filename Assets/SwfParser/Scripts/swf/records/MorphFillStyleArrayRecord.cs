
using System.Xml;

public struct MorphFillStyleArrayRecord{
	public byte fillStyleCount;
	public ushort fillStyleCountExtended;
	public MorphFillStyleRecord[] fillStyles;
	
	public XmlElement toXml(XmlDocument doc){
		var ele=doc.CreateElement("MorphFillStyleArray");
		ele.SetAttribute("fillStyleCount",fillStyleCount.ToString());
		ele.SetAttribute("fillStyleCountExtended",fillStyleCountExtended.ToString());
		for(int i=0;i<fillStyles.Length;i++){
			ele.AppendChild(fillStyles[i].toXml(doc));
		}
		return ele;
	}

}
