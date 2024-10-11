[System.Serializable]
public struct TagHeaderRecord {

    public const uint SHORT_HEADER_MAX_LENGTH = 0x3F;

    public ushort type;
    public uint length;

    public bool isLong => length >= SHORT_HEADER_MAX_LENGTH;

    public TagHeaderRecord(SwfByteArray bytes) {
        ushort tagInfo = bytes.ReadUI16();
        type = (ushort)(tagInfo >> 6);
        uint length = (uint)(tagInfo & ((1 << 6) - 1));
        if (length == 0x3F) {
            length = bytes.ReadUI32();
        }
        this.length = length;
    }
}