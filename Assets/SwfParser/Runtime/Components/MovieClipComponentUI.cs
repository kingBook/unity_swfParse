using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasRenderer), typeof(RectTransform)), DisallowMultipleComponent]
public class MovieClipComponentUI : MaskableGraphic {

    [SerializeField] private Swf m_swf;
    [SerializeField] private string m_symbolClassName;
    private MovieClip m_movieClip;

    private void Init() {
        var meshHelper = new MeshHelperUI(canvasRenderer);
        m_movieClip = new MovieClip(meshHelper, m_swf, m_symbolClassName);
    }

    protected override void OnValidate() {
        //Debug.Log("MovieClipGraphic::OnValidate();");
        base.OnValidate();
        // enter code
    }

    protected override void Reset() {
        //Debug.Log("MovieClipGraphic::Reset();");
        base.Reset();
        // enter code
    }

    protected override void Awake() {
        //Debug.Log("MovieClipGraphic::Awake();");
        base.Awake();
        // enter code
    }

    protected override void OnEnable() {
        //Debug.Log("MovieClipGraphic::OnEnable();");
        base.OnEnable();
    }

    protected override void Start() {
        //Debug.Log("MovieClipGraphic::Start();");
        base.Start();
    }

    public override void Rebuild(CanvasUpdate update) {
        //Debug.Log("MovieClipGraphic::Rebuild();");
        base.Rebuild(update);
        Init();
    }

    protected override void OnDisable() {
        //Debug.Log("MovieClipGraphic::OnDisable();");
        base.OnDisable();
    }

    protected override void OnDestroy() {
        //Debug.Log("MovieClipGraphic::OnDestroy();");
        base.OnDestroy();
    }





}