using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;

public class TestCreateAsset : Editor {

    [MenuItem("Tools/TestCreateAsset")]
    private static void Test() {
        const string path = "Assets/playerData.asset";

        PlayerData playerData;
        bool isExists = File.Exists(path);
        
        if (isExists) {
            playerData = AssetDatabase.LoadAssetAtPath<PlayerData>(path);
            playerData.id = "bb";
            EditorUtility.SetDirty(playerData);
            AssetDatabase.SaveAssetIfDirty(playerData);
        } else {
            playerData = ScriptableObject.CreateInstance<PlayerData>();
            playerData.id = "aa";
            AssetDatabase.CreateAsset(playerData, path);
        }

        AssetDatabase.Refresh();


        // Rect rect = new Rect();
        // rect.width = 10;
        // rect.height = 20;

        // string path = Application.dataPath + "/Rect.bytes";
        // FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
        // BinaryFormatter binaryFormatter = new BinaryFormatter();
        // binaryFormatter.Serialize(fileStream,rect);
        // fileStream.Close();

        // AssetDatabase.Refresh(); // 保存到 Assets 文件夹下需要刷新才能看到




    }

    // * 注意必须标记可序列化
    [System.Serializable]
    public class Rect {
        public int width;
        public int height;
    }

}