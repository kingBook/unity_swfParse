using System.Collections.Generic;
using UnityEngine;

public class SwfSpriteAtlas {

    private Texture2D[] m_atlas;

    public SwfSpriteAtlas(ImageData[] imgDatas, int maxSize = 2048) {
        // Texture2D.GenerateAtlas()
        // var texture2D = new Texture2D(10, 10);
        // texture2D.PackTextures();

        var texture2Ds = ImageDatasToTexture2Ds(imgDatas);
        var atlasConfigs = Texture2DsToAtlasConfigs(texture2Ds, maxSize);
        // 创建图集
        CreateAtlas(atlasConfigs);
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

    private List<(int startIndex, int endIndex, int size)> Texture2DsToAtlasConfigs(Texture2D[] texture2Ds, int maxSize) {
        var atlasConfigs = new List<(int startIndex, int endIndex, int size)>();

        int areaTotal = 0, startIndex = 0;

        for (int i = 0, len = texture2Ds.Length; i < len; i++) {
            var texture2D = texture2Ds[i];

            int area = texture2D.width * texture2D.height;
            areaTotal += area;
            int sqrt = Mathf.CeilToInt(Mathf.Sqrt(areaTotal));
            int nextPowerOfTwo = Mathf.NextPowerOfTwo(sqrt);

            if (nextPowerOfTwo >= maxSize) {
                atlasConfigs.Add((startIndex, i, nextPowerOfTwo));
                startIndex = i;
                areaTotal = 0;
            } else if (i >= len - 1) {
                if (startIndex < i) {
                    atlasConfigs.Add((startIndex, i, nextPowerOfTwo));
                }
            }
        }
        return atlasConfigs;
    }

    private void CreateAtlas(List<(int startIndex, int endIndex, int size)> atlasConfigs) {
        for (int i = 0, len = atlasConfigs.Count; i < len; i++) {

        }
    }

    public void Save(string folderPath) {

    }
}