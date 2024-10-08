using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class SwfImagesExporter {

    private Swf m_swf;

    public SwfImagesExporter(Swf swf) {
        m_swf = swf;
    }

    /// <summary>
    /// 导出位图数据
    /// </summary>
    /// <param name="swfFolderPath"> 绝对路径，如：E:/kingBook/projects/unity_swfParse/Assets </param>
    public AtlasesData Export(string swfFolderPath) {
        Stopwatch sw = new Stopwatch();
        sw.Start();
        ImageData[] imageDatas = m_swf.GetImageDatas(true);
        sw.Stop();
        Debug.Log("GetImageDatas passed time:" + sw.ElapsedMilliseconds);

        sw.Restart();
        CreateAtlases(imageDatas, out _, out RectInfoArray[] rectInfo2Ds);
        var atlasPaths = SaveImageDatas(imageDatas, swfFolderPath);
        var atlasAssets = CreateAtlasAssets(atlasPaths);
        var atlasesData = new AtlasesData {
            atlasAssets = atlasAssets,
            atlasPaths = atlasPaths,
            rectInfo2Ds = rectInfo2Ds
        };
        sw.Stop();
        Debug.Log("Save image datas passed time:" + sw.ElapsedMilliseconds);

        return atlasesData;
    }

    private Texture2D[] CreateAtlasAssets(string[] atlasPaths) {
        int len = atlasPaths.Length;
        var atlasAssets = new Texture2D[len];
        for (int i = 0; i < len; i++) {
            var path = atlasPaths[i];
            AssetDatabase.ImportAsset(path);
            atlasAssets[i] = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
        }
        return atlasAssets;
    }

    private void CreateAtlases(ImageData[] imgDatas, out Texture2D[] atlases, out RectInfoArray[] rectInfo2Ds) {
        int len = imgDatas.Length;
        atlases = ImageDatasToTexture2Ds(imgDatas);

        rectInfo2Ds = new RectInfoArray[len];
        for (int i = 0; i < len; i++) {
            var characterID = imgDatas[i].characterID;
            var texture2D = atlases[i];

            var rectInfos = new RectInfoArray(1);
            rectInfos[0] = new RectInfo {
                characterID = characterID,
                rect = new Rect(0, 0, texture2D.width, texture2D.height)
            };
            rectInfo2Ds[i] = rectInfos;
        }
    }

    private Texture2D[] ImageDatasToTexture2Ds(ImageData[] imgDatas) {
        var texture2Ds = new Texture2D[imgDatas.Length];
        for (int i = 0; i < imgDatas.Length; i++) {
            var imgData = imgDatas[i];
            var texture2D = new Texture2D(4, 4);
            texture2D.LoadImage(imgData.bytes);
            texture2Ds[i] = texture2D;
        }
        return texture2Ds;
    }

    /// <summary>
    /// 以png或jpg，保存位图数据到本地（如果 bytes==null 或 bytes.length<=0 将取消）
    /// </summary>
    /// <param name="imageDatas"> 位图数据 </param>
    /// <param name="swfFolderPath"> 绝对路径，如：E:/kingBook/projects/unity_swfParse/Assets </param>
    private string[] SaveImageDatas(ImageData[] imageDatas, string swfFolderPath) {
        var len = imageDatas.Length;
        var paths = new string[len];
        for (int i = 0; i < len; i++) {
            ImageData imageData = imageDatas[i];
            if (imageData.bytes == null || imageData.bytes.Length <= 0) continue;
            string extensionName = imageData.type == ImageType.Png ? ".png" : ".jpg";
            string path = $"{swfFolderPath}/{imageData.characterID}{extensionName}";
            FileStream fs = new FileStream(path, FileMode.Create);
            fs.Write(imageData.bytes, 0, imageData.bytes.Length);
            fs.Close();
            paths[i] = FileUtil.GetProjectRelativePath(path);
        }
        return paths;
    }


}