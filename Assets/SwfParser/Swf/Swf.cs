using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

public class Swf {

    public SwfHeader header;
    public readonly List<SwfTag> tags = new List<SwfTag>(256);

    #region 为方便访问而增加的列表
    public readonly List<SymbolClassTag> symbolClassTags = new List<SymbolClassTag>(24);
    public readonly List<DefineSpriteTag> defineSpriteTags = new List<DefineSpriteTag>(128);
    public readonly List<ushort> linkageDefineCharacterIds = new List<ushort>(256);
    public readonly List<ICharacterIdTag> linkageDefineTags = new List<ICharacterIdTag>(256);
    #endregion

    public Swf(SwfByteArray bytes) {
        header = new SwfHeader(bytes);
    }

    /// <summary>
    /// 查找有定义链接类名的 DefineSprite(在SymbolClassTag中定义) 的所有 Tag 与及使用到的 characterId
    /// </summary>
    public void FindLinkageDefineTags() {
        // 是否有定义链接类名的 DefineSpriteTag
        bool IsLinkageDefineSpriteTag(DefineSpriteTag defineSpriteTag) {
            for (int i = 0, c = symbolClassTags.Count; i < c; i++) {
                var symbols = symbolClassTags[i].symbols;
                for (int j = 0, len = symbols.Length; j < len; j++) {
                    if (defineSpriteTag.spriteId == symbols[j].tagId) {
                        return true;
                    }
                }
            }
            return false;
        }
        // 有定义链接类名的 DefineSpriteTag，找出引用的 CharacterId
        for (int i = 0, len = defineSpriteTags.Count; i < len; i++) {
            var defineSpriteTag = defineSpriteTags[i];
            if (IsLinkageDefineSpriteTag(defineSpriteTag)) {
                defineSpriteTag.GetNeededCharacterIds(linkageDefineCharacterIds, this);
            }
        }
        // 找到有定义链接类名的引用到的 ICharacterIdTag
        for (int i = 0, len = tags.Count; i < len; i++) {
            var tag = tags[i];
            if (tag is ICharacterIdTag characterIdTag) {
                bool isLinkageTag = linkageDefineCharacterIds.IndexOf(characterIdTag.GetCharacterId()) > -1;
                if (isLinkageTag) {
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
            Debug.LogFormat("ToXml tag type:{0},time:{1}", tag.header.type, sw.ElapsedMilliseconds);

            swfElement.AppendChild(tagXml);
        }
        return doc;
    }

    public SwfData ToData(SwfData swfData, bool isOnlyExportLinkage) {
        // dispose
        swfData.Dispose();
        // 
        swfData.symbolClassTags = symbolClassTags;
        swfData.tagTypeAndIndices = new TagTypeAndIndex[linkageDefineCharacterIds.Max() + 1];
        // 
        for (int i = 0, len = linkageDefineTags.Count; i < len; i++) {
            ICharacterIdTag characterIdTag = linkageDefineTags[i];
            //-------------------------------------------------------------------------------
            // 在运行时并非使用所有的 Tag，Tag 的属性也不全都使用，因此创建需使用的 tag 的精简版
            //-------------------------------------------------------------------------------
            TagTypeAndIndex tagTypeAndIndex = swfData.AddTagData((SwfTag)characterIdTag);
            swfData.tagTypeAndIndices[characterIdTag.GetCharacterId()] = tagTypeAndIndex;
        }
        return swfData;
    }

    /// <summary>
    /// 获取所有图像数据
    /// </summary>
    /// <param name="isOnlyExportLinkage"> 仅导出有链接类名的库元件 </param>
    /// <returns></returns>
    public ImageData[] GetImageDatas(bool isOnlyExportLinkage) {
        // 获取有链接类名的 DefineSprite(在SymbolClassTag中定义) 的所有 CharacterId

        var imageDatas = new List<ImageData>();
        for (int i = 0, len = tags.Count; i < len; i++) {
            var tag = tags[i];
            if (tag.header.type == (uint)TagType.DefineBits) {
                var defineBitsTag = (DefineBitsTag)tag;
                if (isOnlyExportLinkage && linkageDefineCharacterIds.IndexOf(defineBitsTag.characterID) > -1) {
                    var imageData = defineBitsTag.ToImageData();
                    if (imageData.bytes != null && imageData.bytes.Length > 0) {
                        imageDatas.Add(imageData);
                    }
                }
            } else if (tag.header.type == (uint)TagType.DefineBitsJPEG2) {
                var defineBitsJpeg2Tag = (DefineBitsJPEG2Tag)tag;
                if (isOnlyExportLinkage && linkageDefineCharacterIds.IndexOf(defineBitsJpeg2Tag.characterID) > -1) {
                    var imageData = defineBitsJpeg2Tag.ToImageData();
                    imageDatas.Add(imageData);
                }
            } else if (tag.header.type == (uint)TagType.DefineBitsJPEG3) {
                var defineBitsJpeg3Tag = (DefineBitsJPEG3Tag)tag;
                if (isOnlyExportLinkage && linkageDefineCharacterIds.IndexOf(defineBitsJpeg3Tag.characterID) > -1) {
                    var imageData = defineBitsJpeg3Tag.ToImageData();
                    imageDatas.Add(imageData);
                }
            } else if (tag.header.type == (uint)TagType.DefineBitsLossless) {
                var defineBitsLosslessTag = (DefineBitsLosslessTag)tag;
                if (isOnlyExportLinkage && linkageDefineCharacterIds.IndexOf(defineBitsLosslessTag.characterID) > -1) {
                    var imageData = defineBitsLosslessTag.ToImageData();
                    imageDatas.Add(imageData);
                }
            } else if (tag.header.type == (uint)TagType.DefineBitsLossless2) {
                var defineBitsLossless2Tag = (DefineBitsLossless2Tag)tag;
                if (isOnlyExportLinkage && linkageDefineCharacterIds.IndexOf(defineBitsLossless2Tag.characterID) > -1) {
                    var imageData = defineBitsLossless2Tag.ToImageData();
                    imageDatas.Add(imageData);
                }
            } else if (tag.header.type == (uint)TagType.DefineBitsJPEG4) {
                var defineBitsJpeg4Tag = (DefineBitsJPEG4Tag)tag;
                if (isOnlyExportLinkage && linkageDefineCharacterIds.IndexOf(defineBitsJpeg4Tag.characterID) > -1) {
                    var imageData = defineBitsJpeg4Tag.ToImageData();
                    imageDatas.Add(imageData);
                }
            }
        }
        return imageDatas.ToArray();
    }


}