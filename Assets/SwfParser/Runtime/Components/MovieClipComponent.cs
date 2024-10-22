using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer)), DisallowMultipleComponent]
public class MovieClipComponent : MonoBehaviour {

    [SerializeField] private Swf m_swf;
    [SerializeField] private string m_symbolClassName;
    private MovieClip m_movieClip;

    private void Awake() {
        Debug.Log("MovieClip::Awake();");

        MeshFilter meshFilter = GetComponent<MeshFilter>();
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();

        var meshHelper = new MeshHelper(meshFilter, meshRenderer);
        m_movieClip = new MovieClip(meshHelper, m_swf, m_symbolClassName);
    }

}