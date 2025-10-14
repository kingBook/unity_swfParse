#if UNITY_EDITOR
using UnityEditor;

/// <summary>
/// .swf 文件环境菜单
/// </summary>
public class SwfContextMenu : Editor {

    [MenuItem("Assets/Parse swf", true)]
    private static bool ValidateParseSwf() {
        if (EditorApplication.isPlaying) return false;

        const string swfExtensionName = ".swf";
        for (int i = 0, len = Selection.assetGUIDs.Length; i < len; i++) {
            string path = AssetDatabase.GUIDToAssetPath(Selection.assetGUIDs[i]);
            bool isSwf = path.EndsWith(swfExtensionName, true, null);
            if (isSwf) {
                return true;
            }
        }
        return false;
    }

    [MenuItem("Assets/Parse swf")]
    private static void ParseSwf() {
        if (EditorApplication.isPlaying) return;

        const string swfExtensionName = ".swf";
        for (int i = 0, len = Selection.assetGUIDs.Length; i < len; i++) {
            string path = AssetDatabase.GUIDToAssetPath(Selection.assetGUIDs[i]);
            bool isSwf = path.EndsWith(swfExtensionName, true, null);
            if (isSwf) {
                SwfProcessor.ParseSwf(path);
            }
        }
    }

}
#endif
