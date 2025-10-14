public enum ExportImagesOption {
    /// <summary> 单张 </summary>
    OneByOne,
    /// <summary> 图集 </summary>
    Atlas
}

public static class SwfParseConfig {

    /// <summary> 是否导出 .swfData </summary>
    public static bool isExportSwfData = true;

    /// <summary> 是否导出 .xml </summary>
    public static bool isExportXml = false;

    /// <summary> 导出图片选项 </summary>
    public static ExportImagesOption exportImagesOption = ExportImagesOption.Atlas;

}