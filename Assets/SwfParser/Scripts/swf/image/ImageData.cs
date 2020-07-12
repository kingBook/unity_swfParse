using System;
using System.IO;

public struct ImageData{
	public ushort characterID;
	public ImageType type;
	public byte[] bytes;
	
	/// <summary>
	/// 以png或jpg格式，保存位图数据到本地
	/// </summary>
	/// <param name="path">如：E:/kingBook/projects/unity_swfParse/Assets/</param>
	public void saveTo(string path){
		if(type==ImageType.Png){
			path+=characterID+".png";
		}else if(type==ImageType.Jpg){
			path+=characterID+".jpg";
		}
		FileStream fs=new FileStream(path,FileMode.Create);
		fs.Write(bytes,0,bytes.Length);
		fs.Close();
	}
	
}
