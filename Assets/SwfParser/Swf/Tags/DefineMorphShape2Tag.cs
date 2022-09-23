using System.Collections.Generic;

public class DefineMorphShape2Tag : SwfTag, ICharacterIdTag {

    public ushort characterId;
    public RectangleRecord startBounds;
    public RectangleRecord endBounds;
    public RectangleRecord startEdgeBounds;
    public RectangleRecord endEdgeBounds;
    public byte reserved;
    public bool usesNonScalingStrokes;
    public bool usesScalingStrokes;
    public uint offset;
    public MorphFillStyleArrayRecord morphFillStyles;
    public MorphLineStyleArrayRecord morphLineStyles;
    public SHAPE startEdges;
    public SHAPE endEdges;

    public void GetNeededCharacterIds(List<ushort> characterIds, Swf swf) {
        if (characterIds.IndexOf(characterId) < 0) {
            characterIds.Add(characterId);

            // bitmapId
            var fillStyles = morphFillStyles.fillStyles;
            for (int i = 0, len = fillStyles.Length; i < len; i++) {
                var fillStyle = fillStyles[i];
                var type = fillStyle.fillStyleType;
                if (type == 0x40 || type == 0x41 || type == 0x42 || type == 0x43) {
                    if (characterIds.IndexOf(fillStyle.bitmapId) < 0) {
                        characterIds.Add(fillStyle.bitmapId);
                    }
                }
            }
        }
    }

    public ushort GetCharacterId() {
        return characterId;
    }

}