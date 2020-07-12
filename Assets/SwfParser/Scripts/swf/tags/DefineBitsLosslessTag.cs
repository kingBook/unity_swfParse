using System.Xml;

public class DefineBitsLosslessTag:SwfTag {
	public ushort characterID;
	public byte bitmapFormat;
	public ushort bitmapWidth;
	public ushort bitmapHeight;
	public byte bitmapColorTableSize;
	public IMapData zlibBitmapData;
	
	public override XmlElement toXml(XmlDocument doc) {
		var ele=createXmlElement(doc,"DefineBitsLossless");
		ele.SetAttribute("characterID",characterID.ToString());
		ele.SetAttribute("bitmapFormat",bitmapFormat.ToString());
		ele.SetAttribute("bitmapWidth",bitmapWidth.ToString());
		ele.SetAttribute("bitmapHeight",bitmapHeight.ToString());
		ele.SetAttribute("bitmapColorTableSize",bitmapFormat.ToString());
		ele.AppendChild(zlibBitmapData.toXml(doc));
		return ele;
	}
	
}
