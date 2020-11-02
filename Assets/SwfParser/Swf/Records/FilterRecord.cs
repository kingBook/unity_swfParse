
using System.Xml;

public struct FilterRecord{
	public byte filterId;
	public DropShadowFilterRecord dropShadowFilter;
	public BlurFilterRecord blurFilter;
	public GlowFilterRecord glowFilter;
	public BevelFilterRecord bevelFilter;
	public GradientGlowFilterRecord gradientGlowFilter;
	public ConvolutionFilterRecord convolutionFilter;
	public ColorMatrixFilterRecord colorMatrixFilter;
	public GradientBevelFilterRecord gradientBevelFilter;

	public XmlElement ToXml(XmlDocument doc){
		var ele=doc.CreateElement("Filter");
		ele.SetAttribute("filterId",filterId.ToString());
		switch(filterId){
			case 0:
				ele.AppendChild(dropShadowFilter.ToXml(doc));
				break;
			case 1:
				ele.AppendChild(blurFilter.ToXml(doc));
				break;
			case 2:
				ele.AppendChild(glowFilter.ToXml(doc));
				break;
			case 3:
				ele.AppendChild(bevelFilter.ToXml(doc));
				break;
			case 4:
				ele.AppendChild(gradientGlowFilter.ToXml(doc));
				break;
			case 5:
				ele.AppendChild(convolutionFilter.ToXml(doc));
				break;
			case 6:
				ele.AppendChild(colorMatrixFilter.ToXml(doc));
				break;
			case 7:
				ele.AppendChild(gradientBevelFilter.ToXml(doc));
				break;
		}
		return ele;
	}
}
