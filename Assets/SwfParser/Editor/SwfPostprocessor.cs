using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class SwfPostprocessor : AssetPostprocessor {

    private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths) {
        foreach (string str in importedAssets) {
            int dotIndex = str.LastIndexOf('.');
            if (dotIndex > -1) {
                string extensionName = str.Substring(dotIndex);
                if (extensionName == ".swf") {
                    OnSwfPostprocess(str);
                }
            }
        }
    }

    private static void OnSwfPostprocess(string path) {
        int id = path.IndexOf('/');
        path = path.Substring(id);
        path = Application.dataPath + path;

        ParseSwf(path, true, true);

    }

    [MenuItem("SwfParser/Run")]
    public static void Run() {
        string assetsPath = Application.dataPath;
        ParseSwf(assetsPath + "/test.swf", true, true);
    }

    public static void ParseSwf(string swfPath, bool isExportXml, bool isExportBitmap) {
        // 截取掉xx.swf的文件夹路径，如：E:/kingBook/projects/unity_swfParse/Assets/
        string swfFolderPath = swfPath.Substring(0, swfPath.LastIndexOf('/') + 1);
        //Debug.Log(swfPath);
        var swfReader = new SwfReader();

        var swfBytes = new SwfByteArray(swfPath);

        Stopwatch sw = new Stopwatch();
        sw.Start();
        var swf = swfReader.Read(swfBytes);
        swfBytes.Close();
        sw.Stop();
        Debug.Log("read passed time:" + sw.ElapsedMilliseconds);

        if (isExportXml) {
            sw.Restart();
            var xml = swf.ToXml();
            sw.Stop();
            Debug.Log("convert xml passed time:" + sw.ElapsedMilliseconds);

            sw.Restart();
            SaveXml(xml, swfPath);
            sw.Stop();
            Debug.Log("save passed time:" + sw.ElapsedMilliseconds);
            //Debug.Log(formatXml(swf.toXml()));
        }
        if (isExportBitmap) {
            var imageDatas = swf.GetImageDatas();
            Debug.Log(imageDatas.Length);
            for (int i = 0, len = imageDatas.Length; i < len; i++) {
                imageDatas[i].SaveTo(swfFolderPath);
            }
        }

        EditorUtility.DisplayDialog("Complete", "Import complete\n\n" + swfPath.Replace(Application.dataPath, "Assets"), "OK");
    }

    /**保存xml文件*/
    private static void SaveXml(XmlDocument doc, string swfPath) {
        int id = swfPath.LastIndexOf('.');
        string fileName = swfPath.Substring(0, id) + ".xml";
        doc.Save(fileName);
    }

    private static string FormatXml(object xml) {
        XmlDocument xd;
        if (xml is XmlDocument) {
            xd = xml as XmlDocument;
        } else {
            xd = new XmlDocument();
            xd.LoadXml(xml as string);
        }
        StringBuilder sb = new StringBuilder();
        StringWriter sw = new StringWriter(sb);
        XmlTextWriter xtw = null;
        try {
            xtw = new XmlTextWriter(sw);
            xtw.Formatting = Formatting.Indented;
            xtw.Indentation = 1;
            xtw.IndentChar = '\t';
            xd.WriteTo(xtw);
        } finally {
            if (xtw != null) xtw.Close();
        }
        return sb.ToString();
    }


}