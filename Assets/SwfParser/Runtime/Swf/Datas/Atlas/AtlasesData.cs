using UnityEngine;

[System.Serializable]
public class AtlasesData {

    public Texture2D[] atlasAssets;
    public string[] atlasPaths;
    public RectInfoArray[] rectInfo2Ds;

    public (Texture2D atlas, string atlasPath, RectInfo? rectInfo) GetData(ushort characterID) {
        Texture2D atlas = null;
        RectInfo? rectInfo = null;
        string atlasPath = null;

        for (int i = 0, len = rectInfo2Ds.Length; i < len; i++) {
            var rectInfos = rectInfo2Ds[i].rectInfos;
            bool isFound = false;
            for (int j = 0, c = rectInfos.Length; j < c; j++) {
                var retInfo = rectInfos[j];
                if (characterID == retInfo.characterID) {
                    isFound = true;
                    atlas = atlasAssets[i];
                    atlasPath = atlasPaths[i];
                    rectInfo = retInfo;
                    break;
                }
            }
            if (isFound) break;
        }
        return (atlas, atlasPath, rectInfo);
    }


}