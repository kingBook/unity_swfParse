using System.Collections.Generic;
using System.Xml;

public class DefineScalingGridTag : SwfTag, ICharacterIdTag {

    public ushort characterId;
    public RectangleRecord splitter;

    public override XmlElement ToXml(XmlDocument doc) {
        var ele = CreateXmlElement(doc, "DefineScalingGrid");
        ele.SetAttribute("characterId", characterId.ToString());
        ele.SetAttribute("splitter", splitter.ToString());
        return ele;
    }


    public void GetNeededCharacterIds(List<ushort> characterIds, Swf swf) {
        if (characterIds.IndexOf(characterId) < 0) {
            characterIds.Add(characterId);
        }
    }

    public ushort GetCharacterId() {
        return characterId;
    }
}