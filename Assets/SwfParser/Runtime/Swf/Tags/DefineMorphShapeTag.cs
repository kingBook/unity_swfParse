using System.Collections.Generic;
using System.Xml;

[System.Serializable]
public class DefineMorphShapeTag : SwfTag, ICharacterIdTag {

    public ushort characterId;
    public RectangleRecord startBounds;
    public RectangleRecord endBounds;
    public uint offset;
    public MorphFillStyleArrayRecord morphFillStyles;
    public MorphLineStyleArrayRecord morphLineStyles;
    public SHAPE startEdges;
    public SHAPE endEdges;

    public DefineMorphShapeTag(SwfByteArray bytes, TagHeaderRecord header) : base(header) {
        characterId = bytes.ReadUI16();
        startBounds = new RectangleRecord(bytes);
        endBounds = new RectangleRecord(bytes);
        offset = bytes.ReadUI32();
        morphFillStyles = new MorphFillStyleArrayRecord(bytes);
        morphLineStyles = new MorphLineStyleArrayRecord(bytes, 1);
        startEdges = new SHAPE(bytes, 1);
        endEdges = new SHAPE(bytes, 1);
    }


    public override XmlElement ToXml(XmlDocument doc) {
        var ele = CreateXmlElement(doc, "DefineMorphShape");
        ele.SetAttribute("characterId", characterId.ToString());
        ele.SetAttribute("startBounds", startBounds.ToString());
        ele.SetAttribute("endBounds", endBounds.ToString());
        ele.SetAttribute("offset", offset.ToString());
        ele.AppendChild(morphFillStyles.ToXml(doc));
        ele.AppendChild(morphLineStyles.ToXml(doc));
        ele.AppendChild(startEdges.ToXml(doc));
        ele.AppendChild(endEdges.ToXml(doc));
        return ele;
    }

    public void FindUsedCharacterIds(List<ushort> characterIds, Swf swf) {
        if (characterIds.IndexOf(characterId) < 0) {
            characterIds.Add(characterId);

            // bitmapId
            var fillStyles = morphFillStyles.fillStyles;
            for (int i = 0, len = fillStyles.Length; i < len; i++) {
                var fillStyle = fillStyles[i];
                var type = fillStyle.fillStyleType;
                if (type == 0x40 || type == 0x41 || type == 0x42 || type == 0x43) {
                    if (characterIds.IndexOf(fillStyle.bitmapId) < 0) {
                        characterIds.Add(fillStyle.bitmapId);
                    }
                }
            }
        }
    }

    public ushort GetCharacterId() {
        return characterId;
    }

}