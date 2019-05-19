
public struct ConvolutionFilterRecord{
	public byte matrixX;
	public byte matrixY;
	public float divisor;
	public float bias;
	public float[] matrix;
	public RGBARecord defaultColor;
	public byte reserved;
	public bool clamp;
	public bool preserveAlpha;
	
}
