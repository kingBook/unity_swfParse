using UnityEngine;
using System.Collections.Generic;

public class SwfReader {

    public Swf Read(SwfByteArray bytes) {
        var swf = new Swf(bytes);
        //
        TagFactory tagFactory = new TagFactory();
        while (bytes.GetBytesAvailable() > 0) {
            long preHeaderStart = bytes.GetBytePosition();
            TagHeaderRecord tagHeader = new TagHeaderRecord(bytes);

            long startPosition = bytes.GetBytePosition();
            long expectedEndPosition = startPosition + tagHeader.length;
            //Debug2.Log("type:"+tagHeader.type,"preHeaderStart:"+preHeaderStart,"length:"+tagHeader.length);
            SwfTag tag = tagFactory.CreateTag(tagFactory, this, bytes, tagHeader);
            swf.tags.Add(tag);
            if (tag is DefineSpriteTag defineSpriteTag) {
                swf.defineSpriteTags.Add(defineSpriteTag);
            } else if (tag is SymbolClassTag symbolClassTag) {
                swf.symbolClassTags.Add(symbolClassTag);
            }

            bytes.AlignBytes();
            //long newPosition = bytes.GetBytePosition();

            bytes.SetBytePosition(expectedEndPosition);

            if (tag is EndTag) {
                break;
            }
        }
        return swf;
    }

    public IShapeRecord ReadShapeRecord(SwfByteArray bytes, byte numFillBits, byte numLineBits, byte shapeType) {
        IShapeRecord record;
        bool typeFlag = bytes.ReadFlag();
        long start = bytes.GetBytePosition();
        if (!typeFlag) {
            bool stateNewStyles = bytes.ReadFlag();
            bool stateLineStyle = bytes.ReadFlag();
            bool stateFillStyle1 = bytes.ReadFlag();
            bool stateFillStyle0 = bytes.ReadFlag();
            bool stateMoveTo = bytes.ReadFlag();

            bool isEndShapeRecord = !stateNewStyles && !stateLineStyle && !stateFillStyle1 && !stateFillStyle0 && !stateMoveTo;
            if (isEndShapeRecord) {
                record = new EndShapeRecord(bytes, typeFlag);
            } else {
                record = new StyleChangeRecord(bytes, typeFlag, stateNewStyles, stateLineStyle,
                    stateFillStyle1, stateFillStyle0, stateMoveTo, numFillBits, numLineBits, shapeType);
            }
        } else {
            bool straightFlag = bytes.ReadFlag();
            if (straightFlag) {
                record = new StraightEdgeRecord(bytes, typeFlag, straightFlag);
            } else {
                record = new CurvedEdgeRecord(bytes, typeFlag, straightFlag);
            }
        }
        return record;
    }

}