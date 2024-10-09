using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer)), DisallowMultipleComponent]
public class MovieClip : Sprite {

    [SerializeField] private SwfData m_swfData;
    [SerializeField] private string m_symbolClassName;
    private MeshFilter m_meshFilter;
    private MeshRenderer m_meshRenderer;
    private DefineSpritePlayer m_player;
    //
    public int currentFrame;
    public string currentFrameLabel;
    public string currentLabel;
    public string[] currentLabels;
    public bool isPlaying;
    public int totalFrames;


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


    private void Awake() {
        Debug.Log("MovieClip::Awake();");
        m_player = new DefineSpritePlayer(m_swfData, m_symbolClassName);


    }

}