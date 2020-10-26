using System.Xml;

public struct FillStyleArrayRecord{
	public byte fillStyleCount;
	public ushort fillStyleCountExtended;
	public FillStyleRecord[] fillStyles;

	public XmlElement ToXml(XmlDocument doc){
		var ele=doc.CreateElement("FillStyleArray");
		ele.SetAttribute("fillStyleCount",fillStyleCount.ToString());
		ele.SetAttribute("fillStyleCountExtended",fillStyleCountExtended.ToString());
		for(int i=0;i<fillStyles.Length;i++){
			ele.AppendChild(fillStyles[i].ToXml(doc));
		}
		return ele;
	}
}
