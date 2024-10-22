using UnityEngine;

public class MeshHelperUI : MeshHelperBase {

    public MeshHelperUI(CanvasRenderer canvasRenderer) : base() {
        var mesh = new Mesh();
        // mesh.SetVertices(new Vector3[]{
        //     new Vector3(0,0,0),
        //     new Vector3(1,0,0),
        //     new Vector3(1,1,0)
        // });
        // mesh.SetUVs(0, new Vector2[]{
        //     new Vector2(0,0),
        //     new Vector2(1,0),
        //     new Vector2(1,1)
        // });
        // mesh.SetTriangles(new int[] { 2, 1, 0 }, 0);
        // mesh.SetNormals(new Vector3[]{
        //     Vector3.back,
        //     Vector3.back,
        //     Vector3.back
        // });
        canvasRenderer.SetMesh(mesh);

        //canvasRenderer.SetMaterial(material, 0);
    }

    
}