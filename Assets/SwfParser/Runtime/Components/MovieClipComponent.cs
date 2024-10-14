using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer)), DisallowMultipleComponent]
public class MovieClipComponent : MonoBehaviour {

    [SerializeField] private Swf m_swf;
    [SerializeField] private string m_symbolClassName;
    private MeshFilter m_meshFilter;
    private MeshRenderer m_meshRenderer;
    private MovieClip m_movieClip;
    
    private void Awake() {
        Debug.Log("MovieClip::Awake();");
        m_movieClip = new MovieClip(m_swf, m_symbolClassName);

        //m_meshRenderer.SetMaterials()
    }

}