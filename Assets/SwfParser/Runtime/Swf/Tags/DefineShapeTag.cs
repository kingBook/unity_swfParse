﻿using System.Collections.Generic;
using System.Xml;

public class DefineShapeTag : SwfTag, ICharacterIdTag {

    public ushort shapeId;
    public RectangleRecord shapeBounds;
    public ShapeWithStyleRecord shapes;

    public DefineShapeTag(TagHeaderRecord header) : base(header) {
        // empty constructor
    }

    public DefineShapeTag(SwfByteArray bytes, TagHeaderRecord header) : base(header) {
        shapeId = bytes.ReadUI16();
        shapeBounds = new RectangleRecord(bytes);
        shapes = new ShapeWithStyleRecord(bytes, 1);
    }

    public override XmlElement ToXml(XmlDocument doc) {
        var ele = CreateXmlElement(doc, "DefineShape");
        ele.SetAttribute("shapeId", shapeId.ToString());
        ele.SetAttribute("shapeBounds", shapeBounds.ToString());
        ele.AppendChild(shapes.ToXml(doc));
        return ele;
    }

    public DefineShapeTagData ToData() {
        var data = new DefineShapeTagData();
        data.type = header.type;
        data.shapeId = shapeId;

        // bitmapId
        FillStyleRecord[] fillStyles = shapes.fillStyles.fillStyles;
        if (fillStyles.Length >= 2) {
            var fillStyle = fillStyles[1];
            var type = fillStyle.fillStyleType;
            if (type == 0x40 || type == 0x41 || type == 0x42 || type == 0x43) {
                data.bitmapId = fillStyle.bitmapId;
            }
        }
        return data;
    }

    public void GetNeededCharacterIds(List<ushort> characterIds, Swf swf) {
        if (characterIds.IndexOf(shapeId) < 0) {
            characterIds.Add(shapeId);

            // bitmapId
            FillStyleRecord[] fillStyles = shapes.fillStyles.fillStyles;
            if (fillStyles.Length >= 2) {
                var fillStyle = fillStyles[1];
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

}