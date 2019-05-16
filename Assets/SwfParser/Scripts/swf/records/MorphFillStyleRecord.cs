
using System.Xml;

public class MorphFillStyleRecord {
	public byte fillStyleType;
	public RGBARecord startColor;
	public RGBARecord endColor;
	public MatrixRecord startGradientMatrix;
	public MatrixRecord endGradientMatrix;
	public MorphGradientRecord gradient;
	public ushort bitmapId;
	public MatrixRecord startBitmapMatrix;
	public MatrixRecord endBitmapMatrix;

	public XmlElement toXml(XmlDocument doc){
		var type=fillStyleType;
		var ele=doc.CreateElement("MorphFillStyle");
		ele.SetAttribute("fillStyleType",fillStyleType.ToString());
		if(type==0x00){
			ele.SetAttribute("startColor",startColor.ToString());
			ele.SetAttribute("endColor",endColor.ToString());
		}else if(type==0x10||type==0x12){
			ele.SetAttribute("startGradientMatrix",startGradientMatrix.ToString());
			ele.SetAttribute("endGradientMatrix",endGradientMatrix.ToString());
			ele.AppendChild(gradient.toXml(doc));
		}else if(type==0x40||type==0x41||type==0x42||type==0x43){
			ele.SetAttribute("bitmapId",bitmapId.ToString());
			ele.SetAttribute("startBitmapMatrix",startBitmapMatrix.ToString());
			ele.SetAttribute("endBitmapMatrix",endBitmapMatrix.ToString());
		}
		return ele;
	}
	
}
