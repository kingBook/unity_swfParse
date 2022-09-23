/// <summary>
/// 所有矢量图形标签使用的数据
/// <c> TagType.DefineShape || TagType.DefineShape2 || TagType.DefineShape3 || TagType.DefineShape4 </c>
/// </summary>
[System.Serializable]
public class DefineShapeTagData : TagData {

    public ushort shapeId;
    public ushort bitmapId;

}