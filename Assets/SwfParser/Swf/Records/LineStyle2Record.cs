using System.Xml;

public struct LineStyle2Record:ILineStyleRecord{
	public ushort width;
	public byte startCapStyle;
	public byte joinStyle;
	public bool hasFillFlag;
	public bool noHScaleFlag;
	public bool noVScaleFlag;
	public bool pixelHintingFlag;
	public byte reserved;
	public bool noClose;
	public byte endCapStyle;
	public float miterLimitFactor;
	public RGBARecord color;
	public FillStyleRecord fillType;

	public XmlElement ToXml(XmlDocument doc){
		var ele=doc.CreateElement("LineStyle");
		ele.SetAttribute("width",width.ToString());
		ele.SetAttribute("startCapStyle",startCapStyle.ToString());
		ele.SetAttribute("joinStyle",joinStyle.ToString());
		ele.SetAttribute("hasFillFlag",hasFillFlag.ToString());
		ele.SetAttribute("noHScaleFlag",noHScaleFlag.ToString());
		ele.SetAttribute("noVScaleFlag",noVScaleFlag.ToString());
		ele.SetAttribute("pixelHintingFlag",pixelHintingFlag.ToString());
		ele.SetAttribute("reserved",reserved.ToString());
		ele.SetAttribute("noClose",noClose.ToString());
		ele.SetAttribute("endCapStyle",endCapStyle.ToString());
		if(joinStyle==2){
			ele.SetAttribute("miterLimitFactor",miterLimitFactor.ToString());
		}
		if(!hasFillFlag){
			ele.SetAttribute("color",color.ToString());
		}else{
			ele.AppendChild(fillType.ToXml(doc));
		}
		ele.SetAttribute("color",color.ToString());
		return ele;
	}
}
