using UnityEditor;
using UnityEngine;

public class SwfMcExporter {

    private Swf m_swf;

    public SwfMcExporter(Swf swf) {
        m_swf = swf;
    }

    public void Export(SwfData swfData, bool isCreatePrefabAsset) {
        for (int i = 0, len = m_swf.symbolClassTags.Count; i < len; i++) {
            var symbolClassTag = m_swf.symbolClassTags[i];
            for (int j = 0, numSymbols = symbolClassTag.numSymbols; j < numSymbols; j++) {
                var symbolClass = symbolClassTag.symbols[j];
                CreateGameObjectWithSymbolClass(swfData, symbolClass, isCreatePrefabAsset);
            }
        }
    }

    private void CreateGameObjectWithSymbolClass(SwfData swfData, SymbolClassRecord symbolClass, bool isCreatePrefabAsset) {
        GameObject gameObj = new GameObject(symbolClass.name);
        // 记录操作，使其能撤销
        Undo.RegisterCreatedObjectUndo(gameObj, "CreateGameObjectWithSymbolClass");

        MovieClip mc = gameObj.AddComponent<MovieClip>();
        DefineSpriteTagData defineSpriteTagData = GetDefineSpriteTagDataWithSymbolClass(swfData, symbolClass);
        mc.SeDatas(swfData, defineSpriteTagData);
    }

    private DefineSpriteTagData GetDefineSpriteTagDataWithSymbolClass(SwfData swfData, SymbolClassRecord symbolClass) {
        for (int i = 0, len = swfData.defineSpriteTagDatas.Count; i < len; i++) {
            DefineSpriteTagData defineSpriteTagData = swfData.defineSpriteTagDatas[i];
            if (defineSpriteTagData.spriteId == symbolClass.tagId) {
                return defineSpriteTagData;
            }
        }
        Debug.LogError("No corresponding DefineSpriteTagData found");
        return null;
    }


} // end class