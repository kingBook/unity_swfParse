﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class DefineButton2Tag : SwfTag, ICharacterIdTag {

    public ushort buttonId;
    public byte reservedFlags;
    public bool trackAsMenu;
    public ushort actionOffset;
    public ButtonRecord[] characters;

    public byte characterEndFlag;
    //public actions;
    //public actionEndFlag;

    public override XmlElement ToXml(XmlDocument doc) {
        var ele = CreateXmlElement(doc, "DefineButton2");
        ele.SetAttribute("buttonId", buttonId.ToString());
        ele.SetAttribute("reservedFlags", reservedFlags.ToString());
        ele.SetAttribute("trackAsMenu", trackAsMenu.ToString());
        ele.SetAttribute("actionOffset", actionOffset.ToString());
        for (int i = 0; i < characters.Length; i++) {
            ele.AppendChild(characters[i].ToXml(doc));
        }
        ele.SetAttribute("characterEndFlag", characterEndFlag.ToString());
        return ele;
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