using UnityEngine;
using System.Collections.Generic;

public class SwfReader {

    public Swf Read(SwfByteArray bytes) {
        var swf = new Swf(bytes);
        //
        while (bytes.GetBytesAvailable() > 0) {
            long preHeaderStart = bytes.GetBytePosition();
            TagHeaderRecord tagHeader = new TagHeaderRecord(bytes);

            long startPosition = bytes.GetBytePosition();
            long expectedEndPosition = startPosition + tagHeader.length;
            //Debug2.Log("type:"+tagHeader.type,"preHeaderStart:"+preHeaderStart,"length:"+tagHeader.length);
            SwfTag tag = ReadTag(bytes, tagHeader);
            tag.header = tagHeader;
            swf.tags.Add(tag);
            if (tag is DefineSpriteTag defineSpriteTag) {
                swf.defineSpriteTags.Add(defineSpriteTag);
            } else if (tag is SymbolClassTag symbolClassTag) {
                swf.symbolClassTags.Add(symbolClassTag);
            }

            bytes.AlignBytes();
            long newPosition = bytes.GetBytePosition();

            bytes.SetBytePosition(expectedEndPosition);

            if (tag is EndTag) {
                break;
            }
        }
        return swf;
    }

    public SwfTag[] ReadControlTags(SwfByteArray bytes) {
        var tempTags = new List<SwfTag>();
        while (true) {
            var header = new TagHeaderRecord(bytes);
            long startPosition = bytes.GetBytePosition();
            long expectedEndPosition = startPosition + header.length;
            var tag = ReadTag(bytes, header);
            tempTags.Add(tag);
            bytes.SetBytePosition(expectedEndPosition);
            if (tag is EndTag) break;
        }
        return tempTags.ToArray();
    }

    public SwfTag ReadTag(SwfByteArray bytes, TagHeaderRecord header) {
        SwfTag tag;
        switch (header.type) {
            //============= Display list tags =======
            case 4:
                tag = new PlaceObjectTag(bytes, header);
                break;
            case 26:
                tag = new PlaceObject2Tag(bytes, header);
                break;
            case 70:
                tag = new PlaceObject3Tag(bytes, header);
                break;
            case 5:
                tag = new RemoveObjectTag(bytes, header);
                break;
            case 28:
                tag = new RemoveObject2Tag(bytes, header);
                break;
            case 1:
                tag = new ShowFrameTag(bytes, header);
                break;
            //============= Control Tags =======
            case 9:
                tag = new SetBackgroundColorTag(bytes, header);
                break;
            case 43:
                tag = new FrameLabelTag(bytes, header);
                break;
            case 24:
                tag = new ProtectTag(bytes, header);
                break;
            case 0:
                tag = new EndTag(bytes, header);
                break;
            case 56:
                tag = new ExportAssetsTag(bytes, header);
                break;
            case 64:
                tag = new EnableDubugger2Tag(bytes, header);
                break;
            case 65:
                tag = new ScriptLimitsTag(bytes, header);
                break;
            case 66:
                tag = new SetTabIndexTag(bytes, header);
                break;
            case 69:
                tag = new FileAttributesTag(bytes, header);
                break;
            case 71:
                tag = new ImportAssets2Tag(bytes, header);
                break;
            case 76:
                tag = new SymbolClassTag(bytes, header);
                break;
            case 77:
                tag = new MetadataTag(bytes, header);
                break;
            case 78:
                tag = new DefineScalingGridTag(bytes, header);
                break;
            case 86:
                tag = new DefineSceneAndFrameLabelDataTag(bytes, header);
                break;
            //============= Shape Tags =======
            case 2:
                tag = new DefineShapeTag(this, bytes, header);
                break;
            case 22:
                tag = new DefineShape2Tag(this, bytes, header);
                break;
            case 32:
                tag = new DefineShape3Tag(this, bytes, header);
                break;
            case 83:
                tag = new DefineShape4Tag(this, bytes, header);
                break;
            //============= Bitmaps =======
            case 6:
                tag = new DefineBitsTag(bytes, header);
                break;
            case 8:
                tag = new JPEGTablesTag(bytes, header);
                break;
            case 21:
                tag = new DefineBitsJPEG2Tag(bytes, header);
                break;
            case 35:
                tag = new DefineBitsJPEG3Tag(bytes, header);
                break;
            case 20:
                tag = new DefineBitsLosslessTag(bytes, header);
                break;
            case 36:
                tag = new DefineBitsLossless2Tag(bytes, header);
                break;
            case 90:
                tag = new DefineBitsJPEG4Tag(bytes, header);
                break;
            //============= Shape Morphing =======
            case 46:
                tag = new DefineMorphShapeTag(this, bytes, header);
                break;
            case 84:
                tag = new UnknownTag(bytes, header);
                Debug.LogError("DefineMorphShape2Tag is not implemented.");
                //tag = new DefineMorphShape2Tag(this, bytes,header);
                break;
            //============= Fonts and Text =======
            case 10:
                tag = new DefineFontTag(bytes, header);
                break;
            case 11:
                tag = new DefineTextTag(bytes, header);
                break;
            case 33:
                tag = new DefineText2Tag(bytes, header);
                break;
            //============= Buttons =======
            case 7:
                tag = new UnknownTag(bytes, header);
                Debug.LogError("DefineButtonTag is not implemented.");
                //tag = new DefineButtonTag(bytes,header);
                break;
            case 34:
                tag = new DefineButton2Tag(bytes, header);
                break;
            //============= Sprites and Movie Clips =======
            case 39:
                tag = new DefineSpriteTag(this, bytes, header);
                break;

            default:
                tag = new UnknownTag(bytes, header);
                break;
        }
        return tag;
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