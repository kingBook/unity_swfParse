using UnityEngine;
using System.Collections;

public struct TextRecord {

    public byte textRecordType;
    public byte styleFlagsReserved;
    public bool styleFlagsHasFont;
    public bool styleFlagsHasColor;
    public bool styleFlagsHasYOffset;
    public bool styleFlagsHasXOffset;
    public ushort fontID;
    public object textColor;
    public short xOffset;
    public short yOffset;
    public ushort textHeight;
    public byte glyphCount;
    public GlyphEntryRecord[] glyphEntries;
}