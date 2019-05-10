
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
		var ele=doc.CreateElement("MorphFillStyle");
		ele.SetAttribute("fillStyleType",fillStyleType.ToString());
		ele.SetAttribute("startColor",startColor.ToString());
		ele.SetAttribute("endColor",endColor.ToString());
		ele.SetAttribute("startGradientMatrix",startGradientMatrix.ToString());
		ele.SetAttribute("endGradientMatrix",endGradientMatrix.ToString());
		ele.AppendChild(gradient.toXml(doc));
		ele.SetAttribute("bitmapId",bitmapId.ToString());
		ele.SetAttribute("startBitmapMatrix",startBitmapMatrix.ToString());
		ele.SetAttribute("endBitmapMatrix",endBitmapMatrix.ToString());
		return ele;
	}
	
}
