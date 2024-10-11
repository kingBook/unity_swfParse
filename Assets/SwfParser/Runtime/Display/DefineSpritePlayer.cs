using System;
using System.Collections.Generic;
using UnityEngine;

public class DefineSpritePlayer {

    private Swf m_swf;
    private string m_symbolClassName;

    public DefineSpritePlayer(Swf swf, string symbolClassName) {
        DefineSpriteTag defineSpriteTag = swf.GetUsedDefineSpriteTag(symbolClassName);
        // Debug.Log(defineSpriteTag.spriteId);
        m_swf = swf;
        m_symbolClassName = symbolClassName;
        // 
        //m_frameDatas = CreateFrameDatas(defineSpriteTag);
        //GotoFrame(0);
    }

    // private List<TagTypeAndIndex>[] CreateFrameDatas(DefineSpriteTag defineSpriteTag) {
    //     int frameCount = defineSpriteTag.frameCount;
    //     var frameDatas = new List<TagTypeAndIndex>[frameCount];
    //     for (int i = 0; i < frameCount; i++) {
    //         frameDatas[i] = new List<TagTypeAndIndex>();
    //     }
    //     // 
    //     int frameIndex = 0;
    //     for (int i = 0, len = defineSpriteTag.tagTypeAndIndices.Length; i < len; i++) {
    //         var tagTypeAndIndex = defineSpriteTag.tagTypeAndIndices[i];
    //         var tagType = (TagType)tagTypeAndIndex.tagType;
    //         if (tagType == TagType.End) break;
    //         if (tagType == TagType.ShowFrame) {
    //             frameIndex++;
    //             continue; // 不添加 ShowFrame
    //         }
    //         frameDatas[frameIndex].Add(tagTypeAndIndex);
    //     }
    //     return frameDatas;
    // }

    // public void GotoFrame(int frameIndex) {
    //     frameIndex = Math.Clamp(frameIndex, 0, m_frameDatas.Length - 1);
    //     List<TagTypeAndIndex> frameData = m_frameDatas[frameIndex];

    //     for (int i = 0, len = frameData.Count; i < len; i++) {
    //         var tagTypeAndIndex = frameData[i];
    //         var tagType = (TagType)tagTypeAndIndex.tagType;
    //         var index = tagTypeAndIndex.index;

    //         var tag = m_swf.GetTag(tagTypeAndIndex);
    //         Debug.Log(tagType);
    //         switch (tagType) {
    //             case TagType.ShowFrame:
    //                 var showFrameTag = (ShowFrameTag)tag;

    //                 break;
    //             case TagType.PlaceObject:
    //                 var placeObjectTag = (PlaceObjectTag)tag;
    //                 //placeObjectTag.characterId
    //                 //placeObjectTag.depth
    //                 //placeObjectTag.matrix
    //                 //placeObjectTag.colorTransform
    //                 break;
    //             case TagType.PlaceObject2:
    //                 var placeObject2Tag = (PlaceObject2Tag)tag;
    //                 if (placeObject2Tag.placeFlagHasCharacter) {
    //                     //placeObject2Tag.characterId
    //                 }
    //                 Debug.Log("placeObject2Tag.matrix:" + placeObject2Tag.matrix);
    //                 break;
    //             case TagType.PlaceObject3:

    //                 break;
    //             case TagType.RemoveObject:
    //                 var removeobjectTag = (RemoveObjectTag)tag;

    //                 break;
    //             case TagType.RemoveObject2:
    //                 var removeobject2Tag = (RemoveObject2Tag)tag;

    //                 break;
    //             case TagType.FrameLabel:
    //                 var frameLabelTag = (FrameLabelTag)tag;

    //                 break;
    //             case TagType.End:

    //                 break;
    //         }
    //     }

    // }

}