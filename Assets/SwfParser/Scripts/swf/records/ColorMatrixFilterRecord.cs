
using System.Xml;

public struct ColorMatrixFilterRecord{
	public float[] matrix;

	public XmlElement toXml(XmlDocument doc){
		var ele=doc.CreateElement("ColorMatrixFilter");
		var str="";
		for(int i=0;i<20;i++){
			str+=matrix[i].ToString();
			if(i<19)str+=',';
		}
		ele.SetAttribute("matrix",str);
		return ele;
	}
}
