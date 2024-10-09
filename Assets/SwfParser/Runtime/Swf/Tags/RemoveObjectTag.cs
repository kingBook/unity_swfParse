﻿using System.Collections.Generic;
using System.Xml;

public class RemoveObjectTag : SwfTag, ICharacterIdTag {

    public ushort characterId;
    public ushort depth;

    public RemoveObjectTag(SwfByteArray bytes, TagHeaderRecord header) : base(header) {
        characterId = bytes.ReadUI16();
        depth = bytes.ReadUI16();
    }

    public override XmlElement ToXml(XmlDocument doc) {
        var ele = CreateXmlElement(doc, "RemoveObject");
        ele.SetAttribute("characterId", characterId.ToString());
        ele.SetAttribute("depth", depth.ToString());
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

    public RemoveObjectTagData ToData() {
        var data = new RemoveObjectTagData();
        data.characterId = characterId;
        data.depth = depth;
        return data;
    }

}