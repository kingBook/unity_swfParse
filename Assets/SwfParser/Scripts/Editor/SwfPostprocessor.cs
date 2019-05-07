using System.Diagnostics;
using System.IO;
using System.Text;
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
		//Debug.Log(swfPath);
		var swfReader=new SwfReader();

		var swfBytes=new SwfByteArray(swfPath);

		Stopwatch sw=new Stopwatch();
		sw.Start();
		var swf=swfReader.read(swfBytes);
		swfBytes.close();
		sw.Stop();
		UnityEngine.Debug.Log("read passed time:"+sw.ElapsedMilliseconds);
		
		sw.Restart();
		var xml=swf.toXml();
		sw.Stop();
		UnityEngine.Debug.Log("convert xml passed time:"+sw.ElapsedMilliseconds);

		sw.Restart();
		saveXml(xml,swfPath);
		sw.Stop();
		UnityEngine.Debug.Log("save passed time:"+sw.ElapsedMilliseconds);

		//Debug.Log(formatXml(swf.toXml()));
		EditorUtility.DisplayDialog("Complete","Export "+swfPath+" to complete","OK");
	}

	/**保存xml文件*/
	private static void saveXml(XmlDocument doc,string swfPath) {
		int id=swfPath.LastIndexOf('.');
		string fileName=swfPath.Substring(0,id)+".xml";
		doc.Save(fileName);
	}

	private static string formatXml(object xml){
		XmlDocument xd;
		if(xml is XmlDocument) {
			xd=xml as XmlDocument;
		}else{ 
			xd = new XmlDocument();
			xd.LoadXml(xml as string);
		}
		StringBuilder sb = new StringBuilder();
		StringWriter sw = new StringWriter(sb);  
		XmlTextWriter xtw = null;  
		try{  
			xtw = new XmlTextWriter(sw);  
			xtw.Formatting = Formatting.Indented;  
			xtw.Indentation = 1;  
			xtw.IndentChar = '\t';  
			xd.WriteTo(xtw);  
		}finally{  
			if (xtw != null)  
				xtw.Close();  
		}  
		return sb.ToString();
	} 

	

}
