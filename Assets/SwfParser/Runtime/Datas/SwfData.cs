using System.Collections.Generic;
using UnityEngine;

public class SwfData : ScriptableObject {

    public TagTypeAndIndex[] tagTypeAndIndices;

    public List<DefineShapeTagData> defineShapeTagDatas = new List<DefineShapeTagData>(256);

    public List<DefineBitsTagData> defineBitsTagDatas = new List<DefineBitsTagData>(256);

    public List<DefineSpriteTagData> defineSpriteTagDatas = new List<DefineSpriteTagData>(256);

    #region Display List and Sprite and MovieClip
    public List<ShowFrameTagData> showFrameTagDatas = new List<ShowFrameTagData>(64);
    public List<PlaceObjectTagData> placeObjectTagDatas = new List<PlaceObjectTagData>(64);
    public List<PlaceObject2TagData> placeObject2TagDatas = new List<PlaceObject2TagData>(64);
    public List<PlaceObject3TagData> placeObject3TagDatas = new List<PlaceObject3TagData>(64);
    public List<RemoveObjectTagData> removeObjectTagDatas = new List<RemoveObjectTagData>(64);
    public List<RemoveObject2TagData> removeObject2TagDatas = new List<RemoveObject2TagData>(64);
    public List<FrameLabelTagData> frameLabelTagDatas = new List<FrameLabelTagData>(64);
    #endregion
    
    public List<UnknownTagData> unknownTagDatas = new List<UnknownTagData>(64);
    
    
}