using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Text;

public class Swf {
	public SwfHeader header;
	public List<SwfTag> tags;
	
	public XmlDocument toXml(){
		var doc=new XmlDocument();
		XmlDeclaration declaration = doc.CreateXmlDeclaration("1.0","UTF-8",null);
		doc.AppendChild(declaration);

		var swfElement=doc.CreateElement("Swf");
		doc.AppendChild(swfElement);
		var sw=new System.Diagnostics.Stopwatch();
		for(int i=0,len=tags.Count;i<len;i++){
			var tag=tags[i];
			
			sw.Restart();
			var tagXml=tag.toXml(doc);
			sw.Stop();
			Debug.LogFormat("type:{0},time:{1}",tag.header.type,sw.ElapsedMilliseconds);
			
			swfElement.AppendChild(tagXml);
		}
		return doc;
	}
	
	public ImageData[] getImageDatas(){
		var imageDatas=new List<ImageData>();
		for(int i=0,len=tags.Count;i<len;i++){
			var tag=tags[i];
			if(tag.header.type==(uint)TagType.DefineBits){
				var imageData=getDefineBitsImageData((DefineBitsTag)tag);
				if(imageData.bytes!=null && imageData.bytes.Length>0){
					imageDatas.Add(imageData);
				}
			}else if(tag.header.type==(uint)TagType.DefineBitsJPEG2){
				var imageData=getDefineBitsJPEG2ImageData((DefineBitsJPEG2Tag)tag);
				imageDatas.Add(imageData);
			}else if(tag.header.type==(uint)TagType.DefineBitsJPEG3){
				var imageData=getDefineBitsJPEG3ImageData((DefineBitsJPEG3Tag)tag);
				imageDatas.Add(imageData);
			}else if(tag.header.type==(uint)TagType.DefineBitsLossless){
				var imageData=getDefineBitsLosslessImageData((DefineBitsLosslessTag)tag);
				imageDatas.Add(imageData);
			}else if(tag.header.type==(uint)TagType.DefineBitsLossless2){
				var imageData=getDefineBitsLossless2ImageData((DefineBitsLossless2Tag)tag);
				imageDatas.Add(imageData);
			}else if(tag.header.type==(uint)TagType.DefineBitsJPEG4){
				var imageData=getDefineBitsJPEG4ImageData((DefineBitsJPEG4Tag)tag);
				imageDatas.Add(imageData);
			}
		}
		return imageDatas.ToArray();
	}
	
	private ImageData getDefineBitsImageData(DefineBitsTag defineBits){
		var imageData=new ImageData();
		if(defineBits.jpegData!=null){
			imageData.characterID=defineBits.characterID;
			imageData.type=ImageType.Jpg;
			imageData.bytes=defineBits.jpegData;
		}
		return imageData;
	}
	
	private ImageData getDefineBitsJPEG2ImageData(DefineBitsJPEG2Tag defineBitsJPEG2){
		var imageData=new ImageData();
		imageData.characterID=defineBitsJPEG2.characterID;
		
		bool isPng=defineBitsJPEG2.imageData[0]==0x89
				&& defineBitsJPEG2.imageData[1]==0x50 
				&& defineBitsJPEG2.imageData[2]==0x4E
				&& defineBitsJPEG2.imageData[3]==0x47
				&& defineBitsJPEG2.imageData[4]==0x0D
				&& defineBitsJPEG2.imageData[5]==0x0A
				&& defineBitsJPEG2.imageData[6]==0x1A
				&& defineBitsJPEG2.imageData[7]==0x0A;
		bool isGif=defineBitsJPEG2.imageData[0]==0x47
				&& defineBitsJPEG2.imageData[1]==0x49
				&& defineBitsJPEG2.imageData[2]==0x46
				&& defineBitsJPEG2.imageData[3]==0x38
				&& defineBitsJPEG2.imageData[4]==0x39
				&& defineBitsJPEG2.imageData[5]==0x61;
		if(isPng){
			imageData.type=ImageType.Png;
		}else if(isGif){
			imageData.type=ImageType.Gif;
		}else{
			imageData.type=ImageType.Jpg;
		}
		imageData.bytes=defineBitsJPEG2.imageData;
		return imageData;
	}
	
	private ImageData getDefineBitsJPEG3ImageData(DefineBitsJPEG3Tag defineBitsJPEG3){
		var imageData=new ImageData();
		imageData.characterID=defineBitsJPEG3.characterID;
		
		return imageData;
	}
	
	private ImageData getDefineBitsLosslessImageData(DefineBitsLosslessTag defineBitsLossless){
		var texture=new Texture2D(defineBitsLossless.bitmapWidth,defineBitsLossless.bitmapHeight);
		if(defineBitsLossless.bitmapFormat==3){
			//ColorMapDataRecord
			var colorMapDataRecord=(ColorMapDataRecord)defineBitsLossless.zlibBitmapData;
			int length=colorMapDataRecord.colormapPixelData.Length;
			var colors=new Color[length];
			for(int i=0;i<length;i++){
				var colorIndex=colorMapDataRecord.colormapPixelData[i];
				var rgb=colorMapDataRecord.colorTableRGB[colorIndex];
				colors[i]=new Color(rgb.red/255.0f,rgb.green/255.0f,rgb.blue/255.0f);
			}
			colors=flipVerticalBitmapColors(colors,defineBitsLossless.bitmapWidth,defineBitsLossless.bitmapHeight);
			texture.SetPixels(colors);
			texture.Apply();
		}else if(defineBitsLossless.bitmapFormat==4||defineBitsLossless.bitmapFormat==5){
			//BitmapDataRecord
			var bitmapDataRecord=(BitmapDataRecord)defineBitsLossless.zlibBitmapData;
			int length=bitmapDataRecord.bitmapPixelData.Length;
			var colors=new Color[length];
			if(defineBitsLossless.bitmapFormat==4){
				for(int i=0;i<length;i++){
					var pix15=(Pix15Record)bitmapDataRecord.bitmapPixelData[i];
					colors[i]=new Color(pix15.red/255.0f,pix15.green/255.0f,pix15.blue/255.0f);
				}
			}else if(defineBitsLossless.bitmapFormat==5){
				for(int i=0;i<length;i++){
					var pix24=(Pix24Record)bitmapDataRecord.bitmapPixelData[i];
					colors[i]=new Color(pix24.red/255.0f,pix24.green/255.0f,pix24.blue/255.0f);
				}
			}
			colors=flipVerticalBitmapColors(colors,defineBitsLossless.bitmapWidth,defineBitsLossless.bitmapHeight);
			texture.SetPixels(colors);
			texture.Apply();
		}
		var imageData=new ImageData();
		imageData.characterID=defineBitsLossless.characterID;
		//Png或Jpg都可以
		//imageData.type=ImageType.Png;
		imageData.type=ImageType.Jpg;
		imageData.bytes=texture.EncodeToJPG();
		return imageData;
	}
	
	private ImageData getDefineBitsLossless2ImageData(DefineBitsLossless2Tag defineBitsLossless2){
		var texture=new Texture2D(defineBitsLossless2.bitmapWidth,defineBitsLossless2.bitmapHeight);
		if(defineBitsLossless2.bitmapFormat==3){
			//AlphaColorMapDataRecord
			var alphaColorMapDataRecord=(AlphaColorMapDataRecord)defineBitsLossless2.zlibBitmapData;
			int length=alphaColorMapDataRecord.colormapPixelData.Length;
			var colors=new Color[length];
			for(int j=0;j<length;j++){
				var colorIndex=alphaColorMapDataRecord.colormapPixelData[j];
				var rgba=alphaColorMapDataRecord.colorTableRGB[colorIndex];
				colors[j]=new Color(rgba.red/255.0f,rgba.green/255.0f,rgba.blue/255.0f,rgba.alpha/255.0f);
			}
			colors=flipVerticalBitmapColors(colors,defineBitsLossless2.bitmapWidth,defineBitsLossless2.bitmapHeight);
			texture.SetPixels(colors);
			texture.Apply();
		}else if(defineBitsLossless2.bitmapFormat==4||defineBitsLossless2.bitmapFormat==5){
			//AlphaBitmapDataRecord
			var alphaBitmapDataRecord=(AlphaBitmapDataRecord)defineBitsLossless2.zlibBitmapData;
			int length=alphaBitmapDataRecord.bitmapPixelData.Length;
			var colors=new Color[length];
			for(int j=0;j<length;j++){
				var argb=alphaBitmapDataRecord.bitmapPixelData[j];
				colors[j]=new Color(argb.red/255.0f,argb.green/255.0f,argb.blue/255.0f,argb.alpha/255.0f);
			}
			colors=flipVerticalBitmapColors(colors,defineBitsLossless2.bitmapWidth,defineBitsLossless2.bitmapHeight);
			texture.SetPixels(colors);
			texture.Apply();
		}
		var imageData=new ImageData();
		imageData.characterID=defineBitsLossless2.characterID;
		imageData.type=ImageType.Png;
		imageData.bytes=texture.EncodeToPNG();
		return imageData;
	}
	
	private ImageData getDefineBitsJPEG4ImageData(DefineBitsJPEG4Tag defineBitsJPEG4){
		var imageData=new ImageData();
		return imageData;
	}
	
	/// <summary>
	/// 垂直翻转位图颜色数据
	/// </summary>
	private Color[] flipVerticalBitmapColors(Color[] colors,ushort bitmapWidth,ushort bitmapHeight){
		Color[] tempColors=new Color[colors.Length];
		int i=bitmapHeight;
		while(--i>=0){
			var sourceStartIndex=i*bitmapWidth;
			var destStartIndex=(bitmapHeight-i-1)*bitmapWidth;
			Array.Copy(colors,sourceStartIndex,tempColors,destStartIndex,bitmapWidth);
		}
		return tempColors;
	}
	
}
