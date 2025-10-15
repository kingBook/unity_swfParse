using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class MovieClip : Sprite {

    public int currentFrame;
    public string currentFrameLabel;
    public string currentLabel;
    public string[] currentLabels;
    public bool isPlaying;
    public int totalFrames;

    //
    private Swf m_swf;
    private List<Tag>[] m_frameTags;
    private MeshHelperBase m_meshHelper;

    public MovieClip(Swf swf, MeshHelperBase meshHelper, string symbolClassName) : base() {
        // 获取与类名匹配的 DefineSpriteTag
        DefineSpriteTag defineSpriteTag = swf.GetUsedDefineSpriteTag(symbolClassName);
        Init(swf, meshHelper, defineSpriteTag);
    }

    public MovieClip(Swf swf, MeshHelperBase meshHelper, DefineSpriteTag defineSpriteTag) : base() {
        Init(swf, meshHelper, defineSpriteTag);
    }

    private void Init(Swf swf, MeshHelperBase meshHelper, DefineSpriteTag defineSpriteTag) {
        m_meshHelper = meshHelper;
        m_swf = swf;

        // 获取 defineSpriteTag 所有帧的 controlTags
        m_frameTags = GetFrameTags(defineSpriteTag);

        // 跳到指定帧
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

    /// <summary>
    /// 获取一个 DefineSpriteTag 所有帧的 controlTags
    /// </summary>
    /// <param name="defineSpriteTag"></param>
    /// <returns> 返回一个二维列表，一维：帧索引；二维：当前帧的 controlTags </returns>
    private List<Tag>[] GetFrameTags(DefineSpriteTag defineSpriteTag) {
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
            frameDatas[frameIndex].Add(tag);
        }
        return frameDatas;
    }

    /// <summary>
    /// 跳至指定帧
    /// </summary>
    /// <param name="frameIndex"> 帧索引 </param>
    public void GotoFrame(int frameIndex) {
        frameIndex = Math.Clamp(frameIndex, 0, m_frameTags.Length - 1);
        List<Tag> frameData = m_frameTags[frameIndex];

        for (int i = 0, len = frameData.Count; i < len; i++) {
            var tag = frameData[i];
            tag.Load(m_swf, m_meshHelper, this);
        }
    }

}