using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;

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
		
		parse(path);
		
	}

	[MenuItem("SwfParser/run")]
	public static void run(){
		parse(Application.dataPath+"/views.swf");
	}


	public static void parse(string swfPath){
		Debug.Log(swfPath);
		var swfReader=new SwfReader();

		var swfBytes=new SwfByteArray(swfPath);
		swfReader.read(swfBytes);
		swfBytes.close();
	}


}
