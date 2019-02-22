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
			//============= Control Tags =======
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
			case 64:
				tag=readEnableDubugger2(bytes,header);
				break;
			case 65:
				tag=readScriptLimitsTag(bytes,header);
				break;
			case 66:
				tag=readSetTabIndexTag(bytes,header);
				break;	
			case 69:
				tag=readFileAttributesTag(bytes,header);
				break;
			case 71:
				tag=readImportAssets2Tag(bytes,header);
				break;
			case 76:
				tag=readSymbolClassTag(bytes,header);
				break;
			case 77:
				tag=readMetadataTag(bytes,header);
				break;
			case 78:
				tag=readDefineScalingGridTag(bytes,header);
				break;
			case 86:
				tag=readDefineSceneAndFrameLabelDataTag(bytes,header);
				break;
			//============= Shape Tags =======
			case 2:
				tag=readDefineShape(bytes,header);
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

	private EnableDubugger2Tag readEnableDubugger2(SwfByteArray bytes,TagHeaderRecord header){
		var tag=new EnableDubugger2Tag();
		tag.reserved=bytes.readUI16();
		tag.password=bytes.readString();
		return tag;
	}

	private ScriptLimitsTag readScriptLimitsTag(SwfByteArray bytes,TagHeaderRecord header){
		var tag=new ScriptLimitsTag();
		tag.maxRecursionDepth=bytes.readUI16();
		tag.scriptTimeoutSeconds=bytes.readUI16();
		return tag;
	}
	
	private SetTabIndexTag readSetTabIndexTag(SwfByteArray bytes,TagHeaderRecord header){
		var tag=new SetTabIndexTag();
		tag.depth=bytes.readUI16();
		tag.tabIndex=bytes.readUI16();
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

	private ImportAssets2Tag readImportAssets2Tag(SwfByteArray bytes,TagHeaderRecord header){
		var tag=new ImportAssets2Tag();
		bytes.readUI8();
		bytes.readUI8();
		ushort count=bytes.readUI16();
		var list=new ImportAssets2Record[count];
		for(ushort i=0;i<count;i++){
			var record=new ImportAssets2Record();
			record.tag=bytes.readUI16();
			record.name=bytes.readString();
			list[i]=record;
		}
		tag.list=list;
		return tag;
	}

	private SymbolClassTag readSymbolClassTag(SwfByteArray bytes,TagHeaderRecord header){
		var tag=new SymbolClassTag();
		tag.numSymbols=bytes.readUI16();
		var list=new SymbolClassRecord[tag.numSymbols];
		for(ushort i=0;i<tag.numSymbols;i++){
			var record=new SymbolClassRecord();
			record.tag=bytes.readUI16();
			record.name=bytes.readString();
			list[i]=record;
		}
		return tag;
	}

	private MetadataTag readMetadataTag(SwfByteArray bytes,TagHeaderRecord header){
		var tag=new MetadataTag();
		tag.metadata=bytes.readString();
		return tag;
	}

	private DefineScalingGridTag readDefineScalingGridTag(SwfByteArray bytes,TagHeaderRecord header){
		var tag=new DefineScalingGridTag();
		tag.characterId=bytes.readUI16();
		tag.splitter=readRectangleRecord(bytes);
		return tag;
	}

	private DefineSceneAndFrameLabelDataTag readDefineSceneAndFrameLabelDataTag(SwfByteArray bytes,TagHeaderRecord header){
		var tag=new DefineSceneAndFrameLabelDataTag();
		tag.sceneCount=bytes.readEncodedUI32();
		var defineSceneList=new DefineSceneRecord[tag.sceneCount];
		for(uint i=0;i<tag.sceneCount;i++){
			var record=new DefineSceneRecord();
			record.offset=bytes.readEncodedUI32();
			record.name=bytes.readString();
			defineSceneList[i]=record;
		}
		tag.frameLabelCount=bytes.readEncodedUI32();
		var frameLabelList=new FramelabelRecord[tag.frameLabelCount];
		for(uint i=0;i<tag.frameLabelCount;i++){
			var record=new FramelabelRecord();
			record.frameNum=bytes.readEncodedUI32();
			record.frameLabel=bytes.readString();
			frameLabelList[i]=record;
		}
		return tag;
	}

	private DefineShapeTag readDefineShape(SwfByteArray bytes,TagHeaderRecord header){
		var tag=new DefineShapeTag();
		tag.shapeId=bytes.readUI16();
		tag.shapeBounds=readRectangleRecord(bytes);
		tag.shapes=readShapeWithStyleRecord(bytes,1);
		return tag;
	}

	private UnknownTag readUnknownTag(SwfByteArray bytes,TagHeaderRecord header){
		UnknownTag tag=new UnknownTag();
		if(header.length>0){
			tag.content=bytes.readBytes((int)header.length);
		}
		return tag;
	}












	private SwfHeader readSwfHeader(SwfByteArray bytes){
		var header=new SwfHeader();
		header.signature=bytes.readStringWithLength(3);
		header.fileVersion=bytes.readUI8();
		header.uncompressedSize=bytes.readUI32();
		if(header.signature==SwfHeader.COMPRESSED_SIGNATURE){
			bytes.decompress();
		}
		header.frameSize=readRectangleRecord(bytes);
		header.frameRate=bytes.readFixed8_8();
		header.frameCount=bytes.readUI16();
		return header;
	}

	protected TagHeaderRecord readTagHeaderRecord(SwfByteArray bytes){
		var record=new TagHeaderRecord();
		ushort tagInfo=bytes.readUI16();
		record.type=(uint)(tagInfo>>6);
		uint length=(uint)(tagInfo & ((1<<6)-1));
		if(length==0x3F){
			length=bytes.readUI32();
		}
		record.length=length;
		return record;
	}

	private RectangleRecord readRectangleRecord(SwfByteArray bytes){
		bytes.alignBytes();
		var record=new RectangleRecord();
		uint nBits=bytes.readUB(5);
		record.xMin=bytes.readSB(nBits);
		record.xMax=bytes.readSB(nBits);
		record.yMin=bytes.readSB(nBits);
		record.yMax=bytes.readSB(nBits);
		return record;
	}

	private MatrixRecord readMatrixRecord(SwfByteArray bytes){
		var record=new MatrixRecord();
		record.hasScale=bytes.readFlag();
		if(record.hasScale){
			record.nScaleBits=(byte)bytes.readUB(5);
			record.scaleX=bytes.readFB(record.nScaleBits);
			record.scaleY=bytes.readFB(record.nScaleBits);
		}
		record.hasRotate=bytes.readFlag();
		if(record.hasRotate){
			record.nRotateBits=(byte)bytes.readUB(5);
			record.rotateSkew0=bytes.readFB(record.nRotateBits);
			record.rotateSkew1=bytes.readFB(record.nRotateBits);
		}
		record.nTranslateBits=(byte)bytes.readUB(5);
		record.translateX=bytes.readSB(record.nTranslateBits);
		record.translateY=bytes.readSB(record.nTranslateBits);
		return record;
	}

	private RGBRecord readRGBRecord(SwfByteArray bytes){
		var record=new RGBRecord();
		record.red=bytes.readUI8();
		record.green=bytes.readUI8();
		record.blue=bytes.readUI8();
		return record;
	}

	private RGBARecord readRGBARecord(SwfByteArray bytes){
		var record=new RGBARecord();
		record.red=bytes.readUI8();
		record.green=bytes.readUI8();
		record.blue=bytes.readUI8();
		record.alpha=bytes.readUI8();
		return record;
	}

	private GradientRecord readGradientRecord(SwfByteArray bytes,byte shapeType){
		var record=new GradientRecord();
		record.spreadMode=(byte)bytes.readUB(2);
		record.interpolationMode=(byte)bytes.readUB(2);
		record.numGradients=(byte)bytes.readUB(4);
		var list=new GradRecord[record.numGradients];
		for(byte i=0;i<record.numGradients;i++){
			list[i]=readGradRecord(bytes,shapeType);
		}
		record.gradientRecords=list;
		return record;
	}

	private FocalGradientRecord readFocalGradientRecord(SwfByteArray bytes,byte shapeType){
		var record=new FocalGradientRecord();
		record.spreadMode=(byte)bytes.readUB(2);
		record.interpolationMode=(byte)bytes.readUB(2);
		record.numGradients=(byte)bytes.readUB(4);
		var list=new GradRecord[record.numGradients];
		for(byte i=0;i<record.numGradients;i++){
			list[i]=readGradRecord(bytes,shapeType);
		}
		record.gradientRecords=list;
		record.focalPoint=bytes.readFixed8_8();
		return record;
	}

	private GradRecord readGradRecord(SwfByteArray bytes,byte shapeType){
		var record=new GradRecord();
		record.ratio=bytes.readUI8();
		if(shapeType==1||shapeType==2){//RGB(Shape1 or Shape2)
			record.color=readRGBRecord(bytes);
		}else{//RGBA(Shape3)
			record.color=readRGBARecord(bytes);
		}
		return record;
	}

	private ShapeWithStyleRecord readShapeWithStyleRecord(SwfByteArray bytes,byte shapeType){
		var record=new ShapeWithStyleRecord();
		record.fillStyles=readFillStyleArrayRecord(bytes,shapeType);
		record.lineStyles=
		record.numFillBits=(byte)bytes.readUB(4);
		record.numLineBits=(byte)bytes.readUB(4);
		record.shapeRecords=
		return record;
	}

	private FillStyleArrayRecord readFillStyleArrayRecord(SwfByteArray bytes,byte shapeType){
		var record=new FillStyleArrayRecord();
		record.fillStyleCount=bytes.readUI8();
		uint count=record.fillStyleCount;
		if(record.fillStyleCount==0xFF){
			record.fillStyleCountExtended=bytes.readUI16();
			Debug.Log("readFillStyleArrayRecord(); fillStyleCountExtended:"+record.fillStyleCountExtended);
			count+=record.fillStyleCountExtended;
		}
		
		var list=new FillStyleRecord[count];
		for(uint i=0;i<count;i++){
			list[i]=readFillStyleRecord(bytes,shapeType);
		}
		record.fillStyles=list;
		return record;
	}

	private FillStyleRecord readFillStyleRecord(SwfByteArray bytes,byte shapeType){
		var record=new FillStyleRecord();

		byte type=bytes.readUI8();
		record.fillStyleType=type;

		if(type==0x00){
			record.color=readRGBARecord(bytes);
		}else{
			record.color=readRGBRecord(bytes);
		}

		if(type==0x10||type==0x12||type==0x13){
			record.gradientMatrix=readMatrixRecord(bytes);
		}

		if(type==0x10||type==0x12){
			record.gradient=readGradientRecord(bytes,shapeType);
		}else if(type==0x13){
			record.gradient=readFocalGradientRecord(bytes,shapeType);
		}

		if(type==0x40||type==0x41||type==0x42||type==0x43){
			record.bitmapId=bytes.readUI16();
			record.bitmapMatrix=readMatrixRecord(bytes);
		}
		return record;
	}

	private LineStyleArrayRecord readLineStyleArrayRecord(SwfByteArray bytes,byte shapeType){
		var record=new LineStyleArrayRecord();
		record.lineStyleCount=bytes.readUI8();
		uint count=record.lineStyleCount;
		if(record.lineStyleCount==0xFF){
			record.lineStyleCountExtended=bytes.readUI16();
			count+=record.lineStyleCountExtended;
		}
		var list=new ArrayList();
		if(shapeType==1||shapeType==2||shapeType==3){
			for(uint i=0;i<count;i++){
				list[i]=readLineStyleRecord(bytes,shapeType);
			}
		}else if(shapeType==4){
			for(uint i=0;i<count;i++){
				list[i]=readLineStyle2Record(bytes,shapeType);
			}
		}
		record.lineStyles=list;
		return record;
	}

	private LineStyleRecord readLineStyleRecord(SwfByteArray bytes,byte shapeType){
		var record=new LineStyleRecord();
		record.width=bytes.readUI16();
		if(shapeType==1||shapeType==2){//RGB(Shape1 or Shape2)
			record.color=readRGBRecord(bytes);
		}else{//RGBA(Shape3)
			record.color=readRGBARecord(bytes);
		}
		return record;
	}

	private LineStyle2Record readLineStyle2Record(SwfByteArray bytes,byte shapeType){
		var record=new LineStyle2Record();
		record.width=bytes.readUI16();
		record.startCapStyle=(byte)bytes.readUB(2);
		record.joinStyle=(byte)bytes.readUB(2);
		record.hasFillFlag=bytes.readFlag();
		record.noHScaleFlag=bytes.readFlag();
		record.noVScaleFlag=bytes.readFlag();
		record.pixelHintingFlag=bytes.readFlag();
		record.reserved=(byte)bytes.readUB(5);
		record.noClose=bytes.readFlag();
		record.endCapStyle=(byte)bytes.readUB(2);
		if(record.joinStyle==2){
			record.miterLimitFactor=bytes.readUI16();
		}
		if(record.hasFillFlag){
			record.color=readRGBARecord(bytes);
			record.fillType=readFillStyleRecord(bytes,shapeType);
		}
		return record;
	}
	
	
}
