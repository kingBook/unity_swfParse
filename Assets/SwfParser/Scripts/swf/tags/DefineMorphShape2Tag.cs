
public class DefineMorphShape2Tag:SwfTag{
	public ushort characterId;
	public RectangleRecord startBounds;
	public RectangleRecord endBounds;
	public RectangleRecord startEdgeBounds;
	public RectangleRecord endEdgeBounds;
	public byte reserved;
	public bool usesNonScalingStrokes;
	public bool usesScalingStrokes;
	public uint offset;
	public MorphFillStyleArrayRecord morphFillStyles;
	public MorphLineStyleArrayRecord morphLineStyles;
	public SHAPE startEdges;
	public SHAPE endEdges;
}
