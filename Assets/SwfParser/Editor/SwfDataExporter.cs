using UnityEngine;
using UnityEditor;
using System.IO;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class SwfDataExporter {

    private Swf m_swf;

    public SwfDataExporter(Swf swf) {
        m_swf = swf;
    }

    /// <summary>
    /// 导出 .swfData
    /// </summary>
    /// <param name="swfPath"> 绝对路径 </param>
    public SwfData Export(string swfPath) {
        Stopwatch sw = new Stopwatch();
        sw.Start();
        string swfDataRelativePath = $"{FileUtil.GetProjectRelativePath(swfPath)}Data.asset";

        SwfData swfData;
        if (File.Exists(swfDataRelativePath)) {
            swfData = AssetDatabase.LoadAssetAtPath<SwfData>(swfDataRelativePath);
            m_swf.ToData(swfData, true);
            sw.Stop();
            Debug.Log("Swf to data passed time:" + sw.ElapsedMilliseconds);
            sw.Restart();
            EditorUtility.SetDirty(swfData);
            AssetDatabase.SaveAssetIfDirty(swfData);
            sw.Stop();
            Debug.Log("Create swfData.asset passed time:" + sw.ElapsedMilliseconds);
        } else {
            swfData = ScriptableObject.CreateInstance<SwfData>();
            m_swf.ToData(swfData, true);
            sw.Stop();
            Debug.Log("Swf to data passed time:" + sw.ElapsedMilliseconds);
            sw.Restart();
            AssetDatabase.CreateAsset(swfData, swfDataRelativePath);
            sw.Stop();
            Debug.Log("Create swfData.asset passed time:" + sw.ElapsedMilliseconds);
        }

        return swfData;
    }


}