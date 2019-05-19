
public class PlaceObject3Tag:SwfTag{
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
}
