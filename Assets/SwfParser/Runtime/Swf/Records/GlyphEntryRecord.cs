[System.Serializable]
public struct GlyphEntryRecord {

    public uint glyphIndex;
    public int glyphAdvance;

    public GlyphEntryRecord(SwfByteArray bytes, byte glyphBits, byte advanceBits) {
        glyphIndex = bytes.ReadUB(glyphBits);
        glyphAdvance = bytes.ReadSB(advanceBits);
    }

}