public struct LineStyle2Record{
	public ushort width;
	public byte startCapStyle;
	public byte joinStyle;
	public bool hasFillFlag;
	public bool noHScaleFlag;
	public bool noVScaleFlag;
	public bool pixelHintingFlag;
	public byte reserved;
	public bool noClose;
	public byte endCapStyle;
	public ushort miterLimitFactor;
	public RGBARecord color;
	public FillStyleRecord fillType;
}
