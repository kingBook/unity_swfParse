public struct FillStyleRecord{
	public byte fillStyleType;
	public object color;//RGB|RGBA
	public MatrixRecord gradientMatrix;
	public object gradient;
	public ushort bitmapId;
	public MatrixRecord bitmapMatrix;

}
