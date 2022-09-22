﻿using System.Collections.Generic;
using System.Xml;

public class DefineShapeTag : SwfTag, ICharacterIdTag {

    public ushort shapeId;
    public RectangleRecord shapeBounds;
    public ShapeWithStyleRecord shapes;

    public override XmlElement ToXml(XmlDocument doc) {
        var ele = CreateXmlElement(doc, "DefineShape");
        ele.SetAttribute("shapeId", shapeId.ToString());
        ele.SetAttribute("shapeBounds", shapeBounds.ToString());
        ele.AppendChild(shapes.ToXml(doc));
        return ele;
    }

    public void GetNeededCharacterIds(List<ushort> characterIds, Swf swf) {
        if (characterIds.IndexOf(shapeId) < 0) {
            characterIds.Add(shapeId);

            // bitmapId
            FillStyleRecord[] fillStyles = shapes.fillStyles.fillStyles;
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
        return shapeId;
    }

    public TagData ToData() {
        var data = new DefineShapeTagData();
        data.type = (uint)TagType.DefineShape;
        data.shapeId = shapeId;

        // bitmapId
        FillStyleRecord[] fillStyles = shapes.fillStyles.fillStyles;
        for (int i = 0, len = fillStyles.Length; i < len; i++) {
            var fillStyle = fillStyles[i];
            var type = fillStyle.fillStyleType;
            if (type == 0x40 || type == 0x41 || type == 0x42 || type == 0x43) {
                data.bitmapId = fillStyle.bitmapId;
            }
        }
        return data;
    }
}