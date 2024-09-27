
/// <summary>
/// 该类配合 <see cref="SwfData"/> 类使用
/// <para> tagType: 表示标签类型，对应 <see cref="SwfData"/> 类中的标签列表 </para>
/// <para> index: 储存在 <see cref="SwfData"/> 类中的对应标签列表的索引号 </para>
/// </summary>
[System.Serializable]
public class TagTypeAndIndex {
    public uint tagType;
    public int index;
}