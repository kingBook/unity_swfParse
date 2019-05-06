
using System.Text;
using System.Xml;

public struct AlphaBitmapDataRecord:IAlphaMapData{
	public ARGBRecord[] bitmapPixelData;

	public XmlElement toXml(XmlDocument doc){
		var ele=doc.CreateElement("AlphaBitmapData");

		var bitmapPixelDataStrBuilder=new StringBuilder("");
		for(int i=0;i<bitmapPixelData.Length;i++){
			bitmapPixelDataStrBuilder.Append(bitmapPixelData[i].ToString());
			if(i<bitmapPixelData.Length-1){
				bitmapPixelDataStrBuilder.Append(',');
			}
		}
		ele.SetAttribute("bitmapPixelData",bitmapPixelDataStrBuilder.ToString());
		return ele;
	}
}
