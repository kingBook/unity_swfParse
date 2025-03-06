using UnityEngine;

public class Shape : DisplayObject {

    //public Graphics graphics;

    public int vertexStartIndex { get; private set; }
    public int vertexEndIndex { get; private set; }
    public int subMeshIndex { get; private set; }

    public Shape(MeshHelperBase meshHelper, (Texture2D atlas, string atlasPath, RectInfo? rectInfo) atlasData, Matrix matrix) : base() {
        meshHelper.AddRectInfo(atlasData.atlas, atlasData.rectInfo.Value, matrix, out int vertexStartIndex, out int vertexEndIndex, out int subMeshIndex);
        this.vertexStartIndex = vertexStartIndex;
        this.vertexEndIndex = vertexEndIndex;
        this.subMeshIndex = subMeshIndex;


    }

}