using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class SwfReader{
	
	public SwfReadResult read(SwfByteArray bytes){
		var tags=new List<SwfTag>();
		//
		var header=readSwfHeader(bytes);
		//
		var swf=new Swf();
		swf.header=header;
		swf.tags=tags;
		var readResult=new SwfReadResult();
		readResult.swf=swf;

		return readResult;
	}

	private SwfHeader readSwfHeader(SwfByteArray bytes){
		var header=new SwfHeader();
		header.signature = bytes.readStringWithLength(3);
		header.fileVersion = bytes.readUI8();
		header.uncompressedSize = bytes.readUI32();
		if(header.signature == SwfHeader.COMPRESSED_SIGNATURE){
			bytes.decompress();
		}
		header.frameSize = readRectangleRecord(bytes);
		header.frameRate = bytes.readFixed8_8();
		header.frameCount = bytes.readUI16();
		return header;
	}

	private RectangleRecord readRectangleRecord(SwfByteArray bytes){
		bytes.alignBytes();
		var record=new RectangleRecord();
		var nBits = bytes.readUB(5);
		record.xMin = bytes.readSB(nBits);
		record.xMax = bytes.readSB(nBits);
		record.yMin = bytes.readSB(nBits);
		record.yMax = bytes.readSB(nBits);
		return record;
	}

	
	
}
