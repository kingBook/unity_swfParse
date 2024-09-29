using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;

public class SwfImagesExporter {

    private Swf m_swf;

    public SwfImagesExporter(Swf swf) {
        m_swf = swf;
    }

    /// <summary>
    /// 导出位图数据
    /// </summary>
    /// <param name="swfFolderPath"> 绝对路径，如：E:/kingBook/projects/unity_swfParse/Assets </param>
    public void Export(string swfFolderPath) {
        ImageData[] imageDatas = m_swf.GetImageDatas(true);
        Save(imageDatas, swfFolderPath);
    }

    /// <summary>
    /// 以png或jpg，保存位图数据到本地（如果 bytes==null 或 bytes.length<=0 将取消）
    /// </summary>
    /// <param name="imageDatas"> 位图数据 </param>
    /// <param name="swfFolderPath"> 绝对路径，如：E:/kingBook/projects/unity_swfParse/Assets </param>
    private void Save(ImageData[] imageDatas, string swfFolderPath) {
        for (int i = 0, len = imageDatas.Length; i < len; i++) {
            ImageData imageData = imageDatas[i];
            if (imageData.bytes == null || imageData.bytes.Length <= 0) continue;
            string extensionName = imageData.type == ImageType.Png ? ".png" : ".jpg";
            string path = $"{swfFolderPath}/{imageData.characterID}{extensionName}";
            FileStream fs = new FileStream(path, FileMode.Create);
            fs.Write(imageData.bytes, 0, imageData.bytes.Length);
            fs.Close();
        }
    }








}