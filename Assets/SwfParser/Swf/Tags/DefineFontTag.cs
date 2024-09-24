using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Text;

public class DefineFontTag : SwfTag, ICharacterIdTag {

    public ushort fontID;
    public ushort[] offsetTable;
    public SHAPE[] glyphShapeTable;

    public DefineFontTag(SwfByteArray bytes, TagHeaderRecord header) : base(header) {

    }

    public override XmlElement ToXml(XmlDocument doc) {
        var ele = CreateXmlElement(doc, "DefineFont");
        ele.SetAttribute("fontID", fontID.ToString());

        var len = offsetTable.Length;
        var maxId = len - 1;
        var strBuilder = new StringBuilder();
        for (var i = 0; i < len; i++) {
            strBuilder.Append(offsetTable[i].ToString());
            if (i < maxId) strBuilder.Append(',');
        }
        ele.SetAttribute("offsetTable", strBuilder.ToString());

        len = glyphShapeTable.Length;
        maxId = len - 1;
        strBuilder.Clear();
        for (var i = 0; i < len; i++) {
            ele.AppendChild(glyphShapeTable[i].ToXml(doc));
        }
        return ele;
    }

    public void GetNeededCharacterIds(List<ushort> characterIds, Swf swf) {
        if (characterIds.IndexOf(fontID) < 0) {
            characterIds.Add(fontID);
        }
    }

    public ushort GetCharacterId() {
        return fontID;
    }

}