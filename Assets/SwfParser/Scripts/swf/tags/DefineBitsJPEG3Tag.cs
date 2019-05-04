
using System.Xml;

public class DefineBitsJPEG3Tag:SwfTag {
	public ushort characterID;
	public uint alphaDataOffset;
	public byte[] imageData;
	public byte[] bitmapAlphaData;

	public override XmlElement toXml(XmlDocument doc) {
		var ele=createXmlElement(doc,"DefineBitsJPEG3");
		ele.SetAttribute("characterID",characterID.ToString());
		ele.SetAttribute("alphaDataOffset",alphaDataOffset.ToString());

		string imageDataStr="";
		for(int i=0;i<imageData.Length;i++){
			imageDataStr+=imageData[i].ToString();
			if(i<imageData.Length-1){
				imageDataStr+=",";
			}
		}
		ele.SetAttribute("imageData",imageDataStr);

		string alphaDataStr="";
		for(int i=0;i<bitmapAlphaData.Length;i++){
			alphaDataStr+=bitmapAlphaData[i].ToString();
			if(i<bitmapAlphaData.Length-1){
				alphaDataStr+=",";
			}
		}
		ele.SetAttribute("bitmapAlphaData",alphaDataStr);
		return ele;
	}
	
}
