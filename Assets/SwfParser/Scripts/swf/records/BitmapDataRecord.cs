
using System.Text;
using System.Xml;

public struct BitmapDataRecord:IMapData{
	public IPixRecord[] bitmapPixelData;

	public XmlElement toXml(XmlDocument doc){
		var ele=doc.CreateElement("BitmapData");

		var bitmapPixelDataStrBuilder=new StringBuilder();
		int len=bitmapPixelData.Length;
		int maxId=len-1;
		for(var i=0;i<len;i++){
			bitmapPixelDataStrBuilder.Append(bitmapPixelData[i].ToString());
			if(i<maxId){
				bitmapPixelDataStrBuilder.Append(',');
			}
		}
		ele.SetAttribute("bitmapPixelData",bitmapPixelDataStrBuilder.ToString());
		return ele;
	}
}
