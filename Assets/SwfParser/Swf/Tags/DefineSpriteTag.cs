using System.Collections.Generic;
using System.Xml;

public class DefineSpriteTag : SwfTag, ICharacterIdTag {

    public ushort spriteId;
    public ushort frameCount;
    public SwfTag[] controlTags;

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
        if (characterIds.IndexOf(spriteId) < 0) {
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
    }
}