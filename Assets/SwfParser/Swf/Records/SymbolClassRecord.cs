public struct SymbolClassRecord {

    public ushort tag;
    public string name;

    public SymbolClassRecord(SwfByteArray bytes) {
        tag = bytes.ReadUI16();
        name = bytes.ReadString();
    }

}