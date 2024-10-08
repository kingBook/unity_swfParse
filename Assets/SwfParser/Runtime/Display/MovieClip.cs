using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer)), DisallowMultipleComponent]
public class MovieClip : MonoBehaviour {

    [SerializeField] private SwfData m_swfData;
    [SerializeField] private string m_symbolClassName;
    //
    public int currentFrame;
    public string currentFrameLabel;
    public string currentLabel;
    public string[] currentLabels;
    public bool isPlaying;
    public int totalFrames;
    //
    private DefineSpriteTagData m_defineSpriteTagData;
    private Texture2D m_texture2D;

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
    /// 设置播放时使用的数据
    /// </summary>
    /// <param name="swfData"></param>
    /// <param name="defineSpriteTagData"></param>
    public void SeDatas(SwfData swfData, DefineSpriteTagData defineSpriteTagData) {
        //m_swfData = swfData;
        //m_defineSpriteTagData = defineSpriteTagData;
    }



    private void Awake() {
        Debug.Log("MovieClip::Awake();");
        var tagTypeAndIndex = m_swfData.GetTagTypeAndIndex(m_symbolClassName);
        DefineSpriteTagData defineSpriteTagData = (DefineSpriteTagData)m_swfData.GetTagData(tagTypeAndIndex);
        //Debug.Log(defineSpriteTagData.spriteId);

    }

    [ContextMenu("Test")]
    public void Test() {


    }

    // ============================================
    private void Init() {

    }

}