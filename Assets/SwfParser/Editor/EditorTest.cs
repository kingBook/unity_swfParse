#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

public class EditorTest : Editor {

    [MenuItem("Tools/EditorTest", true)]
    private static bool ValidateMenuItem() {
        return !EditorApplication.isPlaying;
    }

    [MenuItem("Tools/EditorTest")]
    private static void Test() {
        if (EditorApplication.isPlaying) return;
        Debug.Log("== Tools/EditorTest ==");
    }
}
#endif