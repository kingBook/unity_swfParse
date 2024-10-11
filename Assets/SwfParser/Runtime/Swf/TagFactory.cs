using UnityEngine;

public static class TagFactory {

    public static SwfTag CreateTag(SwfByteArray bytes, TagHeaderRecord header) {
        SwfTag tag;
        var tagType = (TagType)header.type;
        switch (tagType) {
            //============= Display list tags =======
            case TagType.PlaceObject: // 4
                tag = new PlaceObjectTag(bytes, header);
                break;
            case TagType.PlaceObject2: // 26
                tag = new PlaceObject2Tag(bytes, header);
                break;
            case TagType.PlaceObject3: // 70
                tag = new PlaceObject3Tag(bytes, header);
                break;
            case TagType.RemoveObject: // 5
                tag = new RemoveObjectTag(bytes, header);
                break;
            case TagType.RemoveObject2: // 28
                tag = new RemoveObject2Tag(bytes, header);
                break;
            case TagType.ShowFrame: // 1
                tag = new ShowFrameTag(bytes, header);
                break;
            //============= Control Tags =======
            case TagType.SetBackgroundColor: // 9
                tag = new SetBackgroundColorTag(bytes, header);
                break;
            case TagType.FrameLabel: // 43
                tag = new FrameLabelTag(bytes, header);
                break;
            case TagType.Protect: // 24
                tag = new ProtectTag(bytes, header);
                break;
            case TagType.End: // 0
                tag = new EndTag(bytes, header);
                break;
            case TagType.ExportAssets: // 56
                tag = new ExportAssetsTag(bytes, header);
                break;
            case TagType.EnableDebugger2: // 64
                tag = new EnableDubugger2Tag(bytes, header);
                break;
            case TagType.ScriptLimits: // 65
                tag = new ScriptLimitsTag(bytes, header);
                break;
            case TagType.SetTabIndex: // 66
                tag = new SetTabIndexTag(bytes, header);
                break;
            case TagType.FileAttributes: // 69
                tag = new FileAttributesTag(bytes, header);
                break;
            case TagType.ImportAssets2: // 71
                tag = new ImportAssets2Tag(bytes, header);
                break;
            case TagType.SymbolClass: // 76
                tag = new SymbolClassTag(bytes, header);
                break;
            case TagType.Metadata: // 77
                tag = new MetadataTag(bytes, header);
                break;
            case TagType.DefineScalingGrid: // 78
                tag = new DefineScalingGridTag(bytes, header);
                break;
            case TagType.DefineSceneAndFrameLabelData: // 86
                tag = new DefineSceneAndFrameLabelDataTag(bytes, header);
                break;
            //============= Shape Tags =======
            case TagType.DefineShape: // 2
                tag = new DefineShapeTag(bytes, header);
                break;
            case TagType.DefineShape2: // 22
                tag = new DefineShape2Tag(bytes, header);
                break;
            case TagType.DefineShape3: // 32
                tag = new DefineShape3Tag(bytes, header);
                break;
            case TagType.DefineShape4: // 83
                tag = new DefineShape4Tag(bytes, header);
                break;
            //============= Bitmaps =======
            case TagType.DefineBits: // 6
                tag = new DefineBitsTag(bytes, header);
                break;
            case TagType.JPEGTables: // 8
                tag = new JPEGTablesTag(bytes, header);
                break;
            case TagType.DefineBitsJPEG2: // 21
                tag = new DefineBitsJPEG2Tag(bytes, header);
                break;
            case TagType.DefineBitsJPEG3: // 35
                tag = new DefineBitsJPEG3Tag(bytes, header);
                break;
            case TagType.DefineBitsLossless: // 20
                tag = new DefineBitsLosslessTag(bytes, header);
                break;
            case TagType.DefineBitsLossless2: // 36
                tag = new DefineBitsLossless2Tag(bytes, header);
                break;
            case TagType.DefineBitsJPEG4: // 90
                tag = new DefineBitsJPEG4Tag(bytes, header);
                break;
            //============= Shape Morphing =======
            case TagType.DefineMorphShape: // 46
                tag = new DefineMorphShapeTag(bytes, header);
                break;
            case TagType.DefineMorphShape2: // 84
                tag = new UnknownTag(bytes, header);
                Debug.LogWarning("DefineMorphShape2Tag is not implemented.");
                //tag = new DefineMorphShape2Tag(this, bytes,header);
                break;
            //============= Fonts and Text =======
            case TagType.DefineFont: // 10
                tag = new DefineFontTag(bytes, header);
                break;
            case TagType.DefineText: // 11
                tag = new DefineTextTag(bytes, header);
                break;
            case TagType.DefineText2: // 33
                tag = new DefineText2Tag(bytes, header);
                break;
            //============= Buttons =======
            case TagType.DefineButton: // 7
                tag = new UnknownTag(bytes, header);
                Debug.LogError("DefineButtonTag is not implemented.");
                //tag = new DefineButtonTag(bytes,header);
                break;
            case TagType.DefineButton2: // 34
                tag = new DefineButton2Tag(bytes, header);
                break;
            //============= Sprites and Movie Clips =======
            case TagType.DefineSprite: // 39
                tag = new DefineSpriteTag(bytes, header);
                break;
            default:
                tag = new UnknownTag(bytes, header);
                break;
        }
        return tag;
    }
}