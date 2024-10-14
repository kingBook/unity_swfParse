using System;
using System.Collections.Generic;
using UnityEngine;

public class MovieClip : Sprite {

    public int currentFrame;
    public string currentFrameLabel;
    public string currentLabel;
    public string[] currentLabels;
    public bool isPlaying;
    public int totalFrames;

    //
    private Swf m_swf;
    private List<Tag>[] m_frameDatas;

    public MovieClip(Swf swf, string symbolClassName) {
        m_swf = swf;

        DefineSpriteTag defineSpriteTag = swf.GetUsedDefineSpriteTag(symbolClassName);
        m_frameDatas = CreateFrameDatas(defineSpriteTag);
        GotoFrame(0);
    }

    /// <summary>
    /// 从指定帧开始播放。
    /// </summary>
    /// <param name="frame"></param>
    public void GotoAndPlay(int frame) {

    }

    /// <summary>
    /// 从指定帧标签开始播放。
    /// </summary>
    /// <param name="frameLabel"></param>
    public void GotoAndPlay(string frameLabel) {

    }

    /// <summary>
    /// 将播放头转到下一帧并停止。
    /// </summary>
    public void NextFrame() {

    }

    /// <summary>
    /// 在影片剪辑的时间轴中移动播放头。
    /// </summary>
    public void Play() {

    }

    /// <summary>
    /// 将播放头转到前一帧并停止。
    /// </summary>
    public void PrevFrame() {

    }

    /// <summary>
    /// 停止影片剪辑中的播放头。
    /// </summary>
    public void Stop() {

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
            tag.Load(m_swf, this);
        }
    }

}