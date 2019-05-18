
using System.Xml;

public class DefineSpriteTag:SwfTag{
	public ushort spriteId;
	public ushort frameCount;
	public SwfTag[] controlTags;

	public override XmlElement toXml(XmlDocument doc){
		var ele=createXmlElement(doc,"DefineSprite");
		ele.SetAttribute("spriteId",spriteId.ToString());
		ele.SetAttribute("frameCount",frameCount.ToString());
		int len=controlTags.Length;
		for(int i=0;i<len;i++){
			ele.AppendChild(controlTags[i].toXml(doc));
		}
		return ele;
	}
}
