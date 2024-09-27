using System.Collections.Generic;

[System.Serializable]
public class DefineSpriteTagData : TagData {
    public ushort spriteId;
    public ushort frameCount;

    /// <summary>
    /// 储存 DefineSpriteTag.controlTags 中标签的类型与及标签在 SwfData 的对应列表的索引
    /// <para> {tagType: 标签类型, index: 表示标签在 SwfData 中对应列表中的储存索引} </para>
    /// </summary>
    public TagTypeAndIndex[] tagTypeAndIndices;
}