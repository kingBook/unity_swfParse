using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class DefineButtonTag : SwfTag, ICharacterIdTag {

    public ushort buttonId;
    public ButtonRecord[] characters;

    public byte characterEndFlag;
    //public actions;
    //public actionEndFlag;

    public override XmlElement ToXml(XmlDocument doc) {
        return base.ToXml(doc);
    }

    public void GetNeededCharacterIds(List<ushort> characterIds, Swf swf) {
        if (characterIds.IndexOf(buttonId) < 0) {
            characterIds.Add(buttonId);
        }
    }

}