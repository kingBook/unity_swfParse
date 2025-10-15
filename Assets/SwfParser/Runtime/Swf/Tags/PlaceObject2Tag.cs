using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;

[System.Serializable]
public class PlaceObject2Tag : Tag, ICharacterIdTag {

    public bool placeFlagHasClipActions;
    public bool placeFlagHasClipDepth;
    public bool placeFlagHasName;
    public bool placeFlagHasRatio;
    public bool placeFlagHasColorTransform;
    public bool placeFlagHasMatrix;
    public bool placeFlagHasCharacter;
    public bool placeFlagMove;
    public ushort depth;
    public ushort characterId;
    public MatrixRecord matrix;
    public CXFormWithAlphaRecord colorTransform;
    public ushort ratio;
    public string name;

    public ushort clipDepth;
    //public clipActions;

    public PlaceObject2Tag(SwfByteArray bytes, TagHeaderRecord header) : base(header) {
        placeFlagHasClipActions = bytes.ReadFlag();
        placeFlagHasClipDepth = bytes.ReadFlag();
        placeFlagHasName = bytes.ReadFlag();
        placeFlagHasRatio = bytes.ReadFlag();
        placeFlagHasColorTransform = bytes.ReadFlag();
        placeFlagHasMatrix = bytes.ReadFlag();
        placeFlagHasCharacter = bytes.ReadFlag();
        placeFlagMove = bytes.ReadFlag();
        depth = bytes.ReadUI16();
        if (placeFlagHasCharacter) {
            characterId = bytes.ReadUI16();
        }
        if (placeFlagHasMatrix) {
            matrix = new MatrixRecord(bytes);
        }
        if (placeFlagHasColorTransform) {
            colorTransform = new CXFormWithAlphaRecord(bytes);
        }
        if (placeFlagHasRatio) {
            ratio = bytes.ReadUI16();
        }
        if (placeFlagHasName) {
            name = bytes.ReadString();
        }
        if (placeFlagHasClipDepth) {
            clipDepth = bytes.ReadUI16();
        }
        /*if(tag.placeFlagHasClipActions){
            tag.clipActions=
        }*/
    }

    public override void Load(Swf swf, MeshHelperBase meshHelper, DisplayObjectContainer parent) {
        if (placeFlagHasClipDepth) {

        }
        if (placeFlagHasName) {

        }
        if (placeFlagHasRatio) {

        }
        if (placeFlagHasColorTransform) {

        }
        if (placeFlagHasMatrix) {
            //Debug2.Log("placeObject2Tag.matrix:" + matrix);
        }
        if (placeFlagHasCharacter) {
            //Debug2.Log("placeObject2Tag.characterId:" + characterId);
            Tag tag = (Tag)swf.GetCharacterIdTag(characterId);
            tag.Load(swf, meshHelper, parent);
            //Debug2.Log("placeObject2Tag.characterIdTagType:" + (TagType)tag.header.type);
        }
        if (placeFlagMove) {

        }
    }

    public override XmlElement ToXml(XmlDocument doc) {
        var ele = CreateXmlElement(doc, "PlaceObject2");
        ele.SetAttribute("placeFlagHasClipActions", placeFlagHasClipActions.ToString());
        ele.SetAttribute("placeFlagHasClipDepth", placeFlagHasClipDepth.ToString());
        ele.SetAttribute("placeFlagHasName", placeFlagHasName.ToString());
        ele.SetAttribute("placeFlagHasRatio", placeFlagHasRatio.ToString());
        ele.SetAttribute("placeFlagHasColorTransform", placeFlagHasColorTransform.ToString());
        ele.SetAttribute("placeFlagHasCharacter", placeFlagHasCharacter.ToString());
        ele.SetAttribute("placeFlagMove", placeFlagMove.ToString());
        if (placeFlagHasCharacter) ele.SetAttribute("characterId", characterId.ToString());
        if (placeFlagHasMatrix) ele.SetAttribute("matrix", matrix.ToString());
        if (placeFlagHasColorTransform) ele.AppendChild(colorTransform.ToXml(doc));
        if (placeFlagHasRatio) ele.SetAttribute("ratio", ratio.ToString());
        if (placeFlagHasName) ele.SetAttribute("name", name.ToString());
        if (placeFlagHasClipDepth) ele.SetAttribute("clipDepth", clipDepth.ToString());
        //if(placeFlagHasClipActions)ele.AppendChild(clipActions.toXml(doc));
        return ele;
    }

    public void FindUsedCharacterIds(List<ushort> characterIds, Swf swf) {
        if (placeFlagHasCharacter && characterIds.IndexOf(characterId) < 0) {
            characterIds.Add(characterId);
        }
    }

    public ushort GetCharacterId() {
        return characterId;
    }

}