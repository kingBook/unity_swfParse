using System.Xml;
using UnityEditor;
using UnityEngine;

public class SwfPostprocessor:AssetPostprocessor{
	
	private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths){
        foreach (string str in importedAssets){
			int dotIndex=str.LastIndexOf('.');
			if(dotIndex>-1){
				string extensionName=str.Substring(dotIndex);
				if(extensionName==".swf"){
					OnSwfPostprocess(str);
				}
			}
        }
    }

	private static void OnSwfPostprocess(string path){
		int id=path.IndexOf('/');
		path=path.Substring(id);
		path=Application.dataPath+path;
		
		parseAndExportXml(path);
		
	}

	[MenuItem("SwfParser/run")]
	public static void run(){
		parseAndExportXml(Application.dataPath+"/views.swf");
	}

	public static void parseAndExportXml(string swfPath){
		Debug.Log(swfPath);
		var swfReader=new SwfReader();

		var swfBytes=new SwfByteArray(swfPath);
		var swf=swfReader.read(swfBytes);
		swfBytes.close();
		
		saveXml(swf.toXml(),swfPath);
	}

	/**保存xml文件*/
	private static void saveXml(XmlDocument doc,string swfPath) {
		int id=swfPath.LastIndexOf('.');
		string fileName=swfPath.Substring(0,id)+".xml";

		doc.Save(fileName);
	}

	

}
