using UnityEngine;
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

    public DefineButton2Tag(SwfByteArray bytes, TagHeaderRecord header) : base(header) {
        buttonId = bytes.ReadUI16();
        reservedFlags = (byte)bytes.ReadUB(7);
        trackAsMenu = bytes.ReadFlag();
        actionOffset = bytes.ReadUI16();

        var count = 0;
        var btnRecords = new List<ButtonRecord>();
        while (true) {
            byte reserved = (byte)bytes.ReadUB(2);
            bool hasBlendMode = bytes.ReadFlag();
            bool hasFilterList = bytes.ReadFlag();
            bool stateHitTest = bytes.ReadFlag();
            bool stateDown = bytes.ReadFlag();
            bool stateOver = bytes.ReadFlag();
            bool stateUp = bytes.ReadFlag();

            var isEnd = reserved == 0 &&
                        !hasBlendMode &&
                        !hasFilterList &&
                        !stateHitTest &&
                        !stateDown &&
                        !stateOver &&
                        !stateUp;

            if (isEnd) {
                break;
            } else {
                count++;
                btnRecords.Add(new ButtonRecord(bytes, reserved, hasBlendMode, hasFilterList, stateHitTest, stateDown, stateOver, stateUp, 2));
            }
        }
        characters = btnRecords.ToArray();
        characterEndFlag = 0;
        //tag.actions = readButtonCondAction();
    }

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