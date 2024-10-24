﻿using System.Collections.Generic;
using System.Text;
using System.Xml;

[System.Serializable]
public class DefineBitsTag : Tag, ICharacterIdTag {

    public ushort characterID;
    public byte[] jpegData;

    public DefineBitsTag(SwfByteArray bytes, TagHeaderRecord header) : base(header) {
        characterID = bytes.ReadUI16();
        int length = (int)header.length - 2;
        if (length > 0) {
            jpegData = bytes.ReadBytes(length);
        }
    }

    public override XmlElement ToXml(XmlDocument doc) {
        var ele = CreateXmlElement(doc, "DefineBits");
        ele.SetAttribute("characterID", characterID.ToString());

        var jpegDataStrBuilder = new StringBuilder("");
        if (jpegData != null) {
            for (int i = 0; i < jpegData.Length; i++) {
                jpegDataStrBuilder.Append(jpegData[i]);
                if (i < jpegData.Length - 1) {
                    jpegDataStrBuilder.Append(',');
                }
            }
        }
        ele.SetAttribute("jpegData", jpegDataStrBuilder.ToString());
        return ele;
    }

    public ImageData ToImageData() {
        var imageData = new ImageData();
        if (jpegData != null) {
            imageData.characterID = characterID;
            imageData.type = ImageType.Jpg;
            imageData.bytes = jpegData;
        }
        return imageData;
    }

    public void FindUsedCharacterIds(List<ushort> characterIds, Swf swf) {
        if (characterIds.IndexOf(characterID) < 0) {
            characterIds.Add(characterID);
        }
    }

    public ushort GetCharacterId() {
        return characterID;
    }

}