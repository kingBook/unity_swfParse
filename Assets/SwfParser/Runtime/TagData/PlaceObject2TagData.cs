[System.Serializable]
public class PlaceObject2TagData : TagData {
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
}