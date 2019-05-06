
using System.Text;
using System.Xml;

public struct AlphaColorMapDataRecord:IAlphaMapData{
	public RGBARecord[] colorTableRGB;
	public byte[] colormapPixelData;

	public XmlElement toXml(XmlDocument doc){
		var ele=doc.CreateElement("AlphaColorMapData");

		var colorTableRGBStrBuilder=new StringBuilder("");
		for(int i=0;i<colorTableRGB.Length;i++){
			colorTableRGBStrBuilder.Append(colorTableRGB[i]);
			if(i<colorTableRGB.Length-1){
				colorTableRGBStrBuilder.Append(',');
			}
		}
		ele.SetAttribute("colorTableRGB",colorTableRGBStrBuilder.ToString());

		var colormapPixelDataStrBuilder=new StringBuilder("");
		for(int i=0;i<colormapPixelData.Length;i++){
			colormapPixelDataStrBuilder.Append(colormapPixelData[i].ToString());
			if(i<colormapPixelData.Length-1){
				colormapPixelDataStrBuilder.Append(',');
			}
		}
		ele.SetAttribute("colormapPixelData",colormapPixelDataStrBuilder.ToString());
		return ele;
	}
}
