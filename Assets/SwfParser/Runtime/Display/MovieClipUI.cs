using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasRenderer), typeof(RectTransform)), DisallowMultipleComponent]
public class MovieClipUI : MaskableGraphic {

    private Mesh m_mesh;

    protected override void OnValidate() {
        Debug.Log("MovieClipGraphic::OnValidate();");
        base.OnValidate();
        // enter code
    }

    protected override void Reset() {
        Debug.Log("MovieClipGraphic::Reset();");
        base.Reset();
        // enter code
    }

    protected override void Awake() {
        Debug.Log("MovieClipGraphic::Awake();");
        base.Awake();
        // enter code
        var mesh = new Mesh();
        m_mesh = mesh;

        mesh.SetVertices(new Vector3[]{
            new Vector3(0,0,0),
            new Vector3(1,0,0),
            new Vector3(1,1,0)
        });
        mesh.SetUVs(0, new Vector2[]{
            new Vector2(0,0),
            new Vector2(1,0),
            new Vector2(1,1)
        });
        mesh.SetTriangles(new int[] { 2, 1, 0 }, 0);
        mesh.SetNormals(new Vector3[]{
            Vector3.back,
            Vector3.back,
            Vector3.back
        });
    }

    protected override void OnEnable() {
        Debug.Log("MovieClipGraphic::OnEnable();");
        base.OnEnable();
    }

    protected override void Start() {
        Debug.Log("MovieClipGraphic::Start();");
        base.Start();
        //canvasRenderer.SetMesh(m_mesh);
    }

    public override void Rebuild(CanvasUpdate update) {
        Debug.Log("MovieClipGraphic::Rebuild();");
        base.Rebuild(update);
        canvasRenderer.SetMesh(m_mesh);
    }

    private void Update() {

    }

    private void LateUpdate() {

    }

    protected override void OnDisable() {
        Debug.Log("MovieClipGraphic::OnDisable();");
        base.OnDisable();
    }

    protected override void OnDestroy() {
        Debug.Log("MovieClipGraphic::OnDestroy();");
        base.OnDestroy();
    }





}