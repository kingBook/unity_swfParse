using System.Collections.Generic;

[System.Serializable]
public class DefineSpriteTagData : TagData {
    public ushort spriteId;
    public ushort frameCount;

    public TagTypeAndIndex[] tagTypeAndIndices;
    public List<ShowFrameTagData> showFrameTagDatas = new List<ShowFrameTagData>(64);
    public List<PlaceObjectTagData> placeObjectTagDatas = new List<PlaceObjectTagData>(64);
    public List<PlaceObject2TagData> placeObject2TagDatas = new List<PlaceObject2TagData>(64);
    public List<PlaceObject3TagData> placeObject3TagDatas = new List<PlaceObject3TagData>(64);
    public List<RemoveObjectTagData> removeObjectTagDatas = new List<RemoveObjectTagData>(64);
    public List<RemoveObject2TagData> removeObject2TagDatas = new List<RemoveObject2TagData>(64);
    public List<FrameLabelTagData> frameLabelTagDatas = new List<FrameLabelTagData>(64);
}