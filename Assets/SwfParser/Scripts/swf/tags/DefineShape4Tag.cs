public class DefineShape4Tag:SwfTag{
	public ushort shapeId;
	public RectangleRecord shapeBounds;
	public RectangleRecord edgeBounds;
	public byte reserved;
	public bool usesFillWindingRule;
	public bool usesNonScalingStrokes;
	public bool usesScalingStrokes;
	public ShapeWithStyleRecord shapes;
	
}
