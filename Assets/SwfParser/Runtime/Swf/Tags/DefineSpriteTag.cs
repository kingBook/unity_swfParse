using System.Collections.Generic;
using System.Xml;

public class DefineSpriteTag : SwfTag, ICharacterIdTag {

    public ushort spriteId;
    public ushort frameCount;
    public SwfTag[] controlTags;

    public DefineSpriteTag(SwfByteArray bytes, TagHeaderRecord header) : base(header) {
        spriteId = bytes.ReadUI16();
        frameCount = bytes.ReadUI16();
        controlTags = ReadControlTags(bytes);
    }

    private SwfTag[] ReadControlTags(SwfByteArray bytes) {
        var tempTags = new List<SwfTag>();
        while (true) {
            var header = new TagHeaderRecord(bytes);
            long startPosition = bytes.GetBytePosition();
            long expectedEndPosition = startPosition + header.length;
            var tag = TagFactory.CreateTag(bytes, header);
            tempTags.Add(tag);
            bytes.SetBytePosition(expectedEndPosition);
            if (tag is EndTag) break;
        }
        return tempTags.ToArray();
    }

    public override XmlElement ToXml(XmlDocument doc) {
        var ele = CreateXmlElement(doc, "DefineSprite");
        ele.SetAttribute("spriteId", spriteId.ToString());
        ele.SetAttribute("frameCount", frameCount.ToString());
        int len = controlTags.Length;
        for (int i = 0; i < len; i++) {
            ele.AppendChild(controlTags[i].ToXml(doc));
        }
        return ele;
    }

    public void GetNeededCharacterIds(List<ushort> characterIds, Swf swf) {
        if (characterIds.IndexOf(spriteId) >= 0) return;

        characterIds.Add(spriteId);

        for (int i = 0, len = controlTags.Length; i < len; i++) {
            var tag = controlTags[i];
            if (tag is ICharacterIdTag characterIdTag) {
                characterIdTag.GetNeededCharacterIds(characterIds, swf);

                bool isPlaceObjectTag = characterIdTag is PlaceObjectTag ||
                                        characterIdTag is PlaceObject2Tag ||
                                        characterIdTag is PlaceObject3Tag;

                if (isPlaceObjectTag) {
                    for (int j = 0, lenJ = swf.tags.Count; j < lenJ; j++) {
                        var tempTag = swf.tags[j];
                        if (tempTag is ICharacterIdTag tempIdTag) {
                            tempIdTag.GetNeededCharacterIds(characterIds, swf);
                        }
                    }
                }

            }
        }

    }

    public ushort GetCharacterId() {
        return spriteId;
    }

    public DefineSpriteTagData ToData(SwfData swfData) {
        var tagData = new DefineSpriteTagData();
        tagData.spriteId = spriteId;
        tagData.frameCount = frameCount;

        int len = controlTags.Length;
        tagData.tagTypeAndIndices = new TagTypeAndIndex[len];
        for (int i = 0; i < len; i++) {
            var controlTag = controlTags[i];
            var tagTypeAndIndex = swfData.AddTagData(controlTag);
            tagData.tagTypeAndIndices[i] = tagTypeAndIndex;
        }
        return tagData;
    }

}