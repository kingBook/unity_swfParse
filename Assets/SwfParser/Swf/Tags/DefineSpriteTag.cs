using System.Collections.Generic;
using System.Xml;

public class DefineSpriteTag : SwfTag, ICharacterIdTag {

    public ushort spriteId;
    public ushort frameCount;
    public SwfTag[] controlTags;

    public DefineSpriteTag(TagFactory tagFactory, SwfReader swfReader, SwfByteArray bytes, TagHeaderRecord header) : base(header) {
        spriteId = bytes.ReadUI16();
        frameCount = bytes.ReadUI16();
        controlTags = ReadControlTags(tagFactory, swfReader, bytes);
    }

    private SwfTag[] ReadControlTags(TagFactory tagFactory, SwfReader swfReader, SwfByteArray bytes) {
        var tempTags = new List<SwfTag>();
        while (true) {
            var header = new TagHeaderRecord(bytes);
            long startPosition = bytes.GetBytePosition();
            long expectedEndPosition = startPosition + header.length;
            var tag = tagFactory.CreateTag(tagFactory, swfReader, bytes, header);
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

    public ushort GetCharacterId() {
        return spriteId;
    }

    public DefineSpriteTagData ToData(SwfData swfData) {
        var data = new DefineSpriteTagData();
        data.spriteId = spriteId;
        data.frameCount = frameCount;

        int len = controlTags.Length;
        data.tagTypeAndIndices = new TagTypeAndIndex[len];
        for (int i = 0; i < len; i++) {
            var controlTag = controlTags[i];
            TagType controlTagType = (TagType)controlTag.header.type;
            int dataIndex = -1;
            switch (controlTagType) {
                case TagType.ShowFrame:
                    dataIndex = swfData.showFrameTagDatas.Count;
                    var showFrameTag = (ShowFrameTag)controlTag;
                    swfData.showFrameTagDatas.Add(showFrameTag.ToData());
                    break;
                case TagType.PlaceObject:
                    dataIndex = swfData.placeObjectTagDatas.Count;
                    var placeObjectTagData = ((PlaceObjectTag)controlTag).ToData();
                    swfData.placeObjectTagDatas.Add(placeObjectTagData);
                    break;
                case TagType.PlaceObject2:
                    dataIndex = swfData.placeObject2TagDatas.Count;
                    var placeObject2TagData = ((PlaceObject2Tag)controlTag).ToData();
                    swfData.placeObject2TagDatas.Add(placeObject2TagData);
                    break;
                case TagType.PlaceObject3:
                    dataIndex = swfData.placeObject3TagDatas.Count;
                    var placeObject3TagData = ((PlaceObject3Tag)controlTag).ToData();
                    swfData.placeObject3TagDatas.Add(placeObject3TagData);
                    break;
                case TagType.RemoveObject:
                    dataIndex = swfData.removeObjectTagDatas.Count;
                    var removeObjectTagData = ((RemoveObjectTag)controlTag).ToData();
                    swfData.removeObjectTagDatas.Add(removeObjectTagData);
                    break;
                case TagType.RemoveObject2:
                    dataIndex = swfData.removeObject2TagDatas.Count;
                    var removeObject2TagData = ((RemoveObject2Tag)controlTag).ToData();
                    swfData.removeObject2TagDatas.Add(removeObject2TagData);
                    break;
                case TagType.FrameLabel:
                    dataIndex = swfData.frameLabelTagDatas.Count;
                    var frameLabelTagData = ((FrameLabelTag)controlTag).ToData();
                    swfData.frameLabelTagDatas.Add(frameLabelTagData);
                    break;
            }
            TagTypeAndIndex tagTypeAndIndex = new TagTypeAndIndex();
            tagTypeAndIndex.tagType = controlTag.header.type;
            tagTypeAndIndex.index = dataIndex;
            data.tagTypeAndIndices[i] = tagTypeAndIndex;
        }
        return data;
    }

}