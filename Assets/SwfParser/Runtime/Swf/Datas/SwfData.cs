using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// swf 精简的数据类
/// <para> 获取一个标签数据的步骤： </para>
/// <code> 
/// TagTypeAndIndex tagTypeAndIndex = swfData.tagTypeAndIndices[characterId]; 
/// swfData.GetTagData(tagTypeAndIndex);
/// </code>
/// </summary>
[System.Serializable]
public class SwfData : ScriptableObject {

    public List<SymbolClassTag> symbolClassTags;

    /// <summary>
    /// 记录标签数据存储位置
    /// <code>
    /// tagTypeAndIndices[characterId] = tagTypeAndIndex;
    /// TagTypeAndIndex {
    ///     tagType: 标签类型, 
    ///     index: 表示标签在 SwfData 中对应列表中的储存索引
    /// }
    /// </code>
    /// </summary>
    public TagTypeAndIndex[] tagTypeAndIndices;
    
    public AtlasesData atlasesData;

    [SerializeField] private List<DefineShapeTagData> m_defineShapeTagDatas = new List<DefineShapeTagData>(256);
    [SerializeField] private List<DefineBitsTagData> m_defineBitsTagDatas = new List<DefineBitsTagData>(256);
    [SerializeField] private List<DefineSpriteTagData> m_defineSpriteTagDatas = new List<DefineSpriteTagData>(256);

    #region Display List. Sprite and MovieClip
    [SerializeField] private List<ShowFrameTagData> m_showFrameTagDatas = new List<ShowFrameTagData>(256);
    [SerializeField] private List<PlaceObjectTagData> m_placeObjectTagDatas = new List<PlaceObjectTagData>(256);
    [SerializeField] private List<PlaceObject2TagData> m_placeObject2TagDatas = new List<PlaceObject2TagData>(256);
    [SerializeField] private List<PlaceObject3TagData> m_placeObject3TagDatas = new List<PlaceObject3TagData>(256);
    [SerializeField] private List<RemoveObjectTagData> m_removeObjectTagDatas = new List<RemoveObjectTagData>(256);
    [SerializeField] private List<RemoveObject2TagData> m_removeObject2TagDatas = new List<RemoveObject2TagData>(256);
    [SerializeField] private List<FrameLabelTagData> m_frameLabelTagDatas = new List<FrameLabelTagData>(256);
    #endregion

    [SerializeField] private List<UnknownTagData> m_unknownTagDatas = new List<UnknownTagData>(256);

    public void Dispose() {
        symbolClassTags = null;
        tagTypeAndIndices = null;
        atlasesData = null;
        m_defineShapeTagDatas.Clear();
        m_defineBitsTagDatas.Clear();
        m_defineSpriteTagDatas.Clear();
        m_showFrameTagDatas.Clear();
        m_placeObjectTagDatas.Clear();
        m_placeObject2TagDatas.Clear();
        m_placeObject3TagDatas.Clear();
        m_removeObjectTagDatas.Clear();
        m_removeObject2TagDatas.Clear();
        m_frameLabelTagDatas.Clear();
        m_unknownTagDatas.Clear();
    }

    public TagTypeAndIndex GetTagTypeAndIndex(string symbolClassName) {
        var symbolClassRecord = GetSymbolClassRecord(symbolClassName);
        return tagTypeAndIndices[symbolClassRecord.tagId];
    }

    public SymbolClassRecord GetSymbolClassRecord(string symbolClassName) {
        foreach (var symbolClassTag in symbolClassTags) {
            return symbolClassTag.symbols.Single(symbol => symbol.name == symbolClassName);
        }
        return new SymbolClassRecord();
    }

    public TagData GetTagData(TagTypeAndIndex tagTypeAndIndex) {
        TagType tagType = (TagType)tagTypeAndIndex.tagType;
        int index = tagTypeAndIndex.index;
        TagData tagData;
        switch (tagType) {
            // =============================================
            //             ICharacterIdTag
            // =============================================
            // bitmap
            case TagType.DefineBits:
            case TagType.JPEGTables:
            case TagType.DefineBitsJPEG2:
            case TagType.DefineBitsJPEG3:
            case TagType.DefineBitsJPEG4:
            case TagType.DefineBitsLossless:
            case TagType.DefineBitsLossless2:
                tagData = m_defineBitsTagDatas[index];
                break;
            // shape
            case TagType.DefineShape:
            case TagType.DefineShape2:
            case TagType.DefineShape3:
            case TagType.DefineShape4:
                tagData = m_defineShapeTagDatas[index];
                break;
            // sprite and movieClip
            case TagType.DefineSprite:
                tagData = m_defineSpriteTagDatas[index];
                break;
            // ---------------------------------------------
            //             ControlTag
            // ---------------------------------------------
            case TagType.ShowFrame:
                tagData = m_showFrameTagDatas[index];
                break;
            case TagType.PlaceObject:
                tagData = m_placeObjectTagDatas[index];
                break;
            case TagType.PlaceObject2:
                tagData = m_placeObject2TagDatas[index];
                break;
            case TagType.PlaceObject3:
                tagData = m_placeObject3TagDatas[index];
                break;
            case TagType.RemoveObject:
                tagData = m_removeObjectTagDatas[index];
                break;
            case TagType.RemoveObject2:
                tagData = m_removeObject2TagDatas[index];
                break;
            case TagType.FrameLabel:
                tagData = m_frameLabelTagDatas[index];
                break;
            default:
                tagData = m_unknownTagDatas[index];
                break;
        }
        return tagData;
    }

    public TagTypeAndIndex AddTagData(SwfTag tag) {
        int index;
        TagType tagType = (TagType)tag.header.type;
        switch (tagType) {
            // ---------------------------------------------
            //             ICharacterIdTag
            // ---------------------------------------------
            // bitmap
            case TagType.DefineBits:
            case TagType.JPEGTables:
            case TagType.DefineBitsJPEG2:
            case TagType.DefineBitsJPEG3:
            case TagType.DefineBitsJPEG4:
            case TagType.DefineBitsLossless:
            case TagType.DefineBitsLossless2:
                index = m_defineBitsTagDatas.Count;
                var defineBitsTagData = new DefineBitsTagData();
                defineBitsTagData.type = tag.header.type;
                defineBitsTagData.characterID = ((ICharacterIdTag)tag).GetCharacterId();
                m_defineBitsTagDatas.Add(defineBitsTagData);
                break;
            // shape
            case TagType.DefineShape:
            case TagType.DefineShape2:
            case TagType.DefineShape3:
            case TagType.DefineShape4:
                index = m_defineShapeTagDatas.Count;
                var defineShapeTagData = ((DefineShapeTag)tag).ToData();
                m_defineShapeTagDatas.Add(defineShapeTagData);
                break;
            // sprite and movieClip
            case TagType.DefineSprite:
                index = m_defineSpriteTagDatas.Count;
                var defineSpriteData = ((DefineSpriteTag)tag).ToData(this);
                m_defineSpriteTagDatas.Add(defineSpriteData);
                break;
            // =============================================
            //             ControlTag
            // =============================================
            case TagType.ShowFrame:
                index = m_showFrameTagDatas.Count;
                var showFrameTag = (ShowFrameTag)tag;
                m_showFrameTagDatas.Add(showFrameTag.ToData());
                break;
            case TagType.PlaceObject:
                index = m_placeObjectTagDatas.Count;
                var placeObjectTagData = ((PlaceObjectTag)tag).ToData();
                m_placeObjectTagDatas.Add(placeObjectTagData);
                break;
            case TagType.PlaceObject2:
                index = m_placeObject2TagDatas.Count;
                var placeObject2TagData = ((PlaceObject2Tag)tag).ToData();
                m_placeObject2TagDatas.Add(placeObject2TagData);
                break;
            case TagType.PlaceObject3:
                index = m_placeObject3TagDatas.Count;
                var placeObject3TagData = ((PlaceObject3Tag)tag).ToData();
                m_placeObject3TagDatas.Add(placeObject3TagData);
                break;
            case TagType.RemoveObject:
                index = m_removeObjectTagDatas.Count;
                var removeObjectTagData = ((RemoveObjectTag)tag).ToData();
                m_removeObjectTagDatas.Add(removeObjectTagData);
                break;
            case TagType.RemoveObject2:
                index = m_removeObject2TagDatas.Count;
                var removeObject2TagData = ((RemoveObject2Tag)tag).ToData();
                m_removeObject2TagDatas.Add(removeObject2TagData);
                break;
            case TagType.FrameLabel:
                index = m_frameLabelTagDatas.Count;
                var frameLabelTagData = ((FrameLabelTag)tag).ToData();
                m_frameLabelTagDatas.Add(frameLabelTagData);
                break;
            default:
                index = m_unknownTagDatas.Count;
                var unknownTagData = new UnknownTagData();
                unknownTagData.type = (uint)tagType;
                m_unknownTagDatas.Add(unknownTagData);
                break;
        }
        //
        var tagTypeAndIndex = new TagTypeAndIndex();
        tagTypeAndIndex.tagType = (uint)tagType;
        tagTypeAndIndex.index = index;
        return tagTypeAndIndex;
    }


}