using System.Collections.Generic;
using System.Xml;

public class PlaceObjectTag : SwfTag, ICharacterIdTag {

    public ushort characterId;
    public ushort depth;
    public MatrixRecord matrix;
    public CXFormRecord colorTransform;

    public PlaceObjectTag(SwfByteArray bytes, TagHeaderRecord header) : base(header) {
        var originalPos = bytes.GetBytePosition();
        characterId = bytes.ReadUI16();
        depth = bytes.ReadUI16();
        matrix = new MatrixRecord(bytes);
        if (header.length > bytes.GetBytePosition() - originalPos) {
            colorTransform = new CXFormRecord(bytes);
        }
    }

    public override XmlElement ToXml(XmlDocument doc) {
        var ele = CreateXmlElement(doc, "PlaceObject");
        ele.SetAttribute("characterId", characterId.ToString());
        ele.SetAttribute("depth", depth.ToString());
        ele.SetAttribute("matrix", matrix.ToString());
        ele.AppendChild(colorTransform.ToXml(doc));
        return ele;
    }

    public void GetNeededCharacterIds(List<ushort> characterIds, Swf swf) {
        if (characterIds.IndexOf(characterId) < 0) {
            characterIds.Add(characterId);
        }
    }

    public ushort GetCharacterId() {
        return characterId;
    }

    public PlaceObjectTagData ToData() {
        var data = new PlaceObjectTagData();
        data.type = header.type;
        data.characterId = characterId;
        data.depth = depth;
        data.matrix = matrix;
        data.colorTransform = colorTransform;
        return data;
    }
}