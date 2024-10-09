﻿using System.Collections.Generic;

public class DefineTextTag : SwfTag, ICharacterIdTag {

    public ushort characterID;
    public RectangleRecord textBounds;
    public MatrixRecord textMatrix;
    public byte glyphBits;
    public byte advanceBits;
    public TextRecord[] textRecords;
    public byte endOfRecordsFlag;


    public DefineTextTag(SwfByteArray bytes, TagHeaderRecord header) : base(header) {
        characterID = bytes.ReadUI16();
        textBounds = new RectangleRecord(bytes);
        textMatrix = new MatrixRecord(bytes);
        glyphBits = bytes.ReadUI8();
        advanceBits = bytes.ReadUI8();

        var textRecords = new List<TextRecord>();
        while (true) {
            byte recordType = (byte)bytes.ReadUB(1);
            if (recordType == 0) {
                bytes.ReadUB(7);
                break;
            } else {
                textRecords.Add(new TextRecord(bytes, recordType, 1, glyphBits, advanceBits));
            }
        }
        this.textRecords = textRecords.ToArray();
        endOfRecordsFlag = 0;
    }

    public void GetNeededCharacterIds(List<ushort> characterIds, Swf swf) {
        if (characterIds.IndexOf(characterID) < 0) {
            characterIds.Add(characterID);
        }
    }

    public ushort GetCharacterId() {
        return characterID;
    }


}