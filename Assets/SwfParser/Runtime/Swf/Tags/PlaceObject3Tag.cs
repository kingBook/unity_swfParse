using System.Collections.Generic;
using System.Xml;

[System.Serializable]
public class PlaceObject3Tag : Tag, ICharacterIdTag {

    public bool placeFlagHasClipActions;
    public bool placeFlagHasClipDepth;
    public bool placeFlagHasName;
    public bool placeFlagHasRatio;
    public bool placeFlagHasColorTransform;
    public bool placeFlagHasMatrix;
    public bool placeFlagHasCharacter;
    public bool placeFlagMove;
    public byte reserved;
    public bool placeFlagOpaqueBackground;
    public bool placeFlagHasVisible;
    public bool placeFlagHasImage;
    public bool placeFlagHasClassName;
    public bool placeFlagHasCacheAsBitmap;
    public bool placeFlagHasBlendMode;
    public bool placeFlagHasFilterList;
    public ushort depth;
    public string className;
    public ushort characterId;
    public MatrixRecord matrix;
    public CXFormWithAlphaRecord colorTransform;
    public ushort ratio;
    public string name;
    public ushort clipDepth;
    public FilterListRecord surfaceFilterList;
    public byte blendMode;
    public byte bitmapCache;
    public byte visible;

    public RGBARecord backgroundColor;
    //public clipActions;

    public PlaceObject3Tag(SwfByteArray bytes, TagHeaderRecord header) : base(header) {
        placeFlagHasClipActions = bytes.ReadFlag();
        placeFlagHasClipDepth = bytes.ReadFlag();
        placeFlagHasName = bytes.ReadFlag();
        placeFlagHasRatio = bytes.ReadFlag();
        placeFlagHasColorTransform = bytes.ReadFlag();
        placeFlagHasMatrix = bytes.ReadFlag();
        placeFlagHasCharacter = bytes.ReadFlag();
        placeFlagMove = bytes.ReadFlag();
        reserved = (byte)bytes.ReadUB(1);
        placeFlagOpaqueBackground = bytes.ReadFlag();
        placeFlagHasVisible = bytes.ReadFlag();
        placeFlagHasImage = bytes.ReadFlag();
        placeFlagHasClassName = bytes.ReadFlag();
        placeFlagHasCacheAsBitmap = bytes.ReadFlag();
        placeFlagHasBlendMode = bytes.ReadFlag();
        placeFlagHasFilterList = bytes.ReadFlag();
        depth = bytes.ReadUI16();
        if (placeFlagHasClassName || (placeFlagHasImage && placeFlagHasCharacter)) {
            className = bytes.ReadString();
        }
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
        if (placeFlagHasFilterList) {
            surfaceFilterList = new FilterListRecord(bytes);
        }
        if (placeFlagHasBlendMode) {
            blendMode = bytes.ReadUI8();
        }
        if (placeFlagHasCacheAsBitmap) {
            bitmapCache = bytes.ReadUI8();
        }
        if (placeFlagHasVisible) {
            visible = bytes.ReadUI8();
            backgroundColor = new RGBARecord(bytes);
        }
        /*if(tag.placeFlagHasClipActions){
            tag.placeFlagHasClipActions=
        }*/
    }

    public override XmlElement ToXml(XmlDocument doc) {
        var ele = CreateXmlElement(doc, "PlaceObject3");
        ele.SetAttribute("placeFlagHasClipActions", placeFlagHasClipActions.ToString());
        ele.SetAttribute("placeFlagHasClipDepth", placeFlagHasClipDepth.ToString());
        ele.SetAttribute("placeFlagHasName", placeFlagHasName.ToString());
        ele.SetAttribute("placeFlagHasRatio", placeFlagHasRatio.ToString());
        ele.SetAttribute("placeFlagHasColorTransform", placeFlagHasColorTransform.ToString());
        ele.SetAttribute("placeFlagHasMatrix", placeFlagHasMatrix.ToString());
        ele.SetAttribute("placeFlagHasCharacter", placeFlagHasCharacter.ToString());
        ele.SetAttribute("placeFlagMove", placeFlagMove.ToString());
        ele.SetAttribute("reserved", reserved.ToString());
        ele.SetAttribute("placeFlagOpaqueBackground", placeFlagOpaqueBackground.ToString());
        ele.SetAttribute("placeFlagHasVisible", placeFlagHasVisible.ToString());
        ele.SetAttribute("placeFlagHasImage", placeFlagHasImage.ToString());
        ele.SetAttribute("placeFlagHasClassName", placeFlagHasClassName.ToString());
        ele.SetAttribute("placeFlagHasCacheAsBitmap", placeFlagHasCacheAsBitmap.ToString());
        ele.SetAttribute("placeFlagHasBlendMode", placeFlagHasBlendMode.ToString());
        ele.SetAttribute("placeFlagHasFilterList", placeFlagHasFilterList.ToString());
        ele.SetAttribute("depth", depth.ToString());
        if (placeFlagHasClassName) {
            ele.SetAttribute("className", className.ToString());
        }
        if (placeFlagHasCharacter) {
            ele.SetAttribute("characterId", characterId.ToString());
        }
        if (placeFlagHasMatrix) {
            ele.SetAttribute("matrix", matrix.ToString());
        }
        if (placeFlagHasColorTransform) {
            ele.SetAttribute("colorTransform", colorTransform.ToString());
        }
        if (placeFlagHasRatio) {
            ele.SetAttribute("ratio", ratio.ToString());
        }
        if (placeFlagHasName) {
            ele.SetAttribute("name", name.ToString());
        }
        if (placeFlagHasClipDepth) {
            ele.SetAttribute("clipDepth", clipDepth.ToString());
        }
        if (placeFlagHasFilterList) {
            ele.AppendChild(surfaceFilterList.ToXml(doc));
        }
        if (placeFlagHasBlendMode) {
            ele.SetAttribute("blendMode", blendMode.ToString());
        }
        if (placeFlagHasCacheAsBitmap) {
            ele.SetAttribute("bitmapCache", bitmapCache.ToString());
        }
        if (placeFlagHasVisible) {
            ele.SetAttribute("visible", visible.ToString());
        }
        /*if(placeFlagHasVisible){
            ele.AppendChild(clipActions.toXml(doc));
        }*/
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