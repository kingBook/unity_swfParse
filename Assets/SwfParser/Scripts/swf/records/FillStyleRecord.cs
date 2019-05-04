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
		if(fillStyleType==0x00){
			var colorEle=doc.CreateElement("Color");
			colorEle.InnerText=color.ToString();
			ele.AppendChild(colorEle);
		}
		//gradientMatrix
		if(fillStyleType==0x10||fillStyleType==0x12||fillStyleType==0x13){
			var gradientMatrixEle=doc.CreateElement("GradientMatrix");
			gradientMatrixEle.InnerText=gradientMatrix.ToString();
			ele.AppendChild(gradientMatrixEle);
		}
		//gradient
		if(fillStyleType==0x10||fillStyleType==0x12){
			ele.AppendChild( ((GradientRecord)gradient).toXml(doc) );
		}else if(fillStyleType==0x13){
			ele.AppendChild( ((FocalGradientRecord)gradient).toXml(doc) );
		}
		if(fillStyleType==0x40||fillStyleType==0x41||fillStyleType==0x42||fillStyleType==0x43){
			//bitmapId
			var bitmapIdEle=doc.CreateElement("BitmapId");
			bitmapIdEle.InnerText=bitmapId.ToString();
			ele.AppendChild(bitmapIdEle);
			//bitmapMatrix
			var bitmapMatrixEle=doc.CreateElement("BitmapMatrix");
			bitmapMatrixEle.InnerText=bitmapMatrix.ToString();
			ele.AppendChild(bitmapMatrixEle);
		}
		return ele;
	}

}
