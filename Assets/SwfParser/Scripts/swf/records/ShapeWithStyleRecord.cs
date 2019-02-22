using System.Collections;

public struct ShapeWithStyleRecord{
	public FillStyleArrayRecord fillStyles;
	public LineStyleArrayRecord lineStyles;
	public byte numFillBits;
	public byte numLineBits;
	public IShapeRecord[] shapeRecords;
	
}
