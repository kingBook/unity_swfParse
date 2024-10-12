using System;
using System.Collections.Generic;
using UnityEngine;

public class DefineSpritePlayer {

    private List<Tag>[] m_frameDatas;

    public DefineSpritePlayer(Swf swf, string symbolClassName) {
        DefineSpriteTag defineSpriteTag = swf.GetUsedDefineSpriteTag(symbolClassName);
        m_frameDatas = CreateFrameDatas(defineSpriteTag);
        GotoFrame(0);
    }

    private List<Tag>[] CreateFrameDatas(DefineSpriteTag defineSpriteTag) {
        int frameCount = defineSpriteTag.frameCount;
        var frameDatas = new List<Tag>[frameCount];
        for (int i = 0; i < frameCount; i++) {
            frameDatas[i] = new List<Tag>();
        }
        // 
        int frameIndex = 0;
        for (int i = 0, len = defineSpriteTag.controlTags.Length; i < len; i++) {
            var tag = defineSpriteTag.controlTags[i];
            var tagType = (TagType)tag.header.type;
            if (tagType == TagType.End) break;
            if (tagType == TagType.ShowFrame) {
                frameIndex++;
                continue; // 不添加 ShowFrame
            }
            //Debug2.Log(tagType);
            frameDatas[frameIndex].Add(tag);
        }
        return frameDatas;
    }

    public void GotoFrame(int frameIndex) {
        frameIndex = Math.Clamp(frameIndex, 0, m_frameDatas.Length - 1);
        List<Tag> frameData = m_frameDatas[frameIndex];

        for (int i = 0, len = frameData.Count; i < len; i++) {
            var tag = frameData[i];
            LoadTag(tag);
        }
    }

    private void LoadTag(Tag tag) {
        var tagType = (TagType)tag.header.type;
        switch (tagType) {
            case TagType.ShowFrame:
                var showFrameTag = (ShowFrameTag)tag;

                break;
            case TagType.PlaceObject:
                var placeObjectTag = (PlaceObjectTag)tag;
                //placeObjectTag.characterId
                //placeObjectTag.depth
                //placeObjectTag.matrix
                //placeObjectTag.colorTransform
                break;
            case TagType.PlaceObject2:
                var placeObject2Tag = (PlaceObject2Tag)tag;
                if (placeObject2Tag.placeFlagHasCharacter) {
                    //placeObject2Tag.characterId
                }
                Debug.Log("placeObject2Tag.matrix:" + placeObject2Tag.matrix);
                break;
            case TagType.PlaceObject3:

                break;
            case TagType.RemoveObject:
                var removeobjectTag = (RemoveObjectTag)tag;

                break;
            case TagType.RemoveObject2:
                var removeobject2Tag = (RemoveObject2Tag)tag;

                break;
            case TagType.FrameLabel:
                var frameLabelTag = (FrameLabelTag)tag;

                break;
            case TagType.End:

                break;
        }
    }

}