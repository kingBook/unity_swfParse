using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer)), DisallowMultipleComponent]
public class TestMesh : MonoBehaviour {

    private void Awake() {
        var meshFilter = GetComponent<MeshFilter>();
        var meshRenderer = GetComponent<MeshRenderer>();

        var mesh = new Mesh();
        meshFilter.mesh = mesh;

        mesh.SetVertices(new Vector3[]{
            new (0,0,0),
            new (1,0,0),
            new (1,1,0),
            //
            new (0.5f,0.5f,0),
            new (2,2,0),
            new (2,0.5f,0)
        });
        mesh.SetUVs(0, new Vector2[]{
            new (0,0),
            new (1,0),
            new (1,1),
            new (0,0),
            new (1,0),
            new (1,1)
        });
        mesh.subMeshCount = 2;
        mesh.SetTriangles(new int[] { 2, 1, 0 }, 0);
        mesh.SetTriangles(new int[] { 3, 4, 5 }, 1);
        mesh.SetNormals(new Vector3[]{
            Vector3.back,
            Vector3.back,
            Vector3.back,

            Vector3.back,
            Vector3.back,
            Vector3.back
        });

    }

}
