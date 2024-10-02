[System.Serializable]
public struct SymbolClassRecord{

    public ushort tagId;
    public string name;

    public SymbolClassRecord(SwfByteArray bytes) {
        tagId = bytes.ReadUI16();
        name = bytes.ReadString();
    }

}