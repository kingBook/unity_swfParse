public class DefineTextTag : SwfTag {

    public ushort characterID;
    public RectangleRecord textBounds;
    public MatrixRecord textMatrix;
    public byte glyphBits;
    public byte advanceBits;
    public TextRecord[] textRecords;
    public byte endOfRecordsFlag;
}