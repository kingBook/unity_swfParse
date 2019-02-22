public struct StyleChangeRecord{//Shape Record
	public byte typeFlag;
	public byte stateNewStyles;
	public byte stateLineStyle;
	public byte stateFillStyle1;
	public byte stateFillStyle0;
	public byte stateMoveTo;
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
