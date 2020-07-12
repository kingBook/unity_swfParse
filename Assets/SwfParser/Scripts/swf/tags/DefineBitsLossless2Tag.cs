
using System.Xml;

public class DefineBitsLossless2Tag:SwfTag {
	public ushort characterID;
	public byte bitmapFormat;
	public ushort bitmapWidth;
	public ushort bitmapHeight;
	public byte bitmapColorTableSize;
	public IAlphaMapData zlibBitmapData;

	public override XmlElement toXml(XmlDocument doc) {
		var ele=createXmlElement(doc,"DefineBitsLossless2");
		ele.SetAttribute("characterID",characterID.ToString());
		ele.SetAttribute("bitmapFormat",bitmapFormat.ToString());
		ele.SetAttribute("bitmapWidth",bitmapWidth.ToString());
		ele.SetAttribute("bitmapHeight",bitmapHeight.ToString());
		ele.SetAttribute("bitmapColorTableSize",bitmapFormat.ToString());
		ele.AppendChild(zlibBitmapData.toXml(doc));
		return ele;
	}
	
}
