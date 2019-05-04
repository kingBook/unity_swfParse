using System.Xml;

public class DefineBitsTag:SwfTag {
	public ushort characterID;
	public byte[] jpegData;

	public override XmlElement toXml(XmlDocument doc) {
		var ele=createXmlElement(doc,"DefineBits");
		ele.SetAttribute("characterID",characterID.ToString());

		string jpegDataStr="";
		for(int i=0;i<jpegData.Length;i++){
			jpegDataStr+=jpegData[i].ToString();
			if(i<jpegData.Length-1){
				jpegDataStr+=",";
			}
		}
		ele.SetAttribute("jpegData",jpegDataStr);
		return ele;
	}
	
}
