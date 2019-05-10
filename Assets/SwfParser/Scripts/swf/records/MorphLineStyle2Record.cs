
using System.Xml;

public class MorphLineStyle2Record:IMorphLineStyleRecord{
	public ushort startWidth;
	public ushort endWidth;
	public byte startCapStyle;
	public byte joinStyle;
	public bool hasFillFlag;
	public bool noHScaleFlag;
	public bool noVScaleFlag;
	public bool pixelHintingFlag;
	public byte reserved;
	public bool noClose;
	public byte endCapStyle;
	public ushort miterLimitFactor;
	public RGBARecord startColor;
	public RGBARecord endColor;
	public MorphFillStyleRecord fillType;

	public XmlElement toXml(XmlDocument doc){
		var ele=doc.CreateElement("MorphLineStyle2");
		ele.SetAttribute("startWidth",startWidth.ToString());
		ele.SetAttribute("endWidth",endWidth.ToString());
		ele.SetAttribute("startCapStyle",startCapStyle.ToString());
		ele.SetAttribute("joinStyle",joinStyle.ToString());
		ele.SetAttribute("hasFillFlag",hasFillFlag.ToString());
		ele.SetAttribute("noHScaleFlag",noHScaleFlag.ToString());
		ele.SetAttribute("noVScaleFlag",noVScaleFlag.ToString());
		ele.SetAttribute("pixelHintingFlag",pixelHintingFlag.ToString());
		ele.SetAttribute("reserved",reserved.ToString());
		ele.SetAttribute("noClose",noClose.ToString());
		ele.SetAttribute("endCapStyle",endCapStyle.ToString());
		ele.SetAttribute("miterLimitFactor",miterLimitFactor.ToString());
		ele.SetAttribute("startColor",startColor.ToString());
		ele.SetAttribute("endColor",endColor.ToString());
		ele.AppendChild(fillType.toXml(doc));
		return ele;
	}
	
}
