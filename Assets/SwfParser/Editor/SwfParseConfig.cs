public enum ExportImagesOption {
    OneByOne,
    Atlas
}

public static class SwfParseConfig {

    public static bool isExportSwfDataAsset = true;
    public static bool isExportXml = false;
    public static ExportImagesOption exportImagesOption = ExportImagesOption.Atlas;

}