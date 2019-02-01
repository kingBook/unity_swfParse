using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class SwfReader{
	
	public Swf read(SwfByteArray bytes){
		var swf=new Swf();
		swf.header=readSwfHeader(bytes);
		swf.tags=new List<SwfTag>();
		//
		while(bytes.getBytesAvailable()>0){
			TagHeaderRecord tagHeader = readTagHeaderRecord(bytes);
				
			long startPosition = bytes.getBytePosition();
			long expectedEndPosition = startPosition + tagHeader.length;
				
			SwfTag tag = readTag(bytes, tagHeader);
			tag.header = tagHeader;
			swf.tags.Add(tag);

			bytes.alignBytes();
			long newPosition = bytes.getBytePosition();
			
			bytes.setBytePosition(expectedEndPosition);
			
			if(tag is EndTag){
				break;
			}
		}
		return swf;
	}

	private SwfTag readTag(SwfByteArray bytes,TagHeaderRecord header){
		SwfTag tag;
		switch(header.type){

			//============= control Tags =======
			case 9:
				tag=readSetBackgroundColorTag(bytes,header);
				break;
			case 43:
				tag=readFrameLabelTag(bytes,header);
				break;
			case 24:
				tag=readProtectTag(bytes,header);
				break;
			case 0:
				tag=readEndTag(bytes,header);
				break;
			case 56:
				tag=readExportAssetsTag(bytes,header);
				break;
			case 57:
				tag=readImportAssetsTag(bytes,header);
				break;

				
			case 69:
				tag=readFileAttributesTag(bytes,header);
				break;
			default:
				tag=readUnknownTag(bytes,header);
				break;
		}
		return tag;
	}

	private SetBackgroundColorTag readSetBackgroundColorTag(SwfByteArray bytes,TagHeaderRecord header){
		var tag=new SetBackgroundColorTag();
		tag.backgroundColor=readRGBRecord(bytes);
		return tag;
	}

	private FrameLabelTag readFrameLabelTag(SwfByteArray bytes,TagHeaderRecord header){
		var tag=new FrameLabelTag();
		tag.name=bytes.readString();
		tag.namedAnchorFlag=bytes.readUI8();
		return tag;
	}

	private ProtectTag readProtectTag(SwfByteArray bytes,TagHeaderRecord header){
		var tag=new ProtectTag();
		return tag;
	}

	private EndTag readEndTag(SwfByteArray bytes,TagHeaderRecord header){
		var tag=new EndTag();
		return tag;
	}

	private ExportAssetsTag readExportAssetsTag(SwfByteArray bytes,TagHeaderRecord header){
		var tag=new ExportAssetsTag();

		ushort count=bytes.readUI16();
		var list=new ExportAssetRecord[count];
		for(ushort i=0;i<count;i++){
			var record=new ExportAssetRecord();
			record.tag=bytes.readUI16();
			record.name=bytes.readString();
			list[i]=record;
		}

		tag.list=list;

		return tag;
	}

	private ImportAssetsTag readImportAssetsTag(SwfByteArray bytes,TagHeaderRecord header){
		var tag=new ImportAssetsTag();
		return tag;
	}

	private FileAttributesTag readFileAttributesTag(SwfByteArray bytes,TagHeaderRecord header){
		var tag=new FileAttributesTag();
		bytes.readUB(1);
		tag.useDirectBlit = bytes.readFlag();
		tag.useGPU = bytes.readFlag();
		tag.hasMetadata = bytes.readFlag();
		tag.actionScript3 = bytes.readFlag();
		bytes.readUB(2);
		tag.useNetwork = bytes.readFlag();
		bytes.readUB(24);
		return tag;
	}

	private UnknownTag readUnknownTag(SwfByteArray bytes,TagHeaderRecord header){
		UnknownTag tag = new UnknownTag();
		if(header.length > 0){
			tag.content=bytes.readBytes((int)header.length);
		}
		return tag;
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

	protected TagHeaderRecord readTagHeaderRecord(SwfByteArray bytes){
		var record = new TagHeaderRecord();
		ushort tagInfo = bytes.readUI16();
		record.type = (uint)(tagInfo >> 6);
		uint length = (uint)( tagInfo & ((1 << 6) - 1) );
		if(length == 0x3F){
			length = bytes.readUI32();
		}
		record.length = length;
		return record;
	}

	private RectangleRecord readRectangleRecord(SwfByteArray bytes){
		bytes.alignBytes();
		var record=new RectangleRecord();
		uint nBits = bytes.readUB(5);
		record.xMin = bytes.readSB(nBits);
		record.xMax = bytes.readSB(nBits);
		record.yMin = bytes.readSB(nBits);
		record.yMax = bytes.readSB(nBits);
		return record;
	}

	private RGBRecord readRGBRecord(SwfByteArray bytes){
		var record = new RGBRecord();
		record.red = bytes.readUI8();
		record.green = bytes.readUI8();
		record.blue = bytes.readUI8();
		return record;
	}

	
	
}
