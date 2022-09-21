using System.Collections.Generic;

public class DefineTextTag : SwfTag, ICharacterIdTag {

    public ushort characterID;
    public RectangleRecord textBounds;
    public MatrixRecord textMatrix;
    public byte glyphBits;
    public byte advanceBits;
    public TextRecord[] textRecords;
    public byte endOfRecordsFlag;


    public void GetNeededCharacterIds(List<ushort> characterIds, Swf swf) {
        if (characterIds.IndexOf(characterID) < 0) {
            characterIds.Add(characterID);
        }
    }
}