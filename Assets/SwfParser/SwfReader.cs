using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class SwfReader {

    public Swf Read(SwfByteArray bytes) {
        var swf = new Swf();
        swf.header = ReadSwfHeader(bytes);
        swf.tags = new List<SwfTag>();
        //
        while (bytes.GetBytesAvailable() > 0) {
            long preHeaderStart = bytes.GetBytePosition();
            TagHeaderRecord tagHeader = ReadTagHeaderRecord(bytes);

            long startPosition = bytes.GetBytePosition();
            long expectedEndPosition = startPosition + tagHeader.length;
            //Debug2.Log("type:"+tagHeader.type,"preHeaderStart:"+preHeaderStart,"length:"+tagHeader.length);
            SwfTag tag = ReadTag(bytes, tagHeader);
            tag.header = tagHeader;
            swf.tags.Add(tag);

            bytes.AlignBytes();
            long newPosition = bytes.GetBytePosition();

            bytes.SetBytePosition(expectedEndPosition);

            if (tag is EndTag) {
                break;
            }
        }
        return swf;
    }

    private SwfTag[] ReadControlTags(SwfByteArray bytes) {
        var tags = new List<SwfTag>();
        while (true) {
            var header = ReadTagHeaderRecord(bytes);
            long startPosition = bytes.GetBytePosition();
            long expectedEndPosition = startPosition + header.length;
            var tag = ReadTag(bytes, header);
            tag.header = header;
            tags.Add(tag);
            bytes.SetBytePosition(expectedEndPosition);
            if (tag is EndTag) break;
        }
        return tags.ToArray();
    }

    private SwfTag ReadTag(SwfByteArray bytes, TagHeaderRecord header) {
        SwfTag tag;
        switch (header.type) {
            //============= Display list tags =======
            case 4:
                tag = ReadPlaceObjectTag(bytes, header);
                break;
            case 26:
                tag = ReadPlaceObject2Tag(bytes, header);
                break;
            case 70:
                tag = ReadPlaceObject3Tag(bytes, header);
                break;
            case 5:
                tag = ReadRemoveObjectTag(bytes, header);
                break;
            case 28:
                tag = ReadRemoveObject2Tag(bytes, header);
                break;
            case 1:
                tag = ReadShowFrameTag(bytes, header);
                break;
            //============= Control Tags =======
            case 9:
                tag = ReadSetBackgroundColorTag(bytes, header);
                break;
            case 43:
                tag = ReadFrameLabelTag(bytes, header);
                break;
            case 24:
                tag = ReadProtectTag(bytes, header);
                break;
            case 0:
                tag = ReadEndTag(bytes, header);
                break;
            case 56:
                tag = ReadExportAssetsTag(bytes, header);
                break;
            case 64:
                tag = ReadEnableDubugger2(bytes, header);
                break;
            case 65:
                tag = ReadScriptLimitsTag(bytes, header);
                break;
            case 66:
                tag = ReadSetTabIndexTag(bytes, header);
                break;
            case 69:
                tag = ReadFileAttributesTag(bytes, header);
                break;
            case 71:
                tag = ReadImportAssets2Tag(bytes, header);
                break;
            case 76:
                tag = ReadSymbolClassTag(bytes, header);
                break;
            case 77:
                tag = ReadMetadataTag(bytes, header);
                break;
            case 78:
                tag = ReadDefineScalingGridTag(bytes, header);
                break;
            case 86:
                tag = ReadDefineSceneAndFrameLabelDataTag(bytes, header);
                break;
            //============= Shape Tags =======
            case 2:
                tag = ReadDefineShapeTag(bytes, header);
                break;
            case 22:
                tag = ReadDefineShape2Tag(bytes, header);
                break;
            case 32:
                tag = ReadDefineShape3Tag(bytes, header);
                break;
            case 83:
                tag = ReadDefineShape4Tag(bytes, header);
                break;
            //============= Bitmaps =======
            case 6:
                tag = ReadDefineBitsTag(bytes, header);
                break;
            case 8:
                tag = ReadJPEGTablesTag(bytes, header);
                break;
            case 21:
                tag = ReadDefineBitsJPEG2Tag(bytes, header);
                break;
            case 35:
                tag = ReadDefineBitsJPEG3Tag(bytes, header);
                break;
            case 20:
                tag = ReadDefineBitsLosslessTag(bytes, header);
                break;
            case 36:
                tag = ReadDefineBitsLossless2Tag(bytes, header);
                break;
            case 90:
                tag = ReadDefineBitsJPEG4Tag(bytes, header);
                break;
            //============= Shape Morphing =======
            case 46:
                tag = ReadDefineMorphShapeTag(bytes, header);
                break;
            /*case 84:
                tag=readDefineMorphShape2Tag(bytes,header);
                break;*/
            //============= Fonts and Text =======
            case 10:
                tag = ReadDefineFontTag(bytes, header);
                break;
            case 11:
                tag = ReadDefineTextTag(bytes, header);
                break;
            case 33:
                tag = ReadDefineText2Tag(bytes, header);
                break;
            //============= Buttons =======
            /*case 7:
                tag=readDefineButtonTag(bytes,header);
                break;*/
            case 34:
                tag = ReadDefineButton2Tag(bytes, header);
                break;
            //============= Sprites and Movie Clips =======
            case 39:
                tag = ReadDefineSpriteTag(bytes, header);
                break;

            default:
                tag = ReadUnknownTag(bytes, header);
                break;
        }
        return tag;
    }

    private PlaceObjectTag ReadPlaceObjectTag(SwfByteArray bytes, TagHeaderRecord header) {
        var tag = new PlaceObjectTag();
        var originalPos = bytes.GetBytePosition();
        tag.characterId = bytes.ReadUI16();
        tag.depth = bytes.ReadUI16();
        tag.matrix = ReadMatrixRecord(bytes);
        if (header.length > bytes.GetBytePosition() - originalPos) {
            tag.colorTransform = ReadCXFormRecord(bytes);
        }
        return tag;
    }

    private PlaceObject2Tag ReadPlaceObject2Tag(SwfByteArray bytes, TagHeaderRecord header) {
        var tag = new PlaceObject2Tag();
        tag.placeFlagHasClipActions = bytes.ReadFlag();
        tag.placeFlagHasClipDepth = bytes.ReadFlag();
        tag.placeFlagHasName = bytes.ReadFlag();
        tag.placeFlagHasRatio = bytes.ReadFlag();
        tag.placeFlagHasColorTransform = bytes.ReadFlag();
        tag.placeFlagHasMatrix = bytes.ReadFlag();
        tag.placeFlagHasCharacter = bytes.ReadFlag();
        tag.placeFlagMove = bytes.ReadFlag();
        tag.depth = bytes.ReadUI16();
        if (tag.placeFlagHasCharacter) {
            tag.characterId = bytes.ReadUI16();
        }
        if (tag.placeFlagHasMatrix) {
            tag.matrix = ReadMatrixRecord(bytes);
        }
        if (tag.placeFlagHasColorTransform) {
            tag.colorTransform = ReadCXFormWithAlphaRecord(bytes);
        }
        if (tag.placeFlagHasRatio) {
            tag.ratio = bytes.ReadUI16();
        }
        if (tag.placeFlagHasName) {
            tag.name = bytes.ReadString();
        }
        if (tag.placeFlagHasClipDepth) {
            tag.clipDepth = bytes.ReadUI16();
        }
        /*if(tag.placeFlagHasClipActions){
            tag.clipActions=
        }*/
        return tag;
    }

    private PlaceObject3Tag ReadPlaceObject3Tag(SwfByteArray bytes, TagHeaderRecord header) {
        var tag = new PlaceObject3Tag();
        tag.placeFlagHasClipActions = bytes.ReadFlag();
        tag.placeFlagHasClipDepth = bytes.ReadFlag();
        tag.placeFlagHasName = bytes.ReadFlag();
        tag.placeFlagHasRatio = bytes.ReadFlag();
        tag.placeFlagHasColorTransform = bytes.ReadFlag();
        tag.placeFlagHasMatrix = bytes.ReadFlag();
        tag.placeFlagHasCharacter = bytes.ReadFlag();
        tag.placeFlagMove = bytes.ReadFlag();
        tag.reserved = (byte)bytes.ReadUB(1);
        tag.placeFlagOpaqueBackground = bytes.ReadFlag();
        tag.placeFlagHasVisible = bytes.ReadFlag();
        tag.placeFlagHasImage = bytes.ReadFlag();
        tag.placeFlagHasClassName = bytes.ReadFlag();
        tag.placeFlagHasCacheAsBitmap = bytes.ReadFlag();
        tag.placeFlagHasBlendMode = bytes.ReadFlag();
        tag.placeFlagHasFilterList = bytes.ReadFlag();
        tag.depth = bytes.ReadUI16();
        if (tag.placeFlagHasClassName || (tag.placeFlagHasImage && tag.placeFlagHasCharacter)) {
            tag.className = bytes.ReadString();
        }
        if (tag.placeFlagHasCharacter) {
            tag.characterId = bytes.ReadUI16();
        }
        if (tag.placeFlagHasMatrix) {
            tag.matrix = ReadMatrixRecord(bytes);
        }
        if (tag.placeFlagHasColorTransform) {
            tag.colorTransform = ReadCXFormWithAlphaRecord(bytes);
        }
        if (tag.placeFlagHasRatio) {
            tag.ratio = bytes.ReadUI16();
        }
        if (tag.placeFlagHasName) {
            tag.name = bytes.ReadString();
        }
        if (tag.placeFlagHasClipDepth) {
            tag.clipDepth = bytes.ReadUI16();
        }
        if (tag.placeFlagHasFilterList) {
            tag.surfaceFilterList = ReadFilterListRecord(bytes);
        }
        if (tag.placeFlagHasBlendMode) {
            tag.blendMode = bytes.ReadUI8();
        }
        if (tag.placeFlagHasCacheAsBitmap) {
            tag.bitmapCache = bytes.ReadUI8();
        }
        if (tag.placeFlagHasVisible) {
            tag.visible = bytes.ReadUI8();
            tag.backgroundColor = ReadRGBARecord(bytes);
        }
        /*if(tag.placeFlagHasClipActions){
            tag.placeFlagHasClipActions=
        }*/
        return tag;
    }

    private RemoveObjectTag ReadRemoveObjectTag(SwfByteArray bytes, TagHeaderRecord header) {
        var tag = new RemoveObjectTag();
        tag.characterId = bytes.ReadUI16();
        tag.depth = bytes.ReadUI16();
        return tag;
    }

    private RemoveObject2Tag ReadRemoveObject2Tag(SwfByteArray bytes, TagHeaderRecord header) {
        var tag = new RemoveObject2Tag();
        tag.depth = bytes.ReadUI16();
        return tag;
    }

    private ShowFrameTag ReadShowFrameTag(SwfByteArray bytes, TagHeaderRecord header) {
        var tag = new ShowFrameTag();
        return tag;
    }

    private SetBackgroundColorTag ReadSetBackgroundColorTag(SwfByteArray bytes, TagHeaderRecord header) {
        var tag = new SetBackgroundColorTag();
        tag.backgroundColor = ReadRGBRecord(bytes);
        return tag;
    }

    private FrameLabelTag ReadFrameLabelTag(SwfByteArray bytes, TagHeaderRecord header) {
        var tag = new FrameLabelTag();
        tag.name = bytes.ReadString();
        tag.namedAnchorFlag = bytes.ReadUI8();
        return tag;
    }

    private ProtectTag ReadProtectTag(SwfByteArray bytes, TagHeaderRecord header) {
        var tag = new ProtectTag();
        return tag;
    }

    private EndTag ReadEndTag(SwfByteArray bytes, TagHeaderRecord header) {
        var tag = new EndTag();
        return tag;
    }

    private ExportAssetsTag ReadExportAssetsTag(SwfByteArray bytes, TagHeaderRecord header) {
        var tag = new ExportAssetsTag();
        ushort count = bytes.ReadUI16();
        var list = new ExportAssetRecord[count];
        for (ushort i = 0; i < count; i++) {
            var record = new ExportAssetRecord();
            record.tag = bytes.ReadUI16();
            record.name = bytes.ReadString();
            list[i] = record;
        }
        tag.list = list;
        return tag;
    }

    private EnableDubugger2Tag ReadEnableDubugger2(SwfByteArray bytes, TagHeaderRecord header) {
        var tag = new EnableDubugger2Tag();
        tag.reserved = bytes.ReadUI16();
        tag.password = bytes.ReadString();
        return tag;
    }

    private ScriptLimitsTag ReadScriptLimitsTag(SwfByteArray bytes, TagHeaderRecord header) {
        var tag = new ScriptLimitsTag();
        tag.maxRecursionDepth = bytes.ReadUI16();
        tag.scriptTimeoutSeconds = bytes.ReadUI16();
        return tag;
    }

    private SetTabIndexTag ReadSetTabIndexTag(SwfByteArray bytes, TagHeaderRecord header) {
        var tag = new SetTabIndexTag();
        tag.depth = bytes.ReadUI16();
        tag.tabIndex = bytes.ReadUI16();
        return tag;
    }

    private FileAttributesTag ReadFileAttributesTag(SwfByteArray bytes, TagHeaderRecord header) {
        var tag = new FileAttributesTag();
        bytes.ReadUB(1);
        tag.useDirectBlit = bytes.ReadFlag();
        tag.useGPU = bytes.ReadFlag();
        tag.hasMetadata = bytes.ReadFlag();
        tag.actionScript3 = bytes.ReadFlag();
        bytes.ReadUB(2);
        tag.useNetwork = bytes.ReadFlag();
        bytes.ReadUB(24);
        return tag;
    }

    private ImportAssets2Tag ReadImportAssets2Tag(SwfByteArray bytes, TagHeaderRecord header) {
        var tag = new ImportAssets2Tag();
        bytes.ReadUI8();
        bytes.ReadUI8();
        ushort count = bytes.ReadUI16();
        var list = new ImportAssets2Record[count];
        for (ushort i = 0; i < count; i++) {
            var record = new ImportAssets2Record();
            record.tag = bytes.ReadUI16();
            record.name = bytes.ReadString();
            list[i] = record;
        }
        tag.list = list;
        return tag;
    }

    private SymbolClassTag ReadSymbolClassTag(SwfByteArray bytes, TagHeaderRecord header) {
        var tag = new SymbolClassTag();
        tag.numSymbols = bytes.ReadUI16();
        var list = new SymbolClassRecord[tag.numSymbols];
        for (ushort i = 0; i < tag.numSymbols; i++) {
            var record = new SymbolClassRecord();
            record.tag = bytes.ReadUI16();
            record.name = bytes.ReadString();
            list[i] = record;
        }
        tag.list = list;
        return tag;
    }

    private MetadataTag ReadMetadataTag(SwfByteArray bytes, TagHeaderRecord header) {
        var tag = new MetadataTag();
        tag.metadata = bytes.ReadString();
        return tag;
    }

    private DefineScalingGridTag ReadDefineScalingGridTag(SwfByteArray bytes, TagHeaderRecord header) {
        var tag = new DefineScalingGridTag();
        tag.characterId = bytes.ReadUI16();
        tag.splitter = ReadRectangleRecord(bytes);
        return tag;
    }

    private DefineSceneAndFrameLabelDataTag ReadDefineSceneAndFrameLabelDataTag(SwfByteArray bytes, TagHeaderRecord header) {
        var tag = new DefineSceneAndFrameLabelDataTag();
        tag.sceneCount = bytes.ReadEncodedUI32();
        var defineSceneList = new DefineSceneRecord[tag.sceneCount];
        for (uint i = 0; i < tag.sceneCount; i++) {
            var record = new DefineSceneRecord();
            record.offset = bytes.ReadEncodedUI32();
            record.name = bytes.ReadString();
            defineSceneList[i] = record;
        }
        tag.defineSceneList = defineSceneList;

        tag.frameLabelCount = bytes.ReadEncodedUI32();
        var frameLabelList = new FramelabelRecord[tag.frameLabelCount];
        for (uint i = 0; i < tag.frameLabelCount; i++) {
            var record = new FramelabelRecord();
            record.frameNum = bytes.ReadEncodedUI32();
            record.frameLabel = bytes.ReadString();
            frameLabelList[i] = record;
        }
        tag.frameLabelList = frameLabelList;
        return tag;
    }

    private DefineShapeTag ReadDefineShapeTag(SwfByteArray bytes, TagHeaderRecord header) {
        var tag = new DefineShapeTag();
        tag.shapeId = bytes.ReadUI16();
        tag.shapeBounds = ReadRectangleRecord(bytes);
        tag.shapes = ReadShapeWithStyleRecord(bytes, 1);
        return tag;
    }

    private DefineShape2Tag ReadDefineShape2Tag(SwfByteArray bytes, TagHeaderRecord header) {
        var tag = new DefineShape2Tag();
        tag.shapeId = bytes.ReadUI16();
        tag.shapeBounds = ReadRectangleRecord(bytes);
        tag.shapes = ReadShapeWithStyleRecord(bytes, 2);
        return tag;
    }

    private DefineShape3Tag ReadDefineShape3Tag(SwfByteArray bytes, TagHeaderRecord header) {
        var tag = new DefineShape3Tag();
        tag.shapeId = bytes.ReadUI16();
        tag.shapeBounds = ReadRectangleRecord(bytes);
        tag.shapes = ReadShapeWithStyleRecord(bytes, 3);
        return tag;
    }

    private DefineShape4Tag ReadDefineShape4Tag(SwfByteArray bytes, TagHeaderRecord header) {
        var tag = new DefineShape4Tag();
        tag.shapeId = bytes.ReadUI16();
        tag.shapeBounds = ReadRectangleRecord(bytes);
        tag.edgeBounds = ReadRectangleRecord(bytes);
        tag.reserved = (byte)bytes.ReadUB(5);
        tag.usesFillWindingRule = bytes.ReadFlag();
        tag.usesNonScalingStrokes = bytes.ReadFlag();
        tag.usesScalingStrokes = bytes.ReadFlag();
        tag.shapes = ReadShapeWithStyleRecord(bytes, 4);
        return tag;
    }

    private DefineBitsTag ReadDefineBitsTag(SwfByteArray bytes, TagHeaderRecord header) {
        var tag = new DefineBitsTag();
        tag.characterID = bytes.ReadUI16();
        int length = (int)header.length - 2;
        if (length > 0) {
            tag.jpegData = bytes.ReadBytes(length);
        }
        return tag;
    }

    private JPEGTablesTag ReadJPEGTablesTag(SwfByteArray bytes, TagHeaderRecord header) {
        var tag = new JPEGTablesTag();
        int length = (int)header.length;
        if (length > 0) {
            tag.jpegData = bytes.ReadBytes(length);
        }
        return tag;
    }

    private DefineBitsJPEG2Tag ReadDefineBitsJPEG2Tag(SwfByteArray bytes, TagHeaderRecord header) {
        var tag = new DefineBitsJPEG2Tag();
        tag.characterID = bytes.ReadUI16();
        int length = (int)header.length - 2;
        if (length > 0) {
            tag.imageData = bytes.ReadBytes(length);
        }
        return tag;
    }

    private DefineBitsJPEG3Tag ReadDefineBitsJPEG3Tag(SwfByteArray bytes, TagHeaderRecord header) {
        var tag = new DefineBitsJPEG3Tag();
        long startPosition = bytes.GetBytePosition();
        tag.characterID = bytes.ReadUI16();
        tag.alphaDataOffset = bytes.ReadUI32();
        if (tag.alphaDataOffset > 0) {
            tag.imageData = bytes.ReadBytes((int)tag.alphaDataOffset);
        }
        int bytesRemaining = (int)(header.length - (bytes.GetBytePosition() - startPosition));
        if (bytesRemaining > 0) {
            tag.bitmapAlphaData = bytes.ReadBytes(bytesRemaining);
        }
        return tag;
    }

    private DefineBitsLosslessTag ReadDefineBitsLosslessTag(SwfByteArray bytes, TagHeaderRecord header) {
        long startPosition = bytes.GetBytePosition();
        var tag = new DefineBitsLosslessTag();
        tag.characterID = bytes.ReadUI16();
        tag.bitmapFormat = bytes.ReadUI8();
        tag.bitmapWidth = bytes.ReadUI16();
        tag.bitmapHeight = bytes.ReadUI16();
        if (tag.bitmapFormat == 3) {
            tag.bitmapColorTableSize = bytes.ReadUI8();
        }
        if (tag.bitmapFormat == 3 || tag.bitmapFormat == 4 || tag.bitmapFormat == 5) {
            byte[] unzippedData = null;
            long bytesRead = bytes.GetBytePosition() - startPosition;
            int remainingBytes = (int)(header.length - bytesRead);
            if (remainingBytes > 0) {
                unzippedData = bytes.ReadBytes(remainingBytes);
            }
            unzippedData = ZlibUtil.DeCompressBytes(unzippedData);
            var unzippedSwfArray = new SwfByteArray(unzippedData);
            if (tag.bitmapFormat == 3) {
                uint imageDataSize = (uint)(tag.bitmapWidth * tag.bitmapHeight);
                tag.zlibBitmapData = ReadColorMapDataRecord(unzippedSwfArray, (uint)(tag.bitmapColorTableSize + 1), imageDataSize);
            } else if (tag.bitmapFormat == 4 || tag.bitmapFormat == 5) {
                uint imageDataSize = (uint)(tag.bitmapWidth * tag.bitmapHeight);
                tag.zlibBitmapData = ReadBitmapDataRecord(unzippedSwfArray, tag.bitmapFormat, imageDataSize);
            }
        }
        return tag;
    }

    private DefineBitsLossless2Tag ReadDefineBitsLossless2Tag(SwfByteArray bytes, TagHeaderRecord header) {
        long startPosition = bytes.GetBytePosition();
        var tag = new DefineBitsLossless2Tag();
        tag.characterID = bytes.ReadUI16();
        tag.bitmapFormat = bytes.ReadUI8();
        tag.bitmapWidth = bytes.ReadUI16();
        tag.bitmapHeight = bytes.ReadUI16();
        if (tag.bitmapFormat == 3) {
            tag.bitmapColorTableSize = bytes.ReadUI8();
        }
        if (tag.bitmapFormat == 3 || tag.bitmapFormat == 4 || tag.bitmapFormat == 5) {
            byte[] unzippedData = null;
            long bytesRead = bytes.GetBytePosition() - startPosition;
            int remainingBytes = (int)(header.length - bytesRead);
            if (remainingBytes > 0) {
                unzippedData = bytes.ReadBytes(remainingBytes);
            }
            unzippedData = ZlibUtil.DeCompressBytes(unzippedData);
            var unzippedSwfArray = new SwfByteArray(unzippedData);
            if (tag.bitmapFormat == 3) {
                uint bitmapWidth = tag.bitmapWidth;
                while ((bitmapWidth%4)!=0) {
                    bitmapWidth = (bitmapWidth / 4 + 1) * 4;
                }
                uint imageDataSize = bitmapWidth * tag.bitmapHeight;
                tag.zlibBitmapData = ReadAlphaColorMapDataRecord(unzippedSwfArray, (uint)(tag.bitmapColorTableSize + 1), imageDataSize);
            } else if (tag.bitmapFormat == 4 || tag.bitmapFormat == 5) {
                uint imageDataSize = (uint)(tag.bitmapWidth * tag.bitmapHeight);
                tag.zlibBitmapData = ReadAlphaBitmapDataRecord(unzippedSwfArray, imageDataSize);
            }
            unzippedSwfArray.Close();
        }
        return tag;
    }

    private DefineBitsJPEG4Tag ReadDefineBitsJPEG4Tag(SwfByteArray bytes, TagHeaderRecord header) {
        var tag = new DefineBitsJPEG4Tag();
        long startPosition = bytes.GetBytePosition();
        tag.characterID = bytes.ReadUI16();
        tag.alphaDataOffset = bytes.ReadUI32();
        tag.deblockParam = bytes.ReadFixed8_8();
        if (tag.alphaDataOffset > 0) {
            tag.imageData = bytes.ReadBytes((int)tag.alphaDataOffset);
        }
        int bytesRemaining = (int)(header.length - (bytes.GetBytePosition() - startPosition));
        if (bytesRemaining > 0) {
            tag.bitmapAlphaData = bytes.ReadBytes(bytesRemaining);
        }
        return tag;
    }

    private DefineMorphShapeTag ReadDefineMorphShapeTag(SwfByteArray bytes, TagHeaderRecord header) {
        var tag = new DefineMorphShapeTag();
        tag.characterId = bytes.ReadUI16();
        tag.startBounds = ReadRectangleRecord(bytes);
        tag.endBounds = ReadRectangleRecord(bytes);
        tag.offset = bytes.ReadUI32();
        tag.morphFillStyles = ReadMorphFillStyleArrayRecord(bytes);
        tag.morphLineStyles = ReadMorphLineStyleArrayRecord(bytes, 1);
        tag.startEdges = ReadSHAPE(bytes, 1);
        tag.endEdges = ReadSHAPE(bytes, 1);
        return tag;
    }

    private DefineMorphShape2Tag ReadDefineMorphShape2Tag(SwfByteArray bytes, TagHeaderRecord header) {
        var tag = new DefineMorphShape2Tag();
        tag.characterId = bytes.ReadUI16();
        tag.startBounds = ReadRectangleRecord(bytes);
        tag.endBounds = ReadRectangleRecord(bytes);
        tag.startEdgeBounds = ReadRectangleRecord(bytes);
        tag.endEdgeBounds = ReadRectangleRecord(bytes);
        tag.reserved = (byte)bytes.ReadUB(6);
        tag.usesNonScalingStrokes = bytes.ReadFlag();
        tag.usesScalingStrokes = bytes.ReadFlag();
        tag.offset = bytes.ReadUI32();
        tag.morphFillStyles = ReadMorphFillStyleArrayRecord(bytes);
        tag.morphLineStyles = ReadMorphLineStyleArrayRecord(bytes, 2);
        tag.startEdges = ReadSHAPE(bytes, 1);
        tag.endEdges = ReadSHAPE(bytes, 1);
        return tag;
    }

    private DefineFontTag ReadDefineFontTag(SwfByteArray bytes, TagHeaderRecord header) {
        var tag = new DefineFontTag();
        return tag;
    }

    private DefineTextTag ReadDefineTextTag(SwfByteArray bytes, TagHeaderRecord header) {
        var tag = new DefineTextTag();
        tag.characterID = bytes.ReadUI16();
        tag.textBounds = ReadRectangleRecord(bytes);
        tag.textMatrix = ReadMatrixRecord(bytes);
        tag.glyphBits = bytes.ReadUI8();
        tag.advanceBits = bytes.ReadUI8();

        var textRecords = new List<TextRecord>();
        while (true) {
            byte recordType = (byte)bytes.ReadUB(1);
            if (recordType == 0) {
                bytes.ReadUB(7);
                break;
            } else {
                textRecords.Add(ReadTextRecord(bytes, recordType, 1, tag.glyphBits, tag.advanceBits));
            }
        }
        tag.textRecords = textRecords.ToArray();
        tag.endOfRecordsFlag = 0;
        return tag;
    }

    private DefineText2Tag ReadDefineText2Tag(SwfByteArray bytes, TagHeaderRecord header) {
        var tag = new DefineText2Tag();
        tag.characterID = bytes.ReadUI16();
        tag.textBounds = ReadRectangleRecord(bytes);
        tag.textMatrix = ReadMatrixRecord(bytes);
        tag.glyphBits = bytes.ReadUI8();
        tag.advanceBits = bytes.ReadUI8();

        var textRecords = new List<TextRecord>();
        while (true) {
            byte recordType = (byte)bytes.ReadUB(1);
            if (recordType == 0) {
                bytes.ReadUB(7);
                break;
            } else {
                textRecords.Add(ReadTextRecord(bytes, recordType, 2, tag.glyphBits, tag.advanceBits));
            }
        }
        tag.textRecords = textRecords.ToArray();
        tag.endOfRecordsFlag = 0;
        return tag;
    }

    private DefineButtonTag ReadDefineButtonTag(SwfByteArray bytes, TagHeaderRecord header) {
        var tag = new DefineButtonTag();

        return tag;
    }

    private DefineButton2Tag ReadDefineButton2Tag(SwfByteArray bytes, TagHeaderRecord header) {
        var tag = new DefineButton2Tag();
        tag.buttonId = bytes.ReadUI16();
        tag.reservedFlags = (byte)bytes.ReadUB(7);
        tag.trackAsMenu = bytes.ReadFlag();
        tag.actionOffset = bytes.ReadUI16();

        var count = 0;
        var btnRecords = new List<ButtonRecord>();
        while (true) {
            byte reserved = (byte)bytes.ReadUB(2);
            bool hasBlendMode = bytes.ReadFlag();
            bool hasFilterList = bytes.ReadFlag();
            bool stateHitTest = bytes.ReadFlag();
            bool stateDown = bytes.ReadFlag();
            bool stateOver = bytes.ReadFlag();
            bool stateUp = bytes.ReadFlag();

            var isEnd = reserved == 0 &&
                        !hasBlendMode &&
                        !hasFilterList &&
                        !stateHitTest &&
                        !stateDown &&
                        !stateOver &&
                        !stateUp;

            if (isEnd) {
                break;
            } else {
                count++;
                btnRecords.Add(ReadButtonRecord(bytes, reserved, hasBlendMode, hasFilterList, stateHitTest, stateDown, stateOver, stateUp, 2));
            }
        }
        tag.characters = btnRecords.ToArray();
        tag.characterEndFlag = 0;
        //tag.actions = readButtonCondAction();
        return tag;
    }

    private DefineSpriteTag ReadDefineSpriteTag(SwfByteArray bytes, TagHeaderRecord header) {
        var tag = new DefineSpriteTag();
        tag.spriteId = bytes.ReadUI16();
        tag.frameCount = bytes.ReadUI16();
        tag.controlTags = ReadControlTags(bytes);
        return tag;
    }

    private UnknownTag ReadUnknownTag(SwfByteArray bytes, TagHeaderRecord header) {
        UnknownTag tag = new UnknownTag();
        if (header.length > 0) {
            tag.content = bytes.ReadBytes((int)header.length);
        }
        return tag;
    }


    private SwfHeader ReadSwfHeader(SwfByteArray bytes) {
        var header = new SwfHeader();
        header.signature = bytes.ReadStringWithLength(3);
        header.fileVersion = bytes.ReadUI8();
        header.uncompressedSize = bytes.ReadUI32();
        if (header.signature == SwfHeader.COMPRESSED_SIGNATURE) {
            bytes.Decompress();
        }
        header.frameSize = ReadRectangleRecord(bytes);
        header.frameRate = bytes.ReadFixed8_8();
        header.frameCount = bytes.ReadUI16();
        return header;
    }

    private TagHeaderRecord ReadTagHeaderRecord(SwfByteArray bytes) {
        var record = new TagHeaderRecord();
        ushort tagInfo = bytes.ReadUI16();
        record.type = (uint)(tagInfo >> 6);
        uint length = (uint)(tagInfo & ((1 << 6) - 1));
        if (length == 0x3F) {
            length = bytes.ReadUI32();
        }
        record.length = length;
        return record;
    }

    private RectangleRecord ReadRectangleRecord(SwfByteArray bytes) {
        bytes.AlignBytes();
        var record = new RectangleRecord();
        uint nBits = bytes.ReadUB(5);
        record.xMin = bytes.ReadSB(nBits);
        record.xMax = bytes.ReadSB(nBits);
        record.yMin = bytes.ReadSB(nBits);
        record.yMax = bytes.ReadSB(nBits);
        return record;
    }

    private MatrixRecord ReadMatrixRecord(SwfByteArray bytes) {
        var record = new MatrixRecord();
        record.hasScale = bytes.ReadFlag();
        if (record.hasScale) {
            record.nScaleBits = (byte)bytes.ReadUB(5);
            record.scaleX = bytes.ReadFB(record.nScaleBits);
            record.scaleY = bytes.ReadFB(record.nScaleBits);
        }
        record.hasRotate = bytes.ReadFlag();
        if (record.hasRotate) {
            record.nRotateBits = (byte)bytes.ReadUB(5);
            record.rotateSkew0 = bytes.ReadFB(record.nRotateBits);
            record.rotateSkew1 = bytes.ReadFB(record.nRotateBits);
        }
        record.nTranslateBits = (byte)bytes.ReadUB(5);
        record.translateX = bytes.ReadSB(record.nTranslateBits);
        record.translateY = bytes.ReadSB(record.nTranslateBits);
        return record;
    }

    private RGBRecord ReadRGBRecord(SwfByteArray bytes) {
        var record = new RGBRecord();
        record.red = bytes.ReadUI8();
        record.green = bytes.ReadUI8();
        record.blue = bytes.ReadUI8();
        return record;
    }

    private RGBARecord ReadRGBARecord(SwfByteArray bytes) {
        var record = new RGBARecord();
        record.red = bytes.ReadUI8();
        record.green = bytes.ReadUI8();
        record.blue = bytes.ReadUI8();
        record.alpha = bytes.ReadUI8();
        return record;
    }

    private ARGBRecord ReadARGBRecord(SwfByteArray bytes) {
        var record = new ARGBRecord();
        record.alpha = bytes.ReadUI8();
        record.red = bytes.ReadUI8();
        record.green = bytes.ReadUI8();
        record.blue = bytes.ReadUI8();
        return record;
    }

    private GradientRecord ReadGradientRecord(SwfByteArray bytes, byte shapeType) {
        var record = new GradientRecord();
        record.spreadMode = (byte)bytes.ReadUB(2);
        record.interpolationMode = (byte)bytes.ReadUB(2);
        record.numGradients = (byte)bytes.ReadUB(4);
        var list = new GradRecord[record.numGradients];
        for (byte i = 0; i < record.numGradients; i++) {
            list[i] = ReadGradRecord(bytes, shapeType);
        }
        record.gradientRecords = list;
        return record;
    }

    private FocalGradientRecord ReadFocalGradientRecord(SwfByteArray bytes, byte shapeType) {
        var record = new FocalGradientRecord();
        record.spreadMode = (byte)bytes.ReadUB(2);
        record.interpolationMode = (byte)bytes.ReadUB(2);
        record.numGradients = (byte)bytes.ReadUB(4);
        var list = new GradRecord[record.numGradients];
        for (byte i = 0; i < record.numGradients; i++) {
            list[i] = ReadGradRecord(bytes, shapeType);
        }
        record.gradientRecords = list;
        record.focalPoint = bytes.ReadFixed8_8();
        return record;
    }

    private GradRecord ReadGradRecord(SwfByteArray bytes, byte shapeType) {
        var record = new GradRecord();
        record.ratio = bytes.ReadUI8();
        if (shapeType == 1 || shapeType == 2) { //RGB(Shape1 or Shape2)
            record.color = ReadRGBRecord(bytes);
        } else { //RGBA(Shape3,4)
            record.color = ReadRGBARecord(bytes);
        }
        return record;
    }

    private ShapeWithStyleRecord ReadShapeWithStyleRecord(SwfByteArray bytes, byte shapeType) {
        var record = new ShapeWithStyleRecord();
        record.fillStyles = ReadFillStyleArrayRecord(bytes, shapeType);
        record.lineStyles = ReadLineStyleArrayRecord(bytes, shapeType);
        bytes.AlignBytes();
        byte numFillBits = (byte)bytes.ReadUB(4);
        byte numLineBits = (byte)bytes.ReadUB(4);
        record.numFillBits = numFillBits;
        record.numLineBits = numLineBits;
        var list = new List<IShapeRecord>();
        while (true) {
            var shapeRecord = ReadShapeRecord(bytes, numFillBits, numLineBits, shapeType);
            list.Add(shapeRecord);
            if (shapeRecord is StyleChangeRecord) {
                if (((StyleChangeRecord)shapeRecord).stateNewStyles) {
                    numFillBits = ((StyleChangeRecord)shapeRecord).numFillBits;
                    numLineBits = ((StyleChangeRecord)shapeRecord).numLineBits;
                }
            }
            if (shapeRecord is EndShapeRecord) break;
        }
        record.shapeRecords = list.ToArray();
        return record;
    }

    private FillStyleArrayRecord ReadFillStyleArrayRecord(SwfByteArray bytes, byte shapeType) {
        /*var record=new FillStyleArrayRecord();
        record.fillStyleCount=bytes.readUI8();
        var list=new FillStyleRecord[record.fillStyleCount];
        for(uint i=0;i<record.fillStyleCount;i++){
            list[i]=readFillStyleRecord(bytes,shapeType);
        }
        record.fillStyles=list;*/
        var record = new FillStyleArrayRecord();
        record.fillStyleCount = bytes.ReadUI8();
        //if(shapeType==2||shapeType==3){
        if (record.fillStyleCount == 0xFF) {
            record.fillStyleCountExtended = bytes.ReadUI16();
        }
        //}
        var list = new FillStyleRecord[record.fillStyleCount];
        for (uint i = 0; i < record.fillStyleCount; i++) {
            list[i] = ReadFillStyleRecord(bytes, shapeType);
        }
        record.fillStyles = list;
        return record;
    }

    private FillStyleRecord ReadFillStyleRecord(SwfByteArray bytes, byte shapeType) {
        var record = new FillStyleRecord();

        byte type = bytes.ReadUI8();
        record.fillStyleType = type;

        if (type == 0x00) {
            if (shapeType == 3 || shapeType == 4) {
                record.color = ReadRGBARecord(bytes);
            } else if (shapeType == 1 || shapeType == 2) {
                record.color = ReadRGBRecord(bytes);
            }
        }
        if (type == 0x10 || type == 0x12 || type == 0x13) {
            record.gradientMatrix = ReadMatrixRecord(bytes);
        }

        if (type == 0x10 || type == 0x12) {
            record.gradient = ReadGradientRecord(bytes, shapeType);
        } else if (type == 0x13) {
            record.gradient = ReadFocalGradientRecord(bytes, shapeType);
        }
        if (type == 0x40 || type == 0x41 || type == 0x42 || type == 0x43) {
            record.bitmapId = bytes.ReadUI16();
            record.bitmapMatrix = ReadMatrixRecord(bytes);
        }
        return record;
    }

    private LineStyleArrayRecord ReadLineStyleArrayRecord(SwfByteArray bytes, byte shapeType) {
        var record = new LineStyleArrayRecord();
        record.lineStyleCount = bytes.ReadUI8();
        if (record.lineStyleCount == 0xFF) {
            record.lineStyleCountExtended = bytes.ReadUI16();
        }
        var list = new ILineStyleRecord[record.lineStyleCount];
        if (shapeType == 1 || shapeType == 2 || shapeType == 3) {
            for (int i = 0; i < record.lineStyleCount; i++) {
                list[i] = ReadLineStyleRecord(bytes, shapeType);
            }
        } else if (shapeType == 4) {
            for (int i = 0; i < record.lineStyleCount; i++) {
                list[i] = ReadLineStyle2Record(bytes, shapeType);
            }
        }
        record.lineStyles = list;
        return record;
    }

    private LineStyleRecord ReadLineStyleRecord(SwfByteArray bytes, byte shapeType) {
        var record = new LineStyleRecord();
        record.width = bytes.ReadUI16();
        if (shapeType == 1 || shapeType == 2) { //RGB(Shape1 or Shape2)
            record.color = ReadRGBRecord(bytes);
        } else { //RGBA(Shape3)
            record.color = ReadRGBARecord(bytes);
        }
        return record;
    }

    private LineStyle2Record ReadLineStyle2Record(SwfByteArray bytes, byte shapeType) {
        var record = new LineStyle2Record();
        record.width = bytes.ReadUI16();
        record.startCapStyle = (byte)bytes.ReadUB(2);
        record.joinStyle = (byte)bytes.ReadUB(2);
        record.hasFillFlag = bytes.ReadFlag();
        record.noHScaleFlag = bytes.ReadFlag();
        record.noVScaleFlag = bytes.ReadFlag();
        record.pixelHintingFlag = bytes.ReadFlag();
        record.reserved = (byte)bytes.ReadUB(5);
        record.noClose = bytes.ReadFlag();
        record.endCapStyle = (byte)bytes.ReadUB(2);
        if (record.joinStyle == 2) {
            record.miterLimitFactor = bytes.ReadFixed8_8(); //bytes.readUI16();
        }

        if (!record.hasFillFlag) {
            record.color = ReadRGBARecord(bytes);
        } else {
            record.fillType = ReadFillStyleRecord(bytes, shapeType);
        }
        return record;
    }

    private IShapeRecord ReadShapeRecord(SwfByteArray bytes, byte numFillBits, byte numLineBits, byte shapeType) {
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
                var endShapeRecord = new EndShapeRecord();
                endShapeRecord.typeFlag = typeFlag;
                endShapeRecord.endOfShape = bytes.ReadUB(5);
                record = endShapeRecord;
            } else {
                var styleChangeRecord = new StyleChangeRecord();
                styleChangeRecord.stateNewStyles = stateNewStyles;
                styleChangeRecord.stateLineStyle = stateLineStyle;
                styleChangeRecord.stateFillStyle1 = stateFillStyle1;
                styleChangeRecord.stateFillStyle0 = stateFillStyle0;
                styleChangeRecord.stateMoveTo = stateMoveTo;
                if (stateMoveTo) {
                    styleChangeRecord.moveBits = (byte)bytes.ReadUB(5);
                    styleChangeRecord.moveDeltaX = bytes.ReadSB(styleChangeRecord.moveBits);
                    styleChangeRecord.moveDeltaY = bytes.ReadSB(styleChangeRecord.moveBits);
                }
                if (stateFillStyle0) {
                    styleChangeRecord.fillStyle0 = bytes.ReadUB(numFillBits);
                }
                if (stateFillStyle1) {
                    styleChangeRecord.fillStyle1 = bytes.ReadUB(numFillBits);
                }
                if (stateLineStyle) {
                    styleChangeRecord.lineStyle = bytes.ReadUB(numLineBits);
                }
                //----------------------------------
                if (stateNewStyles) {
                    styleChangeRecord.fillStyles = ReadFillStyleArrayRecord(bytes, shapeType);
                    styleChangeRecord.lineStyles = ReadLineStyleArrayRecord(bytes, shapeType);
                    styleChangeRecord.numFillBits = (byte)bytes.ReadUB(4);
                    styleChangeRecord.numLineBits = (byte)bytes.ReadUB(4);
                }
                //----------------------------------
                record = styleChangeRecord;
            }
        } else {
            bool straightFlag = bytes.ReadFlag();
            if (straightFlag) {
                var straightEdgeRecord = new StraightEdgeRecord();
                straightEdgeRecord.typeFlag = typeFlag;
                straightEdgeRecord.straightFlag = straightFlag;
                straightEdgeRecord.numBits = (byte)bytes.ReadUB(4);
                straightEdgeRecord.generalLineFlag = bytes.ReadFlag();

                /*if(!straightEdgeRecord.generalLineFlag){
                    straightEdgeRecord.vertLineFlag=bytes.readFlag();//(sbyte)bytes.readSB(1);
                }
                if(straightEdgeRecord.generalLineFlag||!straightEdgeRecord.vertLineFlag){
                    straightEdgeRecord.deltaX=bytes.readSB((uint)straightEdgeRecord.numBits+2);
                }
                if(straightEdgeRecord.generalLineFlag||straightEdgeRecord.vertLineFlag){
                    straightEdgeRecord.deltaY=bytes.readSB((uint)straightEdgeRecord.numBits+2);
                }*/

                if (straightEdgeRecord.generalLineFlag) {
                    straightEdgeRecord.deltaX = bytes.ReadSB((uint)straightEdgeRecord.numBits + 2);
                    straightEdgeRecord.deltaY = bytes.ReadSB((uint)straightEdgeRecord.numBits + 2);
                } else {
                    straightEdgeRecord.vertLineFlag = bytes.ReadFlag(); //(sbyte)bytes.readSB(1);
                    if (!straightEdgeRecord.vertLineFlag) {
                        straightEdgeRecord.deltaX = bytes.ReadSB((uint)straightEdgeRecord.numBits + 2);
                    } else {
                        straightEdgeRecord.deltaY = bytes.ReadSB((uint)straightEdgeRecord.numBits + 2);
                    }
                }
                record = straightEdgeRecord;
            } else {
                var curvedEdgeRecord = new CurvedEdgeRecord();
                curvedEdgeRecord.typeFlag = typeFlag;
                curvedEdgeRecord.straightFlag = straightFlag;
                curvedEdgeRecord.numBits = (byte)bytes.ReadUB(4);
                curvedEdgeRecord.controlDeltaX = bytes.ReadSB((uint)curvedEdgeRecord.numBits + 2);
                curvedEdgeRecord.controlDeltaY = bytes.ReadSB((uint)curvedEdgeRecord.numBits + 2);
                curvedEdgeRecord.anchorDeltaX = bytes.ReadSB((uint)curvedEdgeRecord.numBits + 2);
                curvedEdgeRecord.anchorDeltaY = bytes.ReadSB((uint)curvedEdgeRecord.numBits + 2);
                record = curvedEdgeRecord;
            }
        }
        return record;
    }

    private ColorMapDataRecord ReadColorMapDataRecord(SwfByteArray bytes, uint colorTableSize, uint imageDataSize) {
        var record = new ColorMapDataRecord();

        record.colorTableRGB = new RGBRecord[colorTableSize];
        for (uint i = 0; i < colorTableSize; i++) {
            record.colorTableRGB[i] = ReadRGBRecord(bytes);
        }

        record.colormapPixelData = new byte[imageDataSize];
        for (uint i = 0; i < imageDataSize; i++) {
            record.colormapPixelData[i] = bytes.ReadUI8();
        }
        return record;
    }

    private BitmapDataRecord ReadBitmapDataRecord(SwfByteArray bytes, byte bitmapformat, uint imageDataSize) {
        var record = new BitmapDataRecord();
        record.bitmapPixelData = new IPixRecord[imageDataSize];
        if (bitmapformat == 4) {
            for (uint i = 0; i < imageDataSize; i++) {
                record.bitmapPixelData[i] = ReadPix15Record(bytes);
            }
        } else if (bitmapformat == 5) {
            for (uint i = 0; i < imageDataSize; i++) {
                record.bitmapPixelData[i] = ReadPix24Record(bytes);
            }
        }
        return record;
    }

    private Pix15Record ReadPix15Record(SwfByteArray bytes) {
        var record = new Pix15Record();
        record.reserved = (byte)bytes.ReadUB(1);
        record.red = (byte)bytes.ReadUB(5);
        record.green = (byte)bytes.ReadUB(5);
        record.blue = (byte)bytes.ReadUB(5);
        return record;
    }

    private Pix24Record ReadPix24Record(SwfByteArray bytes) {
        var record = new Pix24Record();
        record.reserved = bytes.ReadUI8();
        record.red = bytes.ReadUI8();
        record.green = bytes.ReadUI8();
        record.blue = bytes.ReadUI8();
        return record;
    }

    private AlphaColorMapDataRecord ReadAlphaColorMapDataRecord(SwfByteArray bytes, uint colorTableSize, uint imageDataSize) {
        var record = new AlphaColorMapDataRecord();

        record.colorTableRGB = new RGBARecord[colorTableSize];
        for (uint i = 0; i < colorTableSize; i++) {
            record.colorTableRGB[i] = ReadRGBARecord(bytes);
        }

        record.colormapPixelData = new byte[imageDataSize];
        for (uint i = 0; i < imageDataSize; i++) {
            record.colormapPixelData[i] = bytes.ReadUI8();
        }
        return record;
    }

    private AlphaBitmapDataRecord ReadAlphaBitmapDataRecord(SwfByteArray bytes, uint imageDataSize) {
        var record = new AlphaBitmapDataRecord();
        record.bitmapPixelData = new ARGBRecord[imageDataSize];
        for (uint i = 0; i < imageDataSize; i++) {
            record.bitmapPixelData[i] = ReadARGBRecord(bytes);
        }
        return record;
    }

    private MorphFillStyleArrayRecord ReadMorphFillStyleArrayRecord(SwfByteArray bytes) {
        var record = new MorphFillStyleArrayRecord();
        record.fillStyleCount = bytes.ReadUI8();
        if (record.fillStyleCount == 0xFF) {
            record.fillStyleCountExtended = bytes.ReadUI16();
        }
        record.fillStyles = new MorphFillStyleRecord[record.fillStyleCount];
        for (var i = 0; i < record.fillStyleCount; i++) {
            record.fillStyles[i] = ReadMorphFillStyleRecord(bytes);
        }
        return record;
    }

    private MorphFillStyleRecord ReadMorphFillStyleRecord(SwfByteArray bytes) {
        var record = new MorphFillStyleRecord();
        record.fillStyleType = bytes.ReadUI8();

        var type = record.fillStyleType;
        if (type == 0x00) {
            record.startColor = ReadRGBARecord(bytes);
            record.endColor = ReadRGBARecord(bytes);
        } else if (type == 0x10 || type == 0x12) {
            record.startGradientMatrix = ReadMatrixRecord(bytes);
            record.endGradientMatrix = ReadMatrixRecord(bytes);
            record.gradient = ReadMorphGradientRecord(bytes);
        } else if (type == 0x40 || type == 0x41 || type == 0x42 || type == 0x43) {
            record.bitmapId = bytes.ReadUI16();
            record.startBitmapMatrix = ReadMatrixRecord(bytes);
            record.endBitmapMatrix = ReadMatrixRecord(bytes);
        }
        return record;
    }

    private MorphGradientRecord ReadMorphGradientRecord(SwfByteArray bytes) {
        var record = new MorphGradientRecord();
        record.numGradients = bytes.ReadUI8();
        record.gradientRecords = new MorphGradRecord[record.numGradients];
        for (var i = 0; i < record.numGradients; i++) {
            record.gradientRecords[i] = ReadMorphGradRecord(bytes);
        }
        return record;
    }

    private MorphGradRecord ReadMorphGradRecord(SwfByteArray bytes) {
        var record = new MorphGradRecord();
        record.startRatio = bytes.ReadUI8();
        record.startColor = ReadRGBARecord(bytes);
        record.endRatio = bytes.ReadUI8();
        record.endColor = ReadRGBARecord(bytes);
        return record;
    }

    private MorphLineStyleArrayRecord ReadMorphLineStyleArrayRecord(SwfByteArray bytes, byte morphShapeType) {
        var record = new MorphLineStyleArrayRecord();
        record.lineStyleCount = bytes.ReadUI8();
        if (record.lineStyleCount == 0xFF) {
            record.lineStyleCountExtended = bytes.ReadUI16();
        }
        if (morphShapeType == 1) {
            record.lineStyles = new MorphLineStyleRecord[record.lineStyleCount];
            for (var i = 0; i < record.lineStyleCount; i++) {
                record.lineStyles[i] = ReadMorphLineStyleRecord(bytes);
            }
        } else if (morphShapeType == 2) {
            record.lineStyles = new MorphLineStyle2Record[record.lineStyleCount];
            for (var i = 0; i < record.lineStyleCount; i++) {
                record.lineStyles[i] = ReadMorphLineStyle2Record(bytes);
            }
        }
        return record;
    }

    private MorphLineStyleRecord ReadMorphLineStyleRecord(SwfByteArray bytes) {
        var record = new MorphLineStyleRecord();
        record.startWidth = bytes.ReadUI16();
        record.endWidth = bytes.ReadUI16();
        record.startColor = ReadRGBARecord(bytes);
        record.endColor = ReadRGBARecord(bytes);
        return record;
    }

    private MorphLineStyle2Record ReadMorphLineStyle2Record(SwfByteArray bytes) {
        var record = new MorphLineStyle2Record();
        record.startWidth = bytes.ReadUI16();
        record.endWidth = bytes.ReadUI16();
        record.startCapStyle = (byte)bytes.ReadUB(2);
        record.joinStyle = (byte)bytes.ReadUB(2);
        record.hasFillFlag = bytes.ReadFlag();
        record.noHScaleFlag = bytes.ReadFlag();
        record.noVScaleFlag = bytes.ReadFlag();
        record.pixelHintingFlag = bytes.ReadFlag();
        record.reserved = (byte)bytes.ReadUB(5);
        record.noClose = bytes.ReadFlag();
        record.endCapStyle = (byte)bytes.ReadUB(2);
        if (record.joinStyle == 2) {
            record.miterLimitFactor = bytes.ReadUI16();
        }
        if (!record.hasFillFlag) {
            record.startColor = ReadRGBARecord(bytes);
            record.endColor = ReadRGBARecord(bytes);
        } else {
            record.fillType = ReadMorphFillStyleRecord(bytes);
        }
        return record;
    }

    private SHAPE ReadSHAPE(SwfByteArray bytes, byte morphShapeType) {
        var shape = new SHAPE();
        byte numFillBits = (byte)bytes.ReadUB(4);
        byte numLineBits = (byte)bytes.ReadUB(4);
        shape.numFillBits = numFillBits;
        shape.numLineBits = numLineBits;
        var list = new List<IShapeRecord>();
        //3:DefineMorphShape最小支持版本是SWF3与DefineShape3一样；
        //4:DefineMorphShape2最小支持版本是SWF8与DefineShape4一样
        var shapeType = morphShapeType == 1 ? 3 : 4;
        while (true) {
            var shapeRecord = ReadShapeRecord(bytes, numFillBits, numLineBits, (byte)shapeType);
            list.Add(shapeRecord);
            if (shapeRecord is StyleChangeRecord) {
                if (((StyleChangeRecord)shapeRecord).stateNewStyles) {
                    numFillBits = ((StyleChangeRecord)shapeRecord).numFillBits;
                    numLineBits = ((StyleChangeRecord)shapeRecord).numLineBits;
                }
            }
            if (shapeRecord is EndShapeRecord) break;
        }
        shape.shapeRecords = list.ToArray();
        return shape;
    }

    private CXFormRecord ReadCXFormRecord(SwfByteArray bytes) {
        var record = new CXFormRecord();
        record.hasAddTerms = bytes.ReadFlag();
        record.hasMultTerms = bytes.ReadFlag();
        record.nBits = (byte)bytes.ReadUB(4);
        if (record.hasMultTerms) {
            record.redMultTerm = bytes.ReadSB(record.nBits);
            record.greenMultTerm = bytes.ReadSB(record.nBits);
            record.blueMultTerm = bytes.ReadSB(record.nBits);
        }
        if (record.hasAddTerms) {
            record.redAddTerm = bytes.ReadSB(record.nBits);
            record.greenAddTerm = bytes.ReadSB(record.nBits);
            record.blueAddTerm = bytes.ReadSB(record.nBits);
        }
        return record;
    }

    private CXFormWithAlphaRecord ReadCXFormWithAlphaRecord(SwfByteArray bytes) {
        bytes.AlignBytes(); //必须
        var record = new CXFormWithAlphaRecord();
        record.hasAddTerms = bytes.ReadFlag();
        record.hasMultTerms = bytes.ReadFlag();
        var nBits = (byte)bytes.ReadUB(4);
        record.nBits = nBits;
        if (record.hasMultTerms) {
            record.redMultTerm = bytes.ReadSB(nBits);
            record.greenMultTerm = bytes.ReadSB(nBits);
            record.blueMultTerm = bytes.ReadSB(nBits);
            record.alphaMultTerm = bytes.ReadSB(nBits);
        }
        if (record.hasAddTerms) {
            record.redAddTerm = bytes.ReadSB(nBits);
            record.greenAddTerm = bytes.ReadSB(nBits);
            record.blueAddTerm = bytes.ReadSB(nBits);
            record.alphaAddTerm = bytes.ReadSB(nBits);
        }
        return record;
    }

    private FilterListRecord ReadFilterListRecord(SwfByteArray bytes) {
        var record = new FilterListRecord();
        record.numberOfFilters = bytes.ReadUI8();
        var filters = new FilterRecord[record.numberOfFilters];
        for (var i = 0; i < filters.Length; i++) {
            filters[i] = ReadFilterRecord(bytes);
        }
        record.filters = filters;
        return record;
    }

    private FilterRecord ReadFilterRecord(SwfByteArray bytes) {
        var record = new FilterRecord();
        record.filterId = bytes.ReadUI8();
        switch (record.filterId) {
            case 0:
                record.dropShadowFilter = ReadDropShadowFilterRecord(bytes);
                break;
            case 1:
                record.blurFilter = ReadBlurFilterRecord(bytes);
                break;
            case 2:
                record.glowFilter = ReadGlowFilterRecord(bytes);
                break;
            case 3:
                record.bevelFilter = ReadBevelFilterRecord(bytes);
                break;
            case 4:
                record.gradientGlowFilter = ReadGradientGlowFilterRecord(bytes);
                break;
            case 5:
                record.convolutionFilter = ReadConvolutionFilterRecord(bytes);
                break;
            case 6:
                record.colorMatrixFilter = ReadColorMatrixFilterRecord(bytes);
                break;
            case 7:
                record.gradientBevelFilter = ReadGradientBevelFilterRecord(bytes);
                break;
        }
        return record;
    }

    private DropShadowFilterRecord ReadDropShadowFilterRecord(SwfByteArray bytes) {
        var record = new DropShadowFilterRecord();
        record.dropShadowColor = ReadRGBARecord(bytes);
        record.blurX = bytes.ReadFixed16_16();
        record.blurY = bytes.ReadFixed16_16();
        record.angle = bytes.ReadFixed16_16();
        record.distance = bytes.ReadFixed16_16();
        record.strength = bytes.ReadFixed8_8();
        record.innerShadow = bytes.ReadFlag();
        record.knockout = bytes.ReadFlag();
        record.compositeSource = bytes.ReadFlag();
        record.passes = (byte)bytes.ReadUB(5);
        return record;
    }

    private BlurFilterRecord ReadBlurFilterRecord(SwfByteArray bytes) {
        var record = new BlurFilterRecord();
        record.blurX = bytes.ReadFixed16_16();
        record.blurY = bytes.ReadFixed16_16();
        record.passes = (byte)bytes.ReadUB(5);
        record.reserved = (byte)bytes.ReadUB(3);
        return record;
    }

    private GlowFilterRecord ReadGlowFilterRecord(SwfByteArray bytes) {
        var record = new GlowFilterRecord();
        record.glowColor = ReadRGBARecord(bytes);
        record.blurX = bytes.ReadFixed16_16();
        record.blurY = bytes.ReadFixed16_16();
        record.strength = bytes.ReadFixed8_8();
        record.innerGlow = bytes.ReadFlag();
        record.knockout = bytes.ReadFlag();
        record.compositeSource = bytes.ReadFlag();
        record.passes = (byte)bytes.ReadUB(5);
        return record;
    }

    private BevelFilterRecord ReadBevelFilterRecord(SwfByteArray bytes) {
        var record = new BevelFilterRecord();
        record.shadowColor = ReadRGBARecord(bytes);
        record.highlightColor = ReadRGBARecord(bytes);
        record.blurX = bytes.ReadFixed16_16();
        record.blurY = bytes.ReadFixed16_16();
        record.angle = bytes.ReadFixed16_16();
        record.distance = bytes.ReadFixed16_16();
        record.strength = bytes.ReadFixed8_8();
        record.innerShadow = bytes.ReadFlag();
        record.knockout = bytes.ReadFlag();
        record.compositeSource = bytes.ReadFlag();
        record.onTop = bytes.ReadFlag();
        record.passes = (byte)bytes.ReadUB(4);
        return record;
    }

    private GradientGlowFilterRecord ReadGradientGlowFilterRecord(SwfByteArray bytes) {
        var record = new GradientGlowFilterRecord();
        var numColors = bytes.ReadUI8();
        record.numColors = numColors;
        record.gradientColors = new RGBARecord[numColors];
        record.gradientRatio = new byte[numColors];
        for (var i = 0; i < numColors; i++) {
            record.gradientColors[i] = ReadRGBARecord(bytes);
            record.gradientRatio[i] = bytes.ReadUI8();
        }
        record.blurX = bytes.ReadFixed16_16();
        record.blurY = bytes.ReadFixed16_16();
        record.angle = bytes.ReadFixed16_16();
        record.distance = bytes.ReadFixed16_16();
        record.strength = bytes.ReadFixed8_8();
        record.innerShadow = bytes.ReadFlag();
        record.knockout = bytes.ReadFlag();
        record.compositeSource = bytes.ReadFlag();
        record.onTop = bytes.ReadFlag();
        record.passes = (byte)bytes.ReadUB(4);
        return record;
    }

    private GradientBevelFilterRecord ReadGradientBevelFilterRecord(SwfByteArray bytes) {
        var record = new GradientBevelFilterRecord();
        var numColors = bytes.ReadUI8();
        record.numColors = numColors;
        record.gradientColors = new RGBARecord[numColors];
        record.gradientRatio = new byte[numColors];
        for (var i = 0; i < numColors; i++) {
            record.gradientColors[i] = ReadRGBARecord(bytes);
            record.gradientRatio[i] = bytes.ReadUI8();
        }
        record.blurX = bytes.ReadFixed16_16();
        record.blurY = bytes.ReadFixed16_16();
        record.angle = bytes.ReadFixed16_16();
        record.distance = bytes.ReadFixed16_16();
        record.strength = bytes.ReadFixed8_8();
        record.innerShadow = bytes.ReadFlag();
        record.knockout = bytes.ReadFlag();
        record.compositeSource = bytes.ReadFlag();
        record.onTop = bytes.ReadFlag();
        record.passes = (byte)bytes.ReadUB(4);
        return record;
    }

    private ConvolutionFilterRecord ReadConvolutionFilterRecord(SwfByteArray bytes) {
        var record = new ConvolutionFilterRecord();
        record.matrixX = bytes.ReadUI8();
        record.matrixY = bytes.ReadUI8();
        record.divisor = bytes.ReadFloat();
        record.bias = bytes.ReadFloat();
        int len = record.matrixX * record.matrixY;
        record.matrix = new float[len];
        for (var i = 0; i < len; i++) {
            record.matrix[i] = bytes.ReadFloat();
        }
        record.defaultColor = ReadRGBARecord(bytes);
        record.reserved = (byte)bytes.ReadUB(6);
        record.clamp = bytes.ReadFlag();
        record.preserveAlpha = bytes.ReadFlag();
        return record;
    }

    private ColorMatrixFilterRecord ReadColorMatrixFilterRecord(SwfByteArray bytes) {
        var record = new ColorMatrixFilterRecord();
        record.matrix = new float[20];
        for (byte i = 0; i < 20; i++) {
            float value = bytes.ReadFloat();
            record.matrix[i] = value;
        }
        return record;
    }

    private ButtonRecord ReadButtonRecord(SwfByteArray bytes, byte reserved, bool hasBlendMode, bool hasFilterList, bool stateHitTest, bool stateDown, bool stateOver, bool stateUp, byte buttonType) {
        var record = new ButtonRecord();
        record.buttonReserved = reserved;
        record.buttonHasBlendMode = hasBlendMode;
        record.buttonHasFilterList = hasFilterList;
        record.buttonStateHitTest = stateHitTest;
        record.buttonStateDown = stateDown;
        record.buttonStateOver = stateOver;
        record.buttonStateUp = stateUp;
        record.characterID = bytes.ReadUI16();
        record.placeDepth = bytes.ReadUI16();
        record.placeMatrix = ReadMatrixRecord(bytes);
        if (buttonType == 2) {
            record.colorTransform = ReadCXFormWithAlphaRecord(bytes);
            if (hasFilterList) {
                record.filterList = ReadFilterListRecord(bytes);
            }
            if (hasBlendMode) {
                record.blendMode = bytes.ReadUI8();
            }
        }
        record.buttonType = buttonType;
        return record;
    }

    private TextRecord ReadTextRecord(SwfByteArray bytes, byte recordType, byte defineTextType, byte glyphBits, byte advanceBits) {
        var record = new TextRecord();
        record.textRecordType = recordType;
        record.styleFlagsReserved = (byte)bytes.ReadUB(3);
        record.styleFlagsHasFont = bytes.ReadFlag();
        record.styleFlagsHasColor = bytes.ReadFlag();
        record.styleFlagsHasYOffset = bytes.ReadFlag();
        record.styleFlagsHasXOffset = bytes.ReadFlag();
        if (record.styleFlagsHasFont) record.fontID = bytes.ReadUI16();
        if (record.styleFlagsHasColor) {
            if (defineTextType == 2) record.textColor = ReadRGBARecord(bytes);
            else record.textColor = ReadRGBRecord(bytes);
        }
        if (record.styleFlagsHasXOffset) record.xOffset = bytes.ReadSI16();
        if (record.styleFlagsHasYOffset) record.yOffset = bytes.ReadSI16();
        if (record.styleFlagsHasFont) record.textHeight = bytes.ReadUI16();
        record.glyphCount = bytes.ReadUI8();
        record.glyphEntries = new GlyphEntryRecord[record.glyphCount];
        for (var i = 0; i < record.glyphCount; i++) {
            record.glyphEntries[i] = ReadGlyphEntryRecord(bytes, glyphBits, advanceBits);
        }
        return record;
    }

    private GlyphEntryRecord ReadGlyphEntryRecord(SwfByteArray bytes, byte glyphBits, byte advanceBits) {
        var record = new GlyphEntryRecord();
        record.glyphIndex = bytes.ReadUB(glyphBits);
        record.glyphAdvance = bytes.ReadSB(advanceBits);
        return record;
    }
}