using UnityEditor;
using System.IO;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class SwfAssetExporter {

    private Swf m_swf;

    public SwfAssetExporter(Swf swf) {
        m_swf = swf;
    }

    /// <summary>
    /// 导出 .swfData
    /// </summary>
    /// <param name="swfPath"> 绝对路径 </param>
    /// <param name="atlasesData"> 图集数据 </param>
    public void Export(string swfPath, AtlasesData atlasesData) {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        string swfDataRelativePath = $"{FileUtil.GetProjectRelativePath(swfPath)}.asset";

        if (File.Exists(swfDataRelativePath)) {
            var swfAsset = AssetDatabase.LoadAssetAtPath<Swf>(swfDataRelativePath);
            swfAsset.SetTo(m_swf);
            swfAsset.SetAtlasesData(atlasesData);
            EditorUtility.SetDirty(swfAsset);
            AssetDatabase.SaveAssetIfDirty(swfAsset);
        } else {
            m_swf.SetAtlasesData(atlasesData);
            AssetDatabase.CreateAsset(m_swf, swfDataRelativePath);
        }

        sw.Stop();
        Debug.Log("Create swf.asset passed time:" + sw.ElapsedMilliseconds);
    }


}