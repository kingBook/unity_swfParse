﻿#if UNITY_EDITOR
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

public static class SwfProcessor {

    public static void ParseSwf(string swfPath) {
        // 转为绝对路径
        int idx = swfPath.IndexOf('/');
        swfPath = swfPath.Substring(idx);
        swfPath = Application.dataPath + swfPath;
        // 截取掉xx.swf的文件夹路径，如：E:/kingBook/projects/unity_swfParse/Assets/
        string swfFolderPath = swfPath.Substring(0, swfPath.LastIndexOf('/') + 1);

        var swfReader = new SwfReader();
        var swfBytes = new SwfByteArray(swfPath);

        Stopwatch sw = new Stopwatch();
        sw.Start();
        var swf = swfReader.Read(swfBytes);
        swf.InitLinkageDefineSpritesNeededCharacters();
        swfBytes.Close();
        sw.Stop();
        Debug.Log("read passed time:" + sw.ElapsedMilliseconds);

        // 导出 .xml
        if (SwfParseConfig.isExportXml) {
            sw.Restart();
            var xml = swf.ToXml();
            sw.Stop();
            Debug.Log("convert xml passed time:" + sw.ElapsedMilliseconds);

            sw.Restart();
            SaveXml(xml, swfPath);
            sw.Stop();
            Debug.Log("save passed time:" + sw.ElapsedMilliseconds);
            //Debug.Log(formatXml(swf.toXml()));
        }

        // 导出 bitmapData
        var imageDatas = swf.GetImageDatas(isOnlyExportLinkage: true);
        for (int i = 0, len = imageDatas.Length; i < len; i++) {
            imageDatas[i].SaveTo(swfFolderPath);
        }

        // 导出运行时 SwfData
        var swfData = swf.GetSwfData(isOnlyExportLinkage: true);
        SaveSwfData(swfData, swfPath);

        // 根据有链接类名的库元件，创建 GameObject
        CreateGameObjectsWithSymbolClassTags(swf, swfData, isCreatePrefabAsset:true);

        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("Complete", "Import complete\n\n" + swfPath.Replace(Application.dataPath, "Assets"), "OK");
    }

    private static void CreateGameObjectsWithSymbolClassTags(Swf swf, SwfData swfData, bool isCreatePrefabAsset) {
        for (int i = 0, len = swf.symbolClassTags.Count; i < len; i++) {
            var symbolClassTag = swf.symbolClassTags[i];
            for (int j = 0, lenJ = symbolClassTag.numSymbols; j < lenJ; j++) {
                var symbolClass = symbolClassTag.symbols[j];
                CreateGameObjectWithSymbolClass(swfData, symbolClass, isCreatePrefabAsset);
            }
        }
    }

    private static void CreateGameObjectWithSymbolClass(SwfData swfData, SymbolClassRecord symbolClass, bool isCreatePrefabAsset) {
        GameObject inst = new GameObject(symbolClass.name, typeof(MovieClip));

        for (int i = 0, len = swfData.defineSpriteTagDatas.Count; i < len; i++) {
            var defineSpriteTagData = swfData.defineSpriteTagDatas[i];
            if (defineSpriteTagData.spriteId == symbolClass.tag) {
                for (int j = 0, lenJ = defineSpriteTagData.tagTypeAndIndices.Length; j < lenJ; j++) {
                    var tagTypeAndIndex = defineSpriteTagData.tagTypeAndIndices[j];
                    //
                    //var ctrlTagData = swfData.
                    TagType tagType = (TagType)tagTypeAndIndex.tagType;
                    switch (tagType) {
                        // bitmap
                        case TagType.DefineBits:
                        case TagType.JPEGTables:
                        case TagType.DefineBitsJPEG2:
                        case TagType.DefineBitsJPEG3:
                        case TagType.DefineBitsJPEG4:
                        case TagType.DefineBitsLossless:
                        case TagType.DefineBitsLossless2:
                            // dataIndex = swfData.defineBitsTagDatas.Count;
                            // var defineBitsTagData = GetDefineBitsTagData(characterIdTag);
                            // swfData.defineBitsTagDatas.Add(defineBitsTagData);
                            break;
                        // shape
                        case TagType.DefineShape:
                        case TagType.DefineShape2:
                        case TagType.DefineShape3:
                        case TagType.DefineShape4:
                            // dataIndex = swfData.defineShapeTagDatas.Count;
                            // var defineShapeTagData = GetDefineShapeTagData((DefineShapeTag)characterIdTag);
                            // swfData.defineShapeTagDatas.Add(defineShapeTagData);
                            break;
                        // sprite and movieClip
                        case TagType.DefineSprite:
                            // dataIndex = swfData.defineSpriteTagDatas.Count;
                            // var defineSpriteData = ((DefineSpriteTag)characterIdTag).ToData(swfData);
                            // swfData.defineSpriteTagDatas.Add(defineSpriteData);
                            break;
                        default:
                            
                            break;
                    }
                }
            }
        }
        
        
    }

    /// <summary> 保存xml文件 </summary>
    private static void SaveXml(XmlDocument doc, string swfPath) {
        int dotIdx = swfPath.LastIndexOf('.');
        string xmlPath = $"{swfPath.Substring(0, dotIdx)}.xml";
        doc.Save(xmlPath);
    }

    private static string FormatXml(object xml) {
        XmlDocument xd;
        if (xml is XmlDocument document) {
            xd = document;
        } else {
            xd = new XmlDocument();
            xd.LoadXml((string)xml);
        }
        StringBuilder sb = new StringBuilder();
        StringWriter sw = new StringWriter(sb);
        XmlTextWriter xtw = null;
        try {
            xtw = new XmlTextWriter(sw);
            xtw.Formatting = Formatting.Indented;
            xtw.Indentation = 1;
            xtw.IndentChar = '\t';
            xd.WriteTo(xtw);
        } finally {
            xtw?.Close();
        }
        return sb.ToString();
    }

    private static void SaveSwfData(SwfData swfData, string swfPath) {
        int dotIdx = swfPath.LastIndexOf('.');
        string swfDataPath = $"{swfPath.Substring(0, dotIdx)}.swfData";

        if (SwfParseConfig.isExportSwfDataAsset) {
            string swfDataRelativePath = $"{swfDataPath.Replace(Application.dataPath, "Assets")}.asset";
            AssetDatabase.CreateAsset(swfData, swfDataRelativePath);
        }

        /*FileStream fileStream = new FileStream(swfDataPath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
        StreamWriter streamWriter = new StreamWriter(fileStream, System.Text.Encoding.UTF8);
        XmlSerializer xmlSerializer = new XmlSerializer(swfData.GetType());
        xmlSerializer.Serialize(streamWriter, swfData);
        streamWriter.Close();
        fileStream.Close();*/



    }


}
#endif