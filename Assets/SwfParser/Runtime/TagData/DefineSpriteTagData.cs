using System.Collections.Generic;

[System.Serializable]
public class DefineSpriteTagData : TagData {
    public ushort spriteId;
    public ushort frameCount;

    public TagTypeAndIndex[] tagTypeAndIndices;
}