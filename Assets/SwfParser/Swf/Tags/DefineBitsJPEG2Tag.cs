using System.Collections.Generic;
using System.Text;
using System.Xml;

public class DefineBitsJPEG2Tag : SwfTag, ICharacterIdTag {

    public ushort characterID;
    public byte[] imageData;

    public DefineBitsJPEG2Tag(TagHeaderRecord header) : base(header) {
        // empty constructor
    }

    public DefineBitsJPEG2Tag(SwfByteArray bytes, TagHeaderRecord header) : base(header) {
        characterID = bytes.ReadUI16();
        int length = (int)header.length - 2;
        if (length > 0) {
            imageData = bytes.ReadBytes(length);
        }
    }

    public override XmlElement ToXml(XmlDocument doc) {
        var ele = CreateXmlElement(doc, "DefineBitsJPEG2");
        ele.SetAttribute("characterID", characterID.ToString());
        var imageDataStrBuilder = new StringBuilder("");
        for (int i = 0; i < imageData.Length; i++) {
            imageDataStrBuilder.Append(imageData[i]);
            if (i < imageData.Length - 1) {
                imageDataStrBuilder.Append(',');
            }
        }
        ele.SetAttribute("imageData", imageDataStrBuilder.ToString());
        return ele;
    }

    public void GetNeededCharacterIds(List<ushort> characterIds, Swf swf) {
        if (characterIds.IndexOf(characterID) < 0) {
            characterIds.Add(characterID);
        }
    }

    public ushort GetCharacterId() {
        return characterID;
    }
}