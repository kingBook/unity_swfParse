public struct TagHeaderRecord {
	public static readonly uint SHORT_HEADER_MAX_LENGTH=0x3F;

	public uint type;
	public uint length;

	public bool isLong => length>=SHORT_HEADER_MAX_LENGTH;
}
