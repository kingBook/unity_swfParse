using System.Collections.Generic;
using UnityEngine;

public abstract class MeshHelperBase {

    private readonly List<int> m_tempInts = new();

    protected readonly Mesh m_mesh = new();
    protected int m_subMeshCount;

    protected readonly List<Vector3> m_vertices = new();
    protected readonly List<Vector2> m_uvs = new();
    protected readonly List<Vector3> m_normals = new();
    protected readonly List<Material> m_materials = new();

    public virtual void AddRectInfo(Texture2D atlas, RectInfo rectInfo, out int vertexStartIndex, out int vertexEndIndex, out int subMeshIndex) {
        float w = rectInfo.rect.width * atlas.width;
        float h = rectInfo.rect.height * atlas.height;
        // 添加顶点 -------------------------------------
        vertexStartIndex = m_vertices.Count;
        vertexEndIndex = vertexStartIndex + 3;
        m_vertices.Add(new(0, 0, 0));
        m_vertices.Add(new(0, h, 0));
        m_vertices.Add(new(w, h, 0));
        m_vertices.Add(new(w, 0, 0));
        m_mesh.SetVertices(m_vertices);

        // UV -------------------------------------------
        m_uvs.Add(new(rectInfo.rect.x, rectInfo.rect.y));
        m_uvs.Add(new(rectInfo.rect.x, rectInfo.rect.y + rectInfo.rect.height));
        m_uvs.Add(new(rectInfo.rect.x + rectInfo.rect.width, rectInfo.rect.y + rectInfo.rect.height));
        m_uvs.Add(new(rectInfo.rect.x + rectInfo.rect.width, rectInfo.rect.y));
        m_mesh.SetUVs(0, m_uvs);

        // 法线 ------------------------------------------
        m_normals.Add(Vector3.back);
        m_normals.Add(Vector3.back);
        m_normals.Add(Vector3.back);
        m_normals.Add(Vector3.back);
        m_mesh.SetNormals(m_normals);

        // 三角形 ----------------------------------------
        m_tempInts.Add(0);
        m_tempInts.Add(1);
        m_tempInts.Add(3);

        m_tempInts.Add(1);
        m_tempInts.Add(2);
        m_tempInts.Add(3);
        subMeshIndex = m_subMeshCount;
        m_subMeshCount++;
        m_mesh.subMeshCount = m_subMeshCount;
        m_mesh.SetTriangles(m_tempInts, subMeshIndex);
        m_tempInts.Clear();
        // 材质 ------------------------------------------
        var material = new Material(Shader.Find("Sprites/Default"));
        material.mainTexture = atlas;
        m_materials.Add(material);
        // 由子类重写此方法，将材质应用到渲染器
    }



}