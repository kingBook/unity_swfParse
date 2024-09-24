using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine.UI;

public class DefineButtonTag : SwfTag, ICharacterIdTag {

    public ushort buttonId;
    public ButtonRecord[] characters;

    public byte characterEndFlag;
    //public actions;
    //public actionEndFlag;

    public DefineButtonTag(SwfByteArray bytes, TagHeaderRecord header) : base(header) {
        // default value
        buttonId = 0;
        characters = new ButtonRecord[0];
        characterEndFlag = 0;
        //

    }

    public override XmlElement ToXml(XmlDocument doc) {
        return base.ToXml(doc);
    }

    public void GetNeededCharacterIds(List<ushort> characterIds, Swf swf) {
        if (characterIds.IndexOf(buttonId) < 0) {
            characterIds.Add(buttonId);
        }
    }

    public ushort GetCharacterId() {
        return buttonId;
    }


}