using System.Xml;

public class JPEGTablesTag:SwfTag {
	public byte[] jpegData;

	public override XmlElement toXml(XmlDocument doc) {
		var ele=createXmlElement(doc,"JPEGTables");
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
