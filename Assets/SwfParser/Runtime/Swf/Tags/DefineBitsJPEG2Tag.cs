using System.Collections.Generic;
using System.Text;
using System.Xml;

[System.Serializable]
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

    public virtual ImageData ToImageData() {
        var imgData = new ImageData();
        imgData.characterID = characterID;
        bool isJpg = imageData[0] == 0xFF && (imageData[1] == 0xD8 || imageData[1] == 0xD9);
        bool isPng = imageData[0] == 0x89
                     && imageData[1] == 0x50
                     && imageData[2] == 0x4E
                     && imageData[3] == 0x47
                     && imageData[4] == 0x0D
                     && imageData[5] == 0x0A
                     && imageData[6] == 0x1A
                     && imageData[7] == 0x0A;
        bool isGif = imageData[0] == 0x47
                     && imageData[1] == 0x49
                     && imageData[2] == 0x46
                     && imageData[3] == 0x38
                     && imageData[4] == 0x39
                     && imageData[5] == 0x61;
        if (isPng) {
            imgData.type = ImageType.Png;
        } else if (isJpg || isGif) {
            imgData.type = ImageType.Jpg;
        }
        imgData.bytes = imageData;
        return imgData;
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