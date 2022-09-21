using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class RemoveObjectTag : SwfTag, ICharacterIdTag {

    public ushort characterId;
    public ushort depth;

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

}