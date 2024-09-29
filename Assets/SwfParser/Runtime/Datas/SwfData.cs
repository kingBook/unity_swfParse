using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SwfData : ScriptableObject {

    public List<SymbolClassTag> symbolClassTags;

    /// <summary>
    /// 以 CharacterId 为索引储存，{tagType: 标签类型, index: 表示标签在 SwfData 中对应列表中的储存索引}
    /// </summary>
    public TagTypeAndIndex[] tagTypeAndIndices;

    public List<DefineShapeTagData> defineShapeTagDatas = new List<DefineShapeTagData>(256);
    public List<DefineBitsTagData> defineBitsTagDatas = new List<DefineBitsTagData>(256);
    public List<DefineSpriteTagData> defineSpriteTagDatas = new List<DefineSpriteTagData>(256);

    #region Display List. Sprite and MovieClip
    public List<ShowFrameTagData> showFrameTagDatas = new List<ShowFrameTagData>(256);
    public List<PlaceObjectTagData> placeObjectTagDatas = new List<PlaceObjectTagData>(256);
    public List<PlaceObject2TagData> placeObject2TagDatas = new List<PlaceObject2TagData>(256);
    public List<PlaceObject3TagData> placeObject3TagDatas = new List<PlaceObject3TagData>(256);
    public List<RemoveObjectTagData> removeObjectTagDatas = new List<RemoveObjectTagData>(256);
    public List<RemoveObject2TagData> removeObject2TagDatas = new List<RemoveObject2TagData>(256);
    public List<FrameLabelTagData> frameLabelTagDatas = new List<FrameLabelTagData>(256);
    #endregion

    public List<UnknownTagData> unknownTagDatas = new List<UnknownTagData>(256);


}