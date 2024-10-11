[System.Serializable]
public struct TextRecord {

    public byte textRecordType;
    public byte styleFlagsReserved;
    public bool styleFlagsHasFont;
    public bool styleFlagsHasColor;
    public bool styleFlagsHasYOffset;
    public bool styleFlagsHasXOffset;
    public ushort fontID;
    public object textColor; // RGB | RGBA
    public short xOffset;
    public short yOffset;
    public ushort textHeight;
    public byte glyphCount;
    public GlyphEntryRecord[] glyphEntries;

    public TextRecord(SwfByteArray bytes, byte recordType, byte defineTextType, byte glyphBits, byte advanceBits) {
        textRecordType = recordType;
        styleFlagsReserved = (byte)bytes.ReadUB(3);
        styleFlagsHasFont = bytes.ReadFlag();
        styleFlagsHasColor = bytes.ReadFlag();
        styleFlagsHasYOffset = bytes.ReadFlag();
        styleFlagsHasXOffset = bytes.ReadFlag();
        if (styleFlagsHasFont) fontID = bytes.ReadUI16();
        else fontID = 0;
        if (styleFlagsHasColor) {
            if (defineTextType == 2) textColor = new RGBARecord(bytes);
            else textColor = new RGBRecord(bytes);
        } else {
            textColor = new RGBARecord();  // default value
        }
        if (styleFlagsHasXOffset) xOffset = bytes.ReadSI16();
        else xOffset = 0;
        if (styleFlagsHasYOffset) yOffset = bytes.ReadSI16();
        else yOffset = 0;
        if (styleFlagsHasFont) textHeight = bytes.ReadUI16();
        else textHeight = 0;
        glyphCount = bytes.ReadUI8();
        glyphEntries = new GlyphEntryRecord[glyphCount];
        for (var i = 0; i < glyphCount; i++) {
            glyphEntries[i] = new GlyphEntryRecord(bytes, glyphBits, advanceBits);
        }
    }
}