using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SwfReader{
	
	public SwfReadResult read(SwfByteArray bytes){
		var header=new SwfHeader();
		var tags=new List<SwfTag>();
		//
		readSwfHeader(bytes,header);
		//
		var swf=new Swf();
		swf.header=header;
		swf.tags=tags;
		var readResult=new SwfReadResult();
		readResult.swf=swf;
		return readResult;
	}

	private void readSwfHeader(SwfByteArray bytes,SwfHeader header){
		header.signature = bytes.readStringWithLength(3);
		header.fileVersion = bytes.readUI8();
		header.uncompressedSize = bytes.readUI32();
		if(header.signature == SwfHeader.COMPRESSED_SIGNATURE){
			decompress(bytes);
		}
		/*header.frameSize = readRectangleRecord(context);
		header.frameRate = bytes.readFixed8_8();
		header.frameCount = bytes.readUI16();*/
	}

	private void decompress(SwfByteArray bytes){
		new ICSharpCode
	}

	
	
}
