#if UNITY_EDITOR
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

public static class SwfProcessor {

    public static void ParseSwf(string swfPath) {
        // 转为绝对路径
        int idx = swfPath.IndexOf('/');
        swfPath = swfPath.Substring(idx);
        swfPath = Application.dataPath + swfPath;
        // 截取掉xx.swf的文件夹路径，如：E:/kingBook/projects/unity_swfParse/Assets/
        string swfFolderPath = swfPath.Substring(0, swfPath.LastIndexOf('/') + 1);

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
        var swfDataExporter = new SwfDataExporter(swf);
        var swfData = swfDataExporter.Export(swfPath);
        // ============ 根据有链接类名的库元件，创建 GameObject ============
        //var swfMcExporter = new SwfMcExporter(swf);
        //swfMcExporter.Export(swfData, isCreatePrefabAsset: true);
        // ===============================================================
        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("Complete", "Import complete\n\n" + swfPath.Replace(Application.dataPath, "Assets"), "OK");
    }
}
#endif