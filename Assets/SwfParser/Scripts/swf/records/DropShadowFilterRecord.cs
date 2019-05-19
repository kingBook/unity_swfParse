
using System.Xml;

public struct DropShadowFilterRecord{
	public RGBARecord dropShadowColor;
	public float blurX;
	public float blurY;
	public float angle;
	public float distance;
	public float strength;
	public bool innerShadow;
	public bool knockout;
	public bool compositeSource;
	public byte passes;
	
	public XmlElement toXml(XmlDocument doc){
		var ele=doc.CreateElement("DropShadowFilter");
		ele.SetAttribute("dropShadowColor",dropShadowColor.ToString());
		ele.SetAttribute("blurX",blurX.ToString());
		ele.SetAttribute("blurY",blurY.ToString());
		ele.SetAttribute("angle",angle.ToString());
		ele.SetAttribute("distance",distance.ToString());
		ele.SetAttribute("strength",strength.ToString());
		ele.SetAttribute("innerShadow",innerShadow.ToString());
		ele.SetAttribute("knockout",knockout.ToString());
		ele.SetAttribute("compositeSource",compositeSource.ToString());
		ele.SetAttribute("passes",passes.ToString());
		return ele;
	}

}
