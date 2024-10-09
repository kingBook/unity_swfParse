using System;
using System.Collections.Generic;
using UnityEngine;

public class DefineSpritePlayer {

    private SwfData m_swfData;
    private string m_symbolClassName;

    private List<List<TagTypeAndIndex>> m_frameDatas;

    public DefineSpritePlayer(SwfData swfData, string symbolClassName) {
        var tagTypeAndIndex = swfData.GetTagTypeAndIndex(symbolClassName);
        DefineSpriteTagData defineSpriteTagData = (DefineSpriteTagData)swfData.GetTagData(tagTypeAndIndex);
        // Debug.Log(defineSpriteTagData.spriteId);
        m_swfData = swfData;
        m_symbolClassName = symbolClassName;
        // 
        m_frameDatas = CreateFrameDatas(defineSpriteTagData);
        GotoFrame(0);
    }

    private List<List<TagTypeAndIndex>> CreateFrameDatas(DefineSpriteTagData defineSpriteTagData) {
        var frameDatas = new List<List<TagTypeAndIndex>>();
        for (int i = 0, len = defineSpriteTagData.frameCount; i < len; i++) {
            frameDatas[i] = new List<TagTypeAndIndex>();
        }
        // 
        int frameIndex = 0;
        for (int i = 0, len = defineSpriteTagData.tagTypeAndIndices.Length; i < len; i++) {
            var tagTypeAndIndex = defineSpriteTagData.tagTypeAndIndices[i];
            frameDatas[frameIndex].Add(tagTypeAndIndex);
            if ((TagType)tagTypeAndIndex.tagType == TagType.ShowFrame) {
                frameIndex++;
            }
        }
        return frameDatas;
    }

    public void GotoFrame(int frameIndex) {
        frameIndex = Math.Clamp(frameIndex, 0, m_frameDatas.Count - 1);
        List<TagTypeAndIndex> frameData = m_frameDatas[frameIndex];
        
        for (int i = 0, len = frameData.Count; i < len; i++) {
            var tagTypeAndIndex = frameData[i];
            var tagType = (TagType)tagTypeAndIndex.tagType;
            var index = tagTypeAndIndex.index;

            var tagData = m_swfData.GetTagData(tagTypeAndIndex);
            Debug.Log(tagType);
            switch (tagType) {
                case TagType.ShowFrame:
                    var showFrameTagData = (ShowFrameTagData)tagData;

                    break;
                case TagType.PlaceObject:
                    var placeObjectTagData = (PlaceObjectTagData)tagData;
                    //placeObjectTagData.characterId
                    //placeObjectTagData.depth
                    //placeObjectTagData.matrix
                    //placeObjectTagData.colorTransform
                    break;
                case TagType.PlaceObject2:
                    var placeObject2TagData = (PlaceObject2TagData)tagData;
                    if(placeObject2TagData.placeFlagHasCharacter){
                        //placeObject2TagData.characterId
                    }

                    break;
                case TagType.RemoveObject:
                    var removeobjectTagData = (RemoveObjectTagData)tagData;

                    break;
                case TagType.RemoveObject2:
                    var removeobject2TagData = (RemoveObject2TagData)tagData;

                    break;
                case TagType.FrameLabel:
                    var frameLabelTagData = (FrameLabelTagData)tagData;

                    break;
                case TagType.End:

                    break;
            }
        }

    }

}