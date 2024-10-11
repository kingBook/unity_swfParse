https://github.com/icsharpcode/SharpZipLib

http://www.zlib.net/

https://www.m2osw.com/swf_tag_definebitsjpeg3


https://zhuzhonghua.github.io/2012/12/swf9%E6%96%87%E4%BB%B6%E6%A0%BC%E5%BC%8F-%E5%BD%A2%E7%8A%B6-swf-file-format-version9-shape.html

https://www.thegestalt.org/flash/stuff/SWF%20Reference/SWFfilereference.htm

RemoveObject2Tag 需要导出但是没有characterID ，又不能继承 RemoveObjectTag


```cs
public static void CopyStream(System.IO.Stream input, System.IO.Stream output)
{
	byte[] buffer = new byte[2000];
	int len;
	while ((len = input.Read(buffer, 0, 2000)) > 0)
	{
		output.Write(buffer, 0, len);
	}
	output.Flush();
}

private void compressFile(string inFile, string outFile)
{
	System.IO.FileStream outFileStream = new System.IO.FileStream(outFile, System.IO.FileMode.Create);
	zlib.ZOutputStream outZStream = new zlib.ZOutputStream(outFileStream, zlib.zlibConst.Z_DEFAULT_COMPRESSION);
	System.IO.FileStream inFileStream = new System.IO.FileStream(inFile, System.IO.FileMode.Open);			
	try
	{
		CopyStream(inFileStream, outZStream);
	}
	finally
	{
		outZStream.Close();
		outFileStream.Close();
		inFileStream.Close();
	}
}

private void decompressFile(string inFile, string outFile)
{
	System.IO.FileStream outFileStream = new System.IO.FileStream(outFile, System.IO.FileMode.Create);
	zlib.ZOutputStream outZStream = new zlib.ZOutputStream(outFileStream);
	System.IO.FileStream inFileStream = new System.IO.FileStream(inFile, System.IO.FileMode.Open);			
	try
	{
		CopyStream(inFileStream, outZStream);
	}
	finally
	{
		outZStream.Close();
		outFileStream.Close();
		inFileStream.Close();
	}
}
```

==============================================================================
// swf 显示列表
.swf
    ├ DefineSprite
      ├ frame1
        ├ PlaceObject2
          ├ DefineShape
            ├ DefineBitsLossless2（位图数据）
==============================================================================

