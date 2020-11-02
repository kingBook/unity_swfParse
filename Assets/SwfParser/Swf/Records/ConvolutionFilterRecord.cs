
using System.Xml;

public struct ConvolutionFilterRecord{
	public byte matrixX;
	public byte matrixY;
	public float divisor;
	public float bias;
	public float[] matrix;
	public RGBARecord defaultColor;
	public byte reserved;
	public bool clamp;
	public bool preserveAlpha;

	public XmlElement ToXml(XmlDocument doc){
		var ele=doc.CreateElement("ConvolutionFilter");
		ele.SetAttribute("matrixX",matrixX.ToString());
		ele.SetAttribute("matrixY",matrixY.ToString());
		ele.SetAttribute("divisor",divisor.ToString());
		ele.SetAttribute("bias",bias.ToString());

		var mastrixStr="";
		int len=matrix.Length;
		int maxId=len-1;
		for(int i=0;i<len;i++){
			mastrixStr+=matrix[i].ToString();
			if(i<maxId)mastrixStr+=',';
		}
		ele.SetAttribute("matrix",mastrixStr);
		ele.SetAttribute("defaultColor",defaultColor.ToString());
		ele.SetAttribute("reserved",reserved.ToString());
		ele.SetAttribute("clamp",clamp.ToString());
		ele.SetAttribute("preserveAlpha",preserveAlpha.ToString());
		return ele;
	}
	
}
