
using System.Xml;

public class DefineBitsJPEG2Tag:SwfTag {
	public ushort characterID;
	public byte[] imageData;

	public override XmlElement toXml(XmlDocument doc) {
		var ele=createXmlElement(doc,"DefineBitsJPEG2");
		ele.SetAttribute("characterID",characterID.ToString());
		string imageDataStr="";
		for(int i=0;i<imageData.Length;i++){
			imageDataStr+=imageData[i].ToString();
			if(i<imageData.Length-1){
				imageDataStr+=",";
			}
		}
		ele.SetAttribute("imageData",imageDataStr);
		return ele;
	}
	
}
