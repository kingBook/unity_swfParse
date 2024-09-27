using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Text;

public class Swf {

    public SwfHeader header;
    public readonly List<SwfTag> tags = new List<SwfTag>(256);

    public readonly List<SymbolClassTag> symbolClassTags = new List<SymbolClassTag>(24);
    public readonly List<DefineSpriteTag> defineSpriteTags = new List<DefineSpriteTag>(128);
    public readonly List<ushort> linkageDefineCharacterIds = new List<ushort>(256);
    public readonly List<ICharacterIdTag> linkageDefineTags = new List<ICharacterIdTag>(256);

    public Swf(SwfByteArray bytes) {
        header = new SwfHeader(bytes);
    }

    /// <summary>
    /// 查找有定义链接类名的 DefineSprite(在SymbolClassTag中定义) 的所有 Tag 与及使用到的 characterId
    /// </summary>
    public void FindLinkageDefineTags() {
        for (int i = 0, len = defineSpriteTags.Count; i < len; i++) {
            var defineSpriteTag = defineSpriteTags[i];
            bool isLinkageDefineSpriteTag = false; // 是否有定义链接类名的 DefineSpriteTag
            for (int j = 0, lenJ = symbolClassTags.Count; j < lenJ; j++) {
                var symbols = symbolClassTags[j].symbols;
                for (int k = 0, lenK = symbols.Length; k < lenK; k++) {
                    if (defineSpriteTag.spriteId == symbols[k].tagId) {
                        isLinkageDefineSpriteTag = true;
                        break;
                    }
                }
                if (isLinkageDefineSpriteTag) break;
            }
            if (isLinkageDefineSpriteTag) {
                defineSpriteTag.GetNeededCharacterIds(linkageDefineCharacterIds, this);
            }
        }

        for (int i = 0, len = tags.Count; i < len; i++) {
            var tag = tags[i];
            if (tag is ICharacterIdTag characterIdTag) {
                if (linkageDefineCharacterIds.IndexOf(characterIdTag.GetCharacterId()) > -1) {
                    linkageDefineTags.Add(characterIdTag);
                }
            }
        }
    }

    public XmlDocument ToXml() {
        var doc = new XmlDocument();
        XmlDeclaration declaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
        doc.AppendChild(declaration);

        var swfElement = doc.CreateElement("Swf");
        doc.AppendChild(swfElement);
        var sw = new System.Diagnostics.Stopwatch();
        for (int i = 0, len = tags.Count; i < len; i++) {
            var tag = tags[i];

            sw.Restart();
            var tagXml = tag.ToXml(doc);
            sw.Stop();
            Debug.LogFormat("type:{0},time:{1}", tag.header.type, sw.ElapsedMilliseconds);

            swfElement.AppendChild(tagXml);
        }
        return doc;
    }

    public SwfData ToData(bool isOnlyExportLinkage) {
        var swfData = ScriptableObject.CreateInstance<SwfData>();
        swfData.symbolClassTags = symbolClassTags;

        swfData.tagTypeAndIndices = new TagTypeAndIndex[linkageDefineCharacterIds.Max() + 1];
        for (int i = 0, len = linkageDefineTags.Count; i < len; i++) {
            var characterIdTag = linkageDefineTags[i];
            TagType tagType = (TagType)(((SwfTag)characterIdTag).header.type);
            int dataIndex = -1;
            //-------------------------------------------------------------------------------
            // 在运行时并没有使用所有的 Tag，Tag 的属性也不全都使用，因此创建需使用的 tag 的精简版
            //-------------------------------------------------------------------------------
            switch (tagType) {
                // bitmap
                case TagType.DefineBits:
                case TagType.JPEGTables:
                case TagType.DefineBitsJPEG2:
                case TagType.DefineBitsJPEG3:
                case TagType.DefineBitsJPEG4:
                case TagType.DefineBitsLossless:
                case TagType.DefineBitsLossless2:
                    dataIndex = swfData.defineBitsTagDatas.Count;
                    var defineBitsTagData = GetDefineBitsTagData(characterIdTag);
                    swfData.defineBitsTagDatas.Add(defineBitsTagData);
                    break;
                // shape
                case TagType.DefineShape:
                case TagType.DefineShape2:
                case TagType.DefineShape3:
                case TagType.DefineShape4:
                    dataIndex = swfData.defineShapeTagDatas.Count;
                    var defineShapeTagData = ((DefineShapeTag)characterIdTag).ToData();
                    swfData.defineShapeTagDatas.Add(defineShapeTagData);
                    break;
                // sprite and movieClip
                case TagType.DefineSprite:
                    dataIndex = swfData.defineSpriteTagDatas.Count;
                    var defineSpriteData = ((DefineSpriteTag)characterIdTag).ToData(swfData);
                    swfData.defineSpriteTagDatas.Add(defineSpriteData);
                    break;
                default:
                    dataIndex = swfData.unknownTagDatas.Count;
                    var unknownTagData = new UnknownTagData();
                    unknownTagData.type = (uint)tagType;
                    swfData.unknownTagDatas.Add(unknownTagData);
                    break;
            }

            var tagTypeAndIndex = new TagTypeAndIndex();
            tagTypeAndIndex.tagType = (uint)tagType;
            tagTypeAndIndex.index = dataIndex;
            swfData.tagTypeAndIndices[characterIdTag.GetCharacterId()] = tagTypeAndIndex;
        }
        return swfData;
    }

    private DefineBitsTagData GetDefineBitsTagData(ICharacterIdTag characterIdTag) {
        var data = new DefineBitsTagData();
        data.type = ((SwfTag)characterIdTag).header.type;
        data.characterID = characterIdTag.GetCharacterId();
        return data;
    }

}