using System.Collections.Generic;
using System.Xml;

[System.Serializable]
public class DefineScalingGridTag : Tag, ICharacterIdTag {

    public ushort characterId;
    public RectangleRecord splitter;

    public DefineScalingGridTag(SwfByteArray bytes, TagHeaderRecord header) : base(header) {
        characterId = bytes.ReadUI16();
        splitter = new RectangleRecord(bytes);
    }

    public override XmlElement ToXml(XmlDocument doc) {
        var ele = CreateXmlElement(doc, "DefineScalingGrid");
        ele.SetAttribute("characterId", characterId.ToString());
        ele.SetAttribute("splitter", splitter.ToString());
        return ele;
    }


    public void FindUsedCharacterIds(List<ushort> characterIds, Swf swf) {
        if (characterIds.IndexOf(characterId) < 0) {
            characterIds.Add(characterId);
        }
    }

    public ushort GetCharacterId() {
        return characterId;
    }

}