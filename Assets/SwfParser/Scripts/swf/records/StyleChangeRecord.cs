public struct StyleChangeRecord:IShapeRecord{//:Shape Record
	public bool typeFlag;
	public bool stateNewStyles;
	public bool stateLineStyle;
	public bool stateFillStyle1;
	public bool stateFillStyle0;
	public bool stateMoveTo;
	public byte moveBits;
	public int moveDeltaX;
	public int moveDeltaY;
	public uint fillStyle0;
	public uint fillStyle1;
	public uint lineStyle;
	public FillStyleArrayRecord fillStyles;
	public LineStyleArrayRecord lineStyles;
	public byte numFillBits;
	public byte numLineBits;
}
