using UnityEngine;

public class MeshHelper : MeshHelperBase {

    private MeshRenderer m_meshRenderer;

    public MeshHelper(MeshFilter meshFilter, MeshRenderer meshRenderer) {
        meshFilter.mesh = m_mesh;
        m_meshRenderer = meshRenderer;
    }

    public override void AddRectInfo(Texture2D atlas, RectInfo rectInfo, out int vertexStartIndex, out int vertexEndIndex, out int subMeshIndex) {
        base.AddRectInfo(atlas, rectInfo, out vertexStartIndex, out vertexEndIndex, out subMeshIndex);
        m_meshRenderer.SetMaterials(m_materials);
    }

};
