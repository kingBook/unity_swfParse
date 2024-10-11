[System.Serializable]
public struct FramelabelRecord {

    public uint frameNum;
    public string frameLabel;

    public FramelabelRecord(SwfByteArray bytes) {
        frameNum = bytes.ReadEncodedUI32();
        frameLabel = bytes.ReadString();
    }

}