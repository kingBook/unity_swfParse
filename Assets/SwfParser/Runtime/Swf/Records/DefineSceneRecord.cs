[System.Serializable]
public struct DefineSceneRecord {

    public uint offset;
    public string name;

    public DefineSceneRecord(SwfByteArray bytes) {
        offset = bytes.ReadEncodedUI32();
        name = bytes.ReadString();
    }
}