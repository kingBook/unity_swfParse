using UnityEngine;
using System.Linq;
using UnityEditor;

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
        SwfData swfData = m_swf.ToData(true);
        Save(swfData, swfPath);
        return swfData;
    }

    private void Save(SwfData swfData, string swfPath) {
        int dotIdx = swfPath.LastIndexOf('.');
        string swfDataPath = $"{swfPath.Substring(0, dotIdx)}.swfData";

        if (SwfParseConfig.isExportSwfDataAsset) {
            string swfDataRelativePath = $"{swfDataPath.Replace(Application.dataPath, "Assets")}.asset";
            AssetDatabase.CreateAsset(swfData, swfDataRelativePath);
        }

        // FileStream fileStream = new FileStream(swfDataPath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
        // StreamWriter streamWriter = new StreamWriter(fileStream, System.Text.Encoding.UTF8);
        // XmlSerializer xmlSerializer = new XmlSerializer(swfData.GetType());
        // xmlSerializer.Serialize(streamWriter, swfData);
        // streamWriter.Close();
        // fileStream.Close();
    }



}