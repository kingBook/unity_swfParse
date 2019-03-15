using System.Xml;

public struct FillStyleRecord{
	public byte fillStyleType;
	public object color;//RGB|RGBA
	public MatrixRecord gradientMatrix;
	public object gradient;
	public ushort bitmapId;
	public MatrixRecord bitmapMatrix;

	public XmlElement toXml(XmlDocument doc){
		var ele=doc.CreateElement("FillStyle");
		//fillStyleType
		var fillStyleTypeEle=doc.CreateElement("FillStyleType");
		fillStyleTypeEle.InnerText=fillStyleType.ToString();
		ele.AppendChild(fillStyleTypeEle);
		//color
		var colorEle=doc.CreateElement("Color");
		colorEle.InnerText=color.ToString();
		ele.AppendChild(colorEle);
		//gradientMatrix
		var gradientMatrixEle=doc.CreateElement("GradientMatrix");
		gradientMatrixEle.InnerText=gradientMatrix.ToString();
		ele.AppendChild(gradientMatrixEle);
		//gradient
		if(gradient is GradientRecord){
			ele.AppendChild( ((GradientRecord)gradient).toXml(doc) );
		}else if(gradient is FocalGradientRecord){
			ele.AppendChild( ((FocalGradientRecord)gradient).toXml(doc) );
		}
		//bitmapId
		var bitmapIdEle=doc.CreateElement("BitmapId");
		bitmapIdEle.InnerText=bitmapId.ToString();
		ele.AppendChild(bitmapIdEle);
		//bitmapMatrix
		var bitmapMatrixEle=doc.CreateElement("BitmapMatrix");
		bitmapMatrixEle.InnerText=bitmapMatrix.ToString();
		ele.AppendChild(bitmapMatrixEle);
		return ele;
	}

}
