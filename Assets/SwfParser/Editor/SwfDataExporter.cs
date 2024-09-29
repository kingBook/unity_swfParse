using UnityEngine;
using UnityEditor;
using System.IO;

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
        string swfDataRelativePath = $"{FileUtil.GetProjectRelativePath(swfPath)}.swfData.asset";

        SwfData swfData;
        if (File.Exists(swfDataRelativePath)) {
            swfData = AssetDatabase.LoadAssetAtPath<SwfData>(swfDataRelativePath);
            m_swf.ToData(swfData, true);
            EditorUtility.SetDirty(swfData);
            AssetDatabase.SaveAssetIfDirty(swfData);
        } else {
            swfData = ScriptableObject.CreateInstance<SwfData>();
            m_swf.ToData(swfData, true);
            AssetDatabase.CreateAsset(swfData, swfDataRelativePath);
        }
        return swfData;
    }


}