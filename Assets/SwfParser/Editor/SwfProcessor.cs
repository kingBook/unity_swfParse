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

        var swfReader = new SwfReader();
        var swfBytes = new SwfByteArray(swfPath);

        Stopwatch sw = new Stopwatch();
        sw.Start();
        var swf = swfReader.Read(swfBytes);
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
        var swfImagesExporter = new SwfImagesExporter(swf);
        swfImagesExporter.Export(swfFolderPath);
        // =========== 导出运行时 .swfData ===============================
        if (SwfParseConfig.isExportSwfDataAsset) {
            var swfDataExporter = new SwfDataExporter(swf);
            var swfData = swfDataExporter.Export(swfPath);
        }
        // ===============================================================
        AssetDatabase.Refresh();
        Debug.Log($"Import complete {swfPath.Replace(Application.dataPath, "Assets")}");
        //EditorUtility.DisplayDialog("Complete", $"Import complete\n\n{swfPath.Replace(Application.dataPath, "Assets")}", "OK");
    }
}
#endif