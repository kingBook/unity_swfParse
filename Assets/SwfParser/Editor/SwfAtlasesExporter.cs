using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class SwfAtlasesExporter {

    private struct AtlasConfig {
        public int startIndex;
        public int endIndex;
        public int size;
        public int padding;
    }

    private Swf m_swf;

    public SwfAtlasesExporter(Swf swf) {
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
        CreateAtlases(imageDatas, 2048, 4, out Texture2D[] atlases, out RectInfoArray[] rectInfo2Ds);
        var atlasPaths = SaveTexture2Ds(atlases, swfFolderPath);
        var atlasAssets = CreateAtlasAssets(atlasPaths);
        var atlasesData = new AtlasesData {
            atlasPaths = atlasPaths,
            rectInfo2Ds = rectInfo2Ds,
            atlasAssets = atlasAssets
        };
        sw.Stop();
        Debug.Log("Save swfSpriteAtlas passed time:" + sw.ElapsedMilliseconds);

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

    private List<AtlasConfig> Texture2DsToAtlasConfigs(Texture2D[] texture2Ds, int maxAtlasSize, int padding) {
        var atlasConfigs = new List<AtlasConfig>();

        int areaTotal = 0, startIndex = 0;
        for (int i = 0, len = texture2Ds.Length; i < len; i++) {
            var texture2D = texture2Ds[i];

            int area = texture2D.width * texture2D.height;
            areaTotal += area;
            int sqrt = Mathf.CeilToInt(Mathf.Sqrt(areaTotal));
            int nextPowerOfTwo = Mathf.NextPowerOfTwo(sqrt);
            nextPowerOfTwo = Mathf.Min(nextPowerOfTwo, maxAtlasSize);

            if (nextPowerOfTwo >= maxAtlasSize) {
                var config = new AtlasConfig {
                    startIndex = startIndex,
                    endIndex = i,
                    size = nextPowerOfTwo,
                    padding = padding
                };
                atlasConfigs.Add(config);
                startIndex = i + 1;
                areaTotal = 0;
            } else if (i >= len - 1) { // 最后一个
                if (startIndex <= i) {
                    var config = new AtlasConfig {
                        startIndex = startIndex,
                        endIndex = i,
                        size = nextPowerOfTwo,
                        padding = padding
                    };
                    atlasConfigs.Add(config);
                }
            }
        }
        return atlasConfigs;
    }

    private void CreateAtlases(ImageData[] imgDatas, int maxAtlasSize, int padding, out Texture2D[] atlases, out RectInfoArray[] rectInfo2Ds) {
        var texture2Ds = ImageDatasToTexture2Ds(imgDatas);
        var atlasConfigs = Texture2DsToAtlasConfigs(texture2Ds, maxAtlasSize, padding);
        // 创建图集和数据
        CreateAtlases(imgDatas, texture2Ds, atlasConfigs, out atlases, out rectInfo2Ds);
    }

    private void CreateAtlases(ImageData[] imgDatas, Texture2D[] texture2Ds, List<AtlasConfig> atlasConfigs, out Texture2D[] atlases, out RectInfoArray[] rectInfo2Ds) {
        int len = atlasConfigs.Count;
        atlases = new Texture2D[len];
        rectInfo2Ds = new RectInfoArray[len];
        for (int i = 0; i < len; i++) {
            var cfg = atlasConfigs[i];

            var subTexture2Ds = new Texture2D[cfg.endIndex - cfg.startIndex + 1];
            System.Array.Copy(texture2Ds, cfg.startIndex, subTexture2Ds, 0, subTexture2Ds.Length);

            var texture2DAtlas = new Texture2D(cfg.size, cfg.size);
            var rects = texture2DAtlas.PackTextures(subTexture2Ds, cfg.padding);

            var rectInfos = new RectInfoArray(cfg.endIndex - cfg.startIndex + 1);
            for (int j = cfg.startIndex; j <= cfg.endIndex; j++) {
                var rectInfo = new RectInfo {
                    characterID = imgDatas[j].characterID,
                    rect = rects[j - cfg.startIndex]
                };
                rectInfos[j - cfg.startIndex] = rectInfo;
            }

            atlases[i] = texture2DAtlas;
            rectInfo2Ds[i] = rectInfos;
        }
    }

    private string[] SaveTexture2Ds(Texture2D[] texture2Ds, string swfFolderPath) {
        var len = texture2Ds.Length;
        var paths = new string[len];
        for (int i = 0; i < len; i++) {
            var bytes = texture2Ds[i].EncodeToPNG();
            string path = $"{swfFolderPath}/{i}.png";
            FileStream fs = new FileStream(path, FileMode.Create);
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();
            paths[i] = FileUtil.GetProjectRelativePath(path);
        }
        return paths;
    }

}