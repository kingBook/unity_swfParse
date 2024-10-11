using UnityEngine;
using System.Collections.Generic;
using System.Xml;

[System.Serializable]
public class Swf : ScriptableObject {

    public SwfHeader header;
    [System.NonSerialized]
    public List<Tag> tags;

    #region 额外增加的属性
    public List<SymbolClassTag> symbolClassTags;
    public List<ushort> usedCharacterIds;
    [SerializeReference]
    public List<ICharacterIdTag> usedCharacterIdTags;
    public AtlasesData atlasesData;
    #endregion

    /// <summary>
    ///  根据 swf 绝对路径创建
    /// </summary>
    /// <param name="swfPath"> 绝对路径, 如：E:/kingBook/projects/unity_swfParse/Assets/xx.swf </param>
    public static Swf Create(string swfPath) {
        Swf swf = CreateInstance<Swf>();
        swf.tags = new List<Tag>(256);
        swf.symbolClassTags = new List<SymbolClassTag>(24);
        swf.usedCharacterIds = new List<ushort>(256);
        swf.usedCharacterIdTags = new List<ICharacterIdTag>(256);

        var bytes = new SwfByteArray(swfPath);
        swf.header = new SwfHeader(bytes);
        swf.ReadTags(bytes);
        bytes.Close();
        // 查找使用到的标签
        swf.FindUsedCharacterIds();
        swf.FindUsedCharacterIdTags();
        return swf;
    }

    public void SetTo(Swf target) {
        header = target.header;
        tags = target.tags;
        symbolClassTags = target.symbolClassTags;
        usedCharacterIds = target.usedCharacterIds;
        usedCharacterIdTags = target.usedCharacterIdTags;
        atlasesData = target.atlasesData;
    }

    public void SetAtlasesData(AtlasesData value) {
        atlasesData = value;
    }

    /// <summary>
    /// 根据 symbolClassName 获取 DefineSpriteTag
    /// </summary>
    /// <param name="symbolClassName"> 链接类名 </param>
    /// <returns></returns>
    public DefineSpriteTag GetUsedDefineSpriteTag(string symbolClassName) {
        for (int i = 0, c = symbolClassTags.Count; i < c; i++) {
            var symbols = symbolClassTags[i].symbols;
            for (int j = 0, len = symbols.Length; j < len; j++) {
                var symobolClassRecord = symbols[j];
                if (symbolClassName == symobolClassRecord.name) {
                    return (DefineSpriteTag)GetUsedCharacterIdTag(symobolClassRecord.tagId);
                }
            }
        }
        return null;
    }

    /// <summary>
    /// 根据 characterId 获取标签
    /// </summary>
    /// <param name="characterId"></param>
    /// <returns></returns>
    public ICharacterIdTag GetUsedCharacterIdTag(ushort characterId) {
        for (int i = 0, len = usedCharacterIdTags.Count; i < len; i++) {
            var characterIdTag = usedCharacterIdTags[i];
            if (characterIdTag.GetCharacterId() == characterId) {
                return characterIdTag;
            }
        }
        return null;
    }

    private void ReadTags(SwfByteArray bytes) {
        while (bytes.GetBytesAvailable() > 0) {
            long preHeaderStart = bytes.GetBytePosition();
            TagHeaderRecord tagHeader = new TagHeaderRecord(bytes);

            long startPosition = bytes.GetBytePosition();
            long expectedEndPosition = startPosition + tagHeader.length;
            //Debug2.Log("type:"+tagHeader.type,"preHeaderStart:"+preHeaderStart,"length:"+tagHeader.length);
            Tag tag = TagFactory.CreateTag(bytes, tagHeader);
            AddTag(tag);

            bytes.AlignBytes();
            //long newPosition = bytes.GetBytePosition();

            bytes.SetBytePosition(expectedEndPosition);

            if (tag is EndTag) {
                break;
            }
        }
    }

    private void AddTag(Tag tag) {
        tags.Add(tag);

        if (tag is SymbolClassTag symbolClassTag) {
            symbolClassTags.Add(symbolClassTag);
        }
    }

    private void FindUsedCharacterIds() {
        for (int i = 0, len = tags.Count; i < len; i++) {
            var tag = tags[i];
            if (tag is DefineSpriteTag defineSpriteTag) {
                if (IsUsedDefineSpriteTag(defineSpriteTag)) {
                    defineSpriteTag.FindUsedCharacterIds(usedCharacterIds, this);
                }
            }
        }
    }

    private void FindUsedCharacterIdTags() {
        for (int i = 0, len = tags.Count; i < len; i++) {
            var tag = tags[i];
            if (tag is ICharacterIdTag characterIdTag) {
                bool isUsed = usedCharacterIds.IndexOf(characterIdTag.GetCharacterId()) > -1;
                if (isUsed) {
                    usedCharacterIdTags.Add(characterIdTag);
                }
            }
        }
    }

    /// <summary> 是否为使用到的 DefineSpriteTag（有定义链接类名）</summary>
    private bool IsUsedDefineSpriteTag(DefineSpriteTag defineSpriteTag) {
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

    /// <summary>
    /// 获取所有图像数据
    /// </summary>
    /// <param name="isOnlyExportLinkage"> 仅导出有链接类名的库元件 </param>
    /// <returns></returns>
    public ImageData[] GetImageDatas(bool isOnlyExportLinkage) {
        var imageDatas = new List<ImageData>();
        for (int i = 0, len = tags.Count; i < len; i++) {
            var tag = tags[i];
            var tagType = (TagType)tag.header.type;
            if (tagType == TagType.DefineBits) {
                var defineBitsTag = (DefineBitsTag)tag;
                if (isOnlyExportLinkage && usedCharacterIds.IndexOf(defineBitsTag.characterID) > -1) {
                    var imageData = defineBitsTag.ToImageData();
                    if (imageData.bytes != null && imageData.bytes.Length > 0) {
                        imageDatas.Add(imageData);
                    }
                }
            } else if (tagType == TagType.DefineBitsJPEG2) {
                var defineBitsJpeg2Tag = (DefineBitsJPEG2Tag)tag;
                if (isOnlyExportLinkage && usedCharacterIds.IndexOf(defineBitsJpeg2Tag.characterID) > -1) {
                    var imageData = defineBitsJpeg2Tag.ToImageData();
                    imageDatas.Add(imageData);
                }
            } else if (tagType == TagType.DefineBitsJPEG3) {
                var defineBitsJpeg3Tag = (DefineBitsJPEG3Tag)tag;
                if (isOnlyExportLinkage && usedCharacterIds.IndexOf(defineBitsJpeg3Tag.characterID) > -1) {
                    var imageData = defineBitsJpeg3Tag.ToImageData();
                    imageDatas.Add(imageData);
                }
            } else if (tagType == TagType.DefineBitsLossless) {
                var defineBitsLosslessTag = (DefineBitsLosslessTag)tag;
                if (isOnlyExportLinkage && usedCharacterIds.IndexOf(defineBitsLosslessTag.characterID) > -1) {
                    var imageData = defineBitsLosslessTag.ToImageData();
                    imageDatas.Add(imageData);
                }
            } else if (tagType == TagType.DefineBitsLossless2) {
                var defineBitsLossless2Tag = (DefineBitsLossless2Tag)tag;
                if (isOnlyExportLinkage && usedCharacterIds.IndexOf(defineBitsLossless2Tag.characterID) > -1) {
                    var imageData = defineBitsLossless2Tag.ToImageData();
                    imageDatas.Add(imageData);
                }
            } else if (tagType == TagType.DefineBitsJPEG4) {
                var defineBitsJpeg4Tag = (DefineBitsJPEG4Tag)tag;
                if (isOnlyExportLinkage && usedCharacterIds.IndexOf(defineBitsJpeg4Tag.characterID) > -1) {
                    var imageData = defineBitsJpeg4Tag.ToImageData();
                    imageDatas.Add(imageData);
                }
            }
        }
        return imageDatas.ToArray();
    }


}