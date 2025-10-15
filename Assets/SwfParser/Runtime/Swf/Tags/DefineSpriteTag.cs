using System.Collections.Generic;
using System.Xml;
using UnityEngine;

[System.Serializable]
public class DefineSpriteTag : Tag, ICharacterIdTag {

    public ushort spriteId;
    public ushort frameCount;
    [SerializeReference]
    public Tag[] controlTags;

    public DefineSpriteTag(SwfByteArray bytes, TagHeaderRecord header) : base(header) {
        spriteId = bytes.ReadUI16();
        frameCount = bytes.ReadUI16();
        controlTags = ReadControlTags(bytes);
    }

    private Tag[] ReadControlTags(SwfByteArray bytes) {
        var tempTags = new List<Tag>();
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

    public override void Load(Swf swf, MeshHelperBase meshHelper, DisplayObjectContainer parent) {
        var mc = new MovieClip(swf, meshHelper, this);
        parent.AddChild(mc);
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

    public void FindUsedCharacterIds(List<ushort> characterIds, Swf swf) {
        if (characterIds.IndexOf(spriteId) >= 0) return;

        characterIds.Add(spriteId);

        for (int i = 0, len = controlTags.Length; i < len; i++) {
            var tag = controlTags[i];
            if (tag is ICharacterIdTag characterIdTag) {
                characterIdTag.FindUsedCharacterIds(characterIds, swf);

                bool isPlaceObjectTag = characterIdTag is PlaceObjectTag ||
                                        characterIdTag is PlaceObject2Tag ||
                                        characterIdTag is PlaceObject3Tag;

                if (isPlaceObjectTag) {
                    for (int j = 0, count = swf.tags.Count; j < count; j++) {
                        var tempTag = swf.tags[j];
                        if (tempTag is ICharacterIdTag tempIdTag) {
                            tempIdTag.FindUsedCharacterIds(characterIds, swf);
                        }
                    }
                }
            }
        }
    }

    public ushort GetCharacterId() {
        return spriteId;
    }


}