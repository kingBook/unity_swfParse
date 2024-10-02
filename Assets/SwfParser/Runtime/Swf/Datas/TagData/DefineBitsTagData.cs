/// <summary>
/// 所有位图标签使用的数据
/// <c> TagType.DefineBits || TagType.DefineBitsLossless || TagType.DefineBitsLossless2 || TagType.JPEGTables || TagType.DefineBitsJPEG2 || TagType.DefineBitsJPEG3 || TagType.DefineBitsJPEG4 </c>
/// </summary>
[System.Serializable]
public class DefineBitsTagData : TagData {

    public ushort characterID;

}