#if UNITY_EDITOR
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

public static class SwfProcessor {

    public static void ParseSwf(string swfPath) {
        // 转为绝对路径, 如：E:/kingBook/projects/unity_swfParse/Assets/xx.swf
        swfPath = FileUtil.GetLogicalPath(System.IO.Path.GetFullPath(swfPath));
        // 截取掉 /xx.swf 的文件夹路径，如：E:/kingBook/projects/unity_swfParse/Assets
        string swfFolderPath = FileUtil.GetLogicalPath(System.IO.Path.GetDirectoryName(swfPath));

        var swfBytes = new SwfByteArray(swfPath);
        Stopwatch sw = new Stopwatch();
        sw.Start();
        var swf = SwfReader.Read(swfBytes);
        swf.FindLinkageDefineTags();
        swfBytes.Close();
        sw.Stop();
        Debug.Log("read swf passed time:" + sw.ElapsedMilliseconds);

        // =========== 导出 .xml ========================================
        if (SwfParseConfig.isExportXml) {
            SwfXmlExporter swfXmlExporter = new SwfXmlExporter(swf);
            swfXmlExporter.Export(swfPath);
        }
        // =========== 导出位图 ==========================================
        AtlasesData atlasesData = null;
        switch (SwfParseConfig.exportImagesOption) {
            case ExportImagesOption.OneByOne:
                var swfImagesExporter = new SwfImagesExporter(swf);
                atlasesData = swfImagesExporter.Export(swfFolderPath);
                break;
            case ExportImagesOption.Atlas:
                var swfAtlasesExporter = new SwfAtlasesExporter(swf);
                atlasesData = swfAtlasesExporter.Export(swfFolderPath);
                break;
        }
        Debug.Log("atlasesData.rectInfo2Ds:" + atlasesData.rectInfo2Ds.Length);
        // =========== 导出运行时 .swfData ===============================
        if (SwfParseConfig.isExportSwfDataAsset) {
            var swfDataExporter = new SwfDataExporter(swf);
            swfDataExporter.Export(swfPath, atlasesData);
        }
        // ===============================================================
        AssetDatabase.Refresh();
        Debug.Log($"Parseing completed: {FileUtil.GetProjectRelativePath(swfPath)}");
        //EditorUtility.DisplayDialog("Complete", $"Import complete\n\n{FileUtil.GetProjectRelativePath(swfPath)}", "OK");
    }
}
#endif