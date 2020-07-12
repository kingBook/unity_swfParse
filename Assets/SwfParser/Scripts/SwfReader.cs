﻿using UnityEngine;
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
			long preHeaderStart=bytes.getBytePosition();
			TagHeaderRecord tagHeader=readTagHeaderRecord(bytes);
				
			long startPosition=bytes.getBytePosition();
			long expectedEndPosition=startPosition+tagHeader.length;
			//Debug2.Log("type:"+tagHeader.type,"preHeaderStart:"+preHeaderStart,"length:"+tagHeader.length);
			SwfTag tag=readTag(bytes,tagHeader);
			tag.header=tagHeader;
			swf.tags.Add(tag);
			
			bytes.alignBytes();
			long newPosition=bytes.getBytePosition();
			
			bytes.setBytePosition(expectedEndPosition);
			
			if(tag is EndTag){
				break;
			}
		}
		return swf;
	}

	private SwfTag[] readControlTags(SwfByteArray bytes){
		var tags=new List<SwfTag>();
		while(true){
			var header=readTagHeaderRecord(bytes);
			long startPosition=bytes.getBytePosition();
			long expectedEndPosition=startPosition+header.length;
			var tag=readTag(bytes,header);
			tag.header=header;
			tags.Add(tag);
			bytes.setBytePosition(expectedEndPosition);
			if(tag is EndTag)break;
		}
		return tags.ToArray();
	}

	private SwfTag readTag(SwfByteArray bytes,TagHeaderRecord header){
		SwfTag tag;
		switch(header.type){
			//============= Display list tags =======
			case 4:
				tag=readPlaceObjectTag(bytes,header);
				break;
			case 26:
				tag=readPlaceObject2Tag(bytes,header);
				break;
			case 70:
				tag=readPlaceObject3Tag(bytes,header);
				break;
			case 5:
				tag=readRemoveObjectTag(bytes,header);
				break;
			case 28:
				tag=readRemoveObject2Tag(bytes,header);
				break;
			case 1:
				tag=readShowFrameTag(bytes,header);
				break;
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
				tag=readDefineShapeTag(bytes,header);
				break;
			case 22:
				tag=readDefineShape2Tag(bytes,header);
				break;
			case 32:
				tag=readDefineShape3Tag(bytes,header);
				break;
			case 83:
				tag=readDefineShape4Tag(bytes,header);
				break;
			//============= Bitmaps =======
			case 6:
				tag=readDefineBitsTag(bytes,header);
				break;
			case 8:
				tag=readJPEGTablesTag(bytes,header);
				break;
			case 21:
				tag=readDefineBitsJPEG2Tag(bytes,header);
				break;
			case 35:
				tag=readDefineBitsJPEG3Tag(bytes,header);
				break;
			case 20:
				tag=readDefineBitsLosslessTag(bytes,header);
				break;
			case 36:
				tag=readDefineBitsLossless2Tag(bytes,header);
				break;
			/*case 90:
				tag=readDefineBitsJPEG4Tag(bytes,header);
				break;*/
			//============= Shape Morphing =======
			case 46:
				tag=readDefineMorphShapeTag(bytes,header);
				break;
			/*case 84:
				tag=readDefineMorphShape2Tag(bytes,header);
				break;*/
			//============= Fonts and Text =======
			case 10:
				tag=readDefineFontTag(bytes,header);
				break;
			case 11:
				tag=readDefineTextTag(bytes,header);
				break;
			case 33:
				tag=readDefineText2Tag(bytes,header);
				break;
			//============= Buttons =======
			/*case 7:
				tag=readDefineButtonTag(bytes,header);
				break;*/
			case 34:
				tag=readDefineButton2Tag(bytes,header);
				break;
			//============= Sprites and Movie Clips =======
			case 39:
				tag=readDefineSpriteTag(bytes,header);
				break;

			default:
				tag=readUnknownTag(bytes,header);
				break;
		}
		return tag;
	}

	private PlaceObjectTag readPlaceObjectTag(SwfByteArray bytes,TagHeaderRecord header){
		var tag=new PlaceObjectTag();
		var originalPos=bytes.getBytePosition();
		tag.characterId=bytes.readUI16();
		tag.depth=bytes.readUI16();
		tag.matrix=readMatrixRecord(bytes);
		if(header.length>bytes.getBytePosition()-originalPos){
			tag.colorTransform=readCXFormRecord(bytes);
		}
		return tag;
	}

	private PlaceObject2Tag readPlaceObject2Tag(SwfByteArray bytes,TagHeaderRecord header){
		var tag=new PlaceObject2Tag();
		tag.placeFlagHasClipActions=bytes.readFlag();
		tag.placeFlagHasClipDepth=bytes.readFlag();
		tag.placeFlagHasName=bytes.readFlag();
		tag.placeFlagHasRatio=bytes.readFlag();
		tag.placeFlagHasColorTransform=bytes.readFlag();
		tag.placeFlagHasMatrix=bytes.readFlag();
		tag.placeFlagHasCharacter=bytes.readFlag();
		tag.placeFlagMove=bytes.readFlag();
		tag.depth=bytes.readUI16();
		if(tag.placeFlagHasCharacter){
			tag.characterId=bytes.readUI16();
		}
		if(tag.placeFlagHasMatrix){
			tag.matrix=readMatrixRecord(bytes);
		}
		if(tag.placeFlagHasColorTransform){
			tag.colorTransform=readCXFormWithAlphaRecord(bytes);
		}
		if(tag.placeFlagHasRatio){
			tag.ratio=bytes.readUI16();
		}
		if(tag.placeFlagHasName){
			tag.name=bytes.readString();
		}
		if(tag.placeFlagHasClipDepth){
			tag.clipDepth=bytes.readUI16();
		}
		/*if(tag.placeFlagHasClipActions){
			tag.clipActions=
		}*/
		return tag;
	}

	private PlaceObject3Tag readPlaceObject3Tag(SwfByteArray bytes,TagHeaderRecord header){
		var tag=new PlaceObject3Tag();
		tag.placeFlagHasClipActions=bytes.readFlag();
		tag.placeFlagHasClipDepth=bytes.readFlag();
		tag.placeFlagHasName=bytes.readFlag();
		tag.placeFlagHasRatio=bytes.readFlag();
		tag.placeFlagHasColorTransform=bytes.readFlag();
		tag.placeFlagHasMatrix=bytes.readFlag();
		tag.placeFlagHasCharacter=bytes.readFlag();
		tag.placeFlagMove=bytes.readFlag();
		tag.reserved=(byte)bytes.readUB(1);
		tag.placeFlagOpaqueBackground=bytes.readFlag();
		tag.placeFlagHasVisible=bytes.readFlag();
		tag.placeFlagHasImage=bytes.readFlag();
		tag.placeFlagHasClassName=bytes.readFlag();
		tag.placeFlagHasCacheAsBitmap=bytes.readFlag();
		tag.placeFlagHasBlendMode=bytes.readFlag();
		tag.placeFlagHasFilterList=bytes.readFlag();
		tag.depth=bytes.readUI16();
		if(tag.placeFlagHasClassName||(tag.placeFlagHasImage&&tag.placeFlagHasCharacter)){
			tag.className=bytes.readString();
		}
		if(tag.placeFlagHasCharacter){
			tag.characterId=bytes.readUI16();
		}
		if(tag.placeFlagHasMatrix){
			tag.matrix=readMatrixRecord(bytes);
		}
		if(tag.placeFlagHasColorTransform){
			tag.colorTransform=readCXFormWithAlphaRecord(bytes);
		}
		if(tag.placeFlagHasRatio){
			tag.ratio=bytes.readUI16();
		}
		if(tag.placeFlagHasName){
			tag.name=bytes.readString();
		}
		if(tag.placeFlagHasClipDepth){
			tag.clipDepth=bytes.readUI16();
		}
		if(tag.placeFlagHasFilterList){
			tag.surfaceFilterList=readFilterListRecord(bytes);
		}
		if(tag.placeFlagHasBlendMode){
			tag.blendMode=bytes.readUI8();
		}
		if(tag.placeFlagHasCacheAsBitmap){
			tag.bitmapCache=bytes.readUI8();
		}
		if(tag.placeFlagHasVisible){
			tag.visible=bytes.readUI8();
			tag.backgroundColor=readRGBARecord(bytes);
		}
		/*if(tag.placeFlagHasClipActions){
			tag.placeFlagHasClipActions=
		}*/
		return tag;
	}

	private RemoveObjectTag readRemoveObjectTag(SwfByteArray bytes,TagHeaderRecord header){
		var tag=new RemoveObjectTag();
		tag.characterId=bytes.readUI16();
		tag.depth=bytes.readUI16();
		return tag;
	}

	private RemoveObject2Tag readRemoveObject2Tag(SwfByteArray bytes,TagHeaderRecord header){
		var tag=new RemoveObject2Tag();
		tag.depth=bytes.readUI16();
		return tag;
	}

	private ShowFrameTag readShowFrameTag(SwfByteArray bytes,TagHeaderRecord header){
		var tag=new ShowFrameTag();
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
		tag.list=list;
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
		tag.defineSceneList=defineSceneList;

		tag.frameLabelCount=bytes.readEncodedUI32();
		var frameLabelList=new FramelabelRecord[tag.frameLabelCount];
		for(uint i=0;i<tag.frameLabelCount;i++){
			var record=new FramelabelRecord();
			record.frameNum=bytes.readEncodedUI32();
			record.frameLabel=bytes.readString();
			frameLabelList[i]=record;
		}
		tag.frameLabelList=frameLabelList;
		return tag;
	}

	private DefineShapeTag readDefineShapeTag(SwfByteArray bytes,TagHeaderRecord header){
		var tag=new DefineShapeTag();
		tag.shapeId=bytes.readUI16();
		tag.shapeBounds=readRectangleRecord(bytes);
		tag.shapes=readShapeWithStyleRecord(bytes,1);
		return tag;
	}

	private DefineShape2Tag readDefineShape2Tag(SwfByteArray bytes,TagHeaderRecord header){
		var tag=new DefineShape2Tag();
		tag.shapeId=bytes.readUI16();
		tag.shapeBounds=readRectangleRecord(bytes);
		tag.shapes=readShapeWithStyleRecord(bytes,2);
		return tag;
	}

	private DefineShape3Tag readDefineShape3Tag(SwfByteArray bytes,TagHeaderRecord header){
		var tag=new DefineShape3Tag();
		tag.shapeId=bytes.readUI16();
		tag.shapeBounds=readRectangleRecord(bytes);
		tag.shapes=readShapeWithStyleRecord(bytes,3);
		return tag;
	}

	private DefineShape4Tag readDefineShape4Tag(SwfByteArray bytes,TagHeaderRecord header){
		var tag=new DefineShape4Tag();
		tag.shapeId=bytes.readUI16();
		tag.shapeBounds=readRectangleRecord(bytes);
		tag.edgeBounds=readRectangleRecord(bytes);
		tag.reserved=(byte)bytes.readUB(5);
		tag.usesFillWindingRule=bytes.readFlag();
		tag.usesNonScalingStrokes=bytes.readFlag();
		tag.usesScalingStrokes=bytes.readFlag();
		tag.shapes=readShapeWithStyleRecord(bytes,4);
		return tag;
	}

	private DefineBitsTag readDefineBitsTag(SwfByteArray bytes,TagHeaderRecord header){
		var tag=new DefineBitsTag();
		tag.characterID=bytes.readUI16();
		int length=(int)header.length-2;
		if(length>0){
			tag.jpegData=bytes.readBytes(length);
		}
		return tag;
	}
	
	private JPEGTablesTag readJPEGTablesTag(SwfByteArray bytes,TagHeaderRecord header){
		var tag=new JPEGTablesTag();
		int length=(int)header.length;
		if(length>0){
			tag.jpegData=bytes.readBytes(length);
		}
		return tag;
	}
	
	private DefineBitsJPEG2Tag readDefineBitsJPEG2Tag(SwfByteArray bytes,TagHeaderRecord header){
		var tag=new DefineBitsJPEG2Tag();
		tag.characterID=bytes.readUI16();
		int length=(int)header.length-2;
		if(length>0){
			tag.imageData=bytes.readBytes(length);
		}
		return tag;
	}

	private DefineBitsJPEG3Tag readDefineBitsJPEG3Tag(SwfByteArray bytes,TagHeaderRecord header){
		var tag=new DefineBitsJPEG3Tag();
		long startPosition = bytes.getBytePosition();
		tag.characterID=bytes.readUI16();
		tag.alphaDataOffset=bytes.readUI32();
		if(tag.alphaDataOffset>0){
			tag.imageData=bytes.readBytes((int)tag.alphaDataOffset);
		}
		int bytesRemaining=(int)(header.length - (bytes.getBytePosition() - startPosition));
		if(bytesRemaining>0){
			tag.bitmapAlphaData=bytes.readBytes(bytesRemaining);
		}
		return tag;
	}
	
	private DefineBitsLosslessTag readDefineBitsLosslessTag(SwfByteArray bytes,TagHeaderRecord header){
		long startPosition=bytes.getBytePosition();
		var tag=new DefineBitsLosslessTag();
		tag.characterID=bytes.readUI16();
		tag.bitmapFormat=bytes.readUI8();
		tag.bitmapWidth=bytes.readUI16();
		tag.bitmapHeight=bytes.readUI16();
		if(tag.bitmapFormat==3){
			tag.bitmapColorTableSize=bytes.readUI8();
		}
		if(tag.bitmapFormat==3||tag.bitmapFormat==4||tag.bitmapFormat==5){
			byte[] unzippedData=null;
			long bytesRead=bytes.getBytePosition()-startPosition;
			int remainingBytes=(int)(header.length-bytesRead);
			if(remainingBytes>0){
				unzippedData=bytes.readBytes(remainingBytes);
			}
			unzippedData=ZlibUtil.deCompressBytes(unzippedData);
			var unzippedSwfArray=new SwfByteArray(unzippedData);
			if(tag.bitmapFormat==3){
				uint imageDataSize=(uint)(tag.bitmapWidth*tag.bitmapHeight);
				tag.zlibBitmapData=readColorMapDataRecord(unzippedSwfArray, (uint)(tag.bitmapColorTableSize + 1), imageDataSize);
			}else if(tag.bitmapFormat==4||tag.bitmapFormat==5){
				uint imageDataSize=(uint)(tag.bitmapWidth*tag.bitmapHeight);
				tag.zlibBitmapData=readBitmapDataRecord(unzippedSwfArray,tag.bitmapFormat,imageDataSize);
			}
		}
		return tag;
	}

	private DefineBitsLossless2Tag readDefineBitsLossless2Tag(SwfByteArray bytes,TagHeaderRecord header){
		long startPosition=bytes.getBytePosition();
		var tag=new DefineBitsLossless2Tag();
		tag.characterID=bytes.readUI16();
		tag.bitmapFormat=bytes.readUI8();
		tag.bitmapWidth=bytes.readUI16();
		tag.bitmapHeight=bytes.readUI16();
		if(tag.bitmapFormat==3){
			tag.bitmapColorTableSize=bytes.readUI8();
		}
		if(tag.bitmapFormat==3||tag.bitmapFormat==4||tag.bitmapFormat==5){
			byte[] unzippedData=null;
			long bytesRead=bytes.getBytePosition()-startPosition;
			int remainingBytes=(int)(header.length-bytesRead);
			if(remainingBytes>0){
				unzippedData=bytes.readBytes(remainingBytes);
			}
			unzippedData=ZlibUtil.deCompressBytes(unzippedData);
			var unzippedSwfArray=new SwfByteArray(unzippedData);
			if(tag.bitmapFormat==3){
				//uint imageDataSize=(uint)((tag.bitmapWidth + (8 - (tag.bitmapWidth % 8))) * tag.bitmapHeight);//此代码经验证是错的
				uint imageDataSize=(uint)(tag.bitmapWidth*tag.bitmapHeight);
				tag.zlibBitmapData=readAlphaColorMapDataRecord(unzippedSwfArray, (uint)(tag.bitmapColorTableSize + 1), imageDataSize);
			}else if(tag.bitmapFormat==4||tag.bitmapFormat==5){
				uint imageDataSize=(uint)(tag.bitmapWidth*tag.bitmapHeight);
				tag.zlibBitmapData=readAlphaBitmapDataRecord(unzippedSwfArray,imageDataSize);
			}
			unzippedSwfArray.close();
		}
		return tag;
	}

	private DefineMorphShapeTag readDefineMorphShapeTag(SwfByteArray bytes,TagHeaderRecord header){
		var tag=new DefineMorphShapeTag();
		tag.characterId=bytes.readUI16();
		tag.startBounds=readRectangleRecord(bytes);
		tag.endBounds=readRectangleRecord(bytes);
		tag.offset=bytes.readUI32();
		tag.morphFillStyles=readMorphFillStyleArrayRecord(bytes);
		tag.morphLineStyles=readMorphLineStyleArrayRecord(bytes,1);
		tag.startEdges=readSHAPE(bytes,1);
		tag.endEdges=readSHAPE(bytes,1);
		return tag;
	}

	private DefineMorphShape2Tag readDefineMorphShape2Tag(SwfByteArray bytes,TagHeaderRecord header){
		var tag=new DefineMorphShape2Tag();
		tag.characterId=bytes.readUI16();
		tag.startBounds=readRectangleRecord(bytes);
		tag.endBounds=readRectangleRecord(bytes);
		tag.startEdgeBounds=readRectangleRecord(bytes);
		tag.endEdgeBounds=readRectangleRecord(bytes);
		tag.reserved=(byte)bytes.readUB(6);
		tag.usesNonScalingStrokes=bytes.readFlag();
		tag.usesScalingStrokes=bytes.readFlag();
		tag.offset=bytes.readUI32();
		tag.morphFillStyles=readMorphFillStyleArrayRecord(bytes);
		tag.morphLineStyles=readMorphLineStyleArrayRecord(bytes,2);
		tag.startEdges=readSHAPE(bytes,1);
		tag.endEdges=readSHAPE(bytes,1);
		return tag;
	}

	private DefineFontTag readDefineFontTag(SwfByteArray bytes,TagHeaderRecord header){
		var tag=new DefineFontTag();
		return tag;
	}

	private DefineTextTag readDefineTextTag(SwfByteArray bytes,TagHeaderRecord header){
		var tag=new DefineTextTag();
		tag.characterID=bytes.readUI16();
		tag.textBounds=readRectangleRecord(bytes);
		tag.textMatrix=readMatrixRecord(bytes);
		tag.glyphBits=bytes.readUI8();
		tag.advanceBits=bytes.readUI8();

		var textRecords=new List<TextRecord>();
		while(true){
			byte recordType=(byte)bytes.readUB(1);
			if(recordType==0){
				bytes.readUB(7);
				break;
			}else{
				textRecords.Add(readTextRecord(bytes,recordType,1,tag.glyphBits,tag.advanceBits));
			}
		}
		tag.textRecords=textRecords.ToArray();
		tag.endOfRecordsFlag=0;
		return tag;
	}

	private DefineText2Tag readDefineText2Tag(SwfByteArray bytes,TagHeaderRecord header){
		var tag=new DefineText2Tag();
		tag.characterID=bytes.readUI16();
		tag.textBounds=readRectangleRecord(bytes);
		tag.textMatrix=readMatrixRecord(bytes);
		tag.glyphBits=bytes.readUI8();
		tag.advanceBits=bytes.readUI8();

		var textRecords=new List<TextRecord>();
		while(true){
			byte recordType=(byte)bytes.readUB(1);
			if(recordType==0){
				bytes.readUB(7);
				break;
			}else{
				textRecords.Add(readTextRecord(bytes,recordType,2,tag.glyphBits,tag.advanceBits));
			}
		}
		tag.textRecords=textRecords.ToArray();
		tag.endOfRecordsFlag=0;
		return tag;
	}

	private DefineButtonTag readDefineButtonTag(SwfByteArray bytes,TagHeaderRecord header){
		var tag=new DefineButtonTag();
		
		return tag;
	}

	private DefineButton2Tag readDefineButton2Tag(SwfByteArray bytes,TagHeaderRecord header){
		var tag=new DefineButton2Tag();
		tag.buttonId=bytes.readUI16();
		tag.reservedFlags=(byte)bytes.readUB(7);
		tag.trackAsMenu=bytes.readFlag();
		tag.actionOffset=bytes.readUI16();

		var count=0;
		var btnRecords=new List<ButtonRecord>();
		while(true){
			byte reserved=(byte)bytes.readUB(2);
			bool hasBlendMode=bytes.readFlag();
			bool hasFilterList=bytes.readFlag();
			bool stateHitTest=bytes.readFlag();
			bool stateDown=bytes.readFlag();
			bool stateOver=bytes.readFlag();
			bool stateUp=bytes.readFlag();

			var isEnd=reserved==0 &&
			!hasBlendMode &&
			!hasFilterList &&
			!stateHitTest &&
			!stateDown &&
			!stateOver &&
			!stateUp;

			if(isEnd){
				break;
			}else{
				count++;
				btnRecords.Add(readButtonRecord(bytes,reserved,hasBlendMode,hasFilterList,stateHitTest,stateDown,stateOver,stateUp,2));
			}
		}
		tag.characters=btnRecords.ToArray();
		tag.characterEndFlag=0;
		//tag.actions = readButtonCondAction();
		return tag;
	}
	
	private DefineSpriteTag readDefineSpriteTag(SwfByteArray bytes,TagHeaderRecord header){
		var tag=new DefineSpriteTag();
		tag.spriteId=bytes.readUI16();
		tag.frameCount=bytes.readUI16();
		tag.controlTags=readControlTags(bytes);
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
	private ARGBRecord readARGBRecord(SwfByteArray bytes){
		var record=new ARGBRecord();
		record.alpha=bytes.readUI8();
		record.red=bytes.readUI8();
		record.green=bytes.readUI8();
		record.blue=bytes.readUI8();
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
		}else{//RGBA(Shape3,4)
			record.color=readRGBARecord(bytes);
		}
		return record;
	}

	private ShapeWithStyleRecord readShapeWithStyleRecord(SwfByteArray bytes,byte shapeType){
		var record=new ShapeWithStyleRecord();
		record.fillStyles=readFillStyleArrayRecord(bytes,shapeType);
		record.lineStyles=readLineStyleArrayRecord(bytes,shapeType);
		bytes.alignBytes();
		byte numFillBits=(byte)bytes.readUB(4);
		byte numLineBits=(byte)bytes.readUB(4);
		record.numFillBits=numFillBits;
		record.numLineBits=numLineBits;
		var list=new List<IShapeRecord>();
		while(true){
			var shapeRecord=readShapeRecord(bytes,numFillBits,numLineBits,shapeType);
			list.Add(shapeRecord);
			if(shapeRecord is StyleChangeRecord){
				if(((StyleChangeRecord)shapeRecord).stateNewStyles){
					numFillBits=((StyleChangeRecord)shapeRecord).numFillBits;
					numLineBits=((StyleChangeRecord)shapeRecord).numLineBits;
				}
			}
			if(shapeRecord is EndShapeRecord)break;
		}
		record.shapeRecords=list.ToArray();
		return record;
	}

	private FillStyleArrayRecord readFillStyleArrayRecord(SwfByteArray bytes,byte shapeType){
		/*var record=new FillStyleArrayRecord();
		record.fillStyleCount=bytes.readUI8();
		var list=new FillStyleRecord[record.fillStyleCount];
		for(uint i=0;i<record.fillStyleCount;i++){
			list[i]=readFillStyleRecord(bytes,shapeType);
		}
		record.fillStyles=list;*/
		var record=new FillStyleArrayRecord();
		record.fillStyleCount=bytes.readUI8();
		//if(shapeType==2||shapeType==3){
			if(record.fillStyleCount==0xFF){
				record.fillStyleCountExtended=bytes.readUI16();
			}
		//}
		var list=new FillStyleRecord[record.fillStyleCount];
		for(uint i=0;i<record.fillStyleCount;i++){
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
			if(shapeType==3||shapeType==4){
				record.color=readRGBARecord(bytes);
			}else if(shapeType==1||shapeType==2){
				record.color=readRGBRecord(bytes);
			}
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
		if(record.lineStyleCount==0xFF){
			record.lineStyleCountExtended=bytes.readUI16();
		}
		var list=new ILineStyleRecord[record.lineStyleCount];
		if(shapeType==1||shapeType==2||shapeType==3){
			for(int i=0;i<record.lineStyleCount;i++){
				list[i]=readLineStyleRecord(bytes,shapeType);
			}
		}else if(shapeType==4){
			for(int i=0;i<record.lineStyleCount;i++){
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
			record.miterLimitFactor=bytes.readFixed8_8();//bytes.readUI16();
		}

		if(!record.hasFillFlag){
			record.color=readRGBARecord(bytes);
		}else{
			record.fillType=readFillStyleRecord(bytes,shapeType);
		}
		return record;
	}
	
	private IShapeRecord readShapeRecord(SwfByteArray bytes,byte numFillBits,byte numLineBits,byte shapeType){
		IShapeRecord record;
		bool typeFlag=bytes.readFlag();
		long start=bytes.getBytePosition();
		if(!typeFlag){
			bool stateNewStyles=bytes.readFlag();
			bool stateLineStyle=bytes.readFlag();
			bool stateFillStyle1=bytes.readFlag();
			bool stateFillStyle0=bytes.readFlag();
			bool stateMoveTo=bytes.readFlag();

			bool isEndShapeRecord=!stateNewStyles&&!stateLineStyle&&!stateFillStyle1&&!stateFillStyle0&&!stateMoveTo;
			if(isEndShapeRecord){
				var endShapeRecord=new EndShapeRecord();
				endShapeRecord.typeFlag=typeFlag;
				endShapeRecord.endOfShape=bytes.readUB(5);
				record=endShapeRecord;
			}else{
				var styleChangeRecord=new StyleChangeRecord();
				styleChangeRecord.stateNewStyles=stateNewStyles;
				styleChangeRecord.stateLineStyle=stateLineStyle;
				styleChangeRecord.stateFillStyle1=stateFillStyle1;
				styleChangeRecord.stateFillStyle0=stateFillStyle0;
				styleChangeRecord.stateMoveTo=stateMoveTo;
				if(stateMoveTo){
					styleChangeRecord.moveBits=(byte)bytes.readUB(5);
					styleChangeRecord.moveDeltaX=bytes.readSB(styleChangeRecord.moveBits);
					styleChangeRecord.moveDeltaY=bytes.readSB(styleChangeRecord.moveBits);
				}
				if(stateFillStyle0){
					styleChangeRecord.fillStyle0=bytes.readUB(numFillBits);
				}
				if(stateFillStyle1){
					styleChangeRecord.fillStyle1=bytes.readUB(numFillBits);
				}
				if(stateLineStyle){
					styleChangeRecord.lineStyle=bytes.readUB(numLineBits);
				}
				//----------------------------------
				if(stateNewStyles){
					styleChangeRecord.fillStyles=readFillStyleArrayRecord(bytes,shapeType);
					styleChangeRecord.lineStyles=readLineStyleArrayRecord(bytes,shapeType);
					styleChangeRecord.numFillBits=(byte)bytes.readUB(4);
					styleChangeRecord.numLineBits=(byte)bytes.readUB(4);
				}
				//----------------------------------
				record=styleChangeRecord;
			}
		}else{
			bool straightFlag=bytes.readFlag();
			if(straightFlag){
				var straightEdgeRecord=new StraightEdgeRecord();
				straightEdgeRecord.typeFlag=typeFlag;
				straightEdgeRecord.straightFlag=straightFlag;
				straightEdgeRecord.numBits=(byte)bytes.readUB(4);
				straightEdgeRecord.generalLineFlag=bytes.readFlag();
				
				/*if(!straightEdgeRecord.generalLineFlag){
					straightEdgeRecord.vertLineFlag=bytes.readFlag();//(sbyte)bytes.readSB(1);
				}
				if(straightEdgeRecord.generalLineFlag||!straightEdgeRecord.vertLineFlag){
					straightEdgeRecord.deltaX=bytes.readSB((uint)straightEdgeRecord.numBits+2);
				}
				if(straightEdgeRecord.generalLineFlag||straightEdgeRecord.vertLineFlag){
					straightEdgeRecord.deltaY=bytes.readSB((uint)straightEdgeRecord.numBits+2);
				}*/
				
				if(straightEdgeRecord.generalLineFlag){
					straightEdgeRecord.deltaX=bytes.readSB((uint)straightEdgeRecord.numBits+2);
					straightEdgeRecord.deltaY=bytes.readSB((uint)straightEdgeRecord.numBits+2);
				}else{
					straightEdgeRecord.vertLineFlag=bytes.readFlag();//(sbyte)bytes.readSB(1);
					if(!straightEdgeRecord.vertLineFlag){
						straightEdgeRecord.deltaX=bytes.readSB((uint)straightEdgeRecord.numBits+2);
					}else{
						straightEdgeRecord.deltaY=bytes.readSB((uint)straightEdgeRecord.numBits+2);
					}
				}
				record=straightEdgeRecord;
			}else{
				var curvedEdgeRecord=new CurvedEdgeRecord();
				curvedEdgeRecord.typeFlag=typeFlag;
				curvedEdgeRecord.straightFlag=straightFlag;
				curvedEdgeRecord.numBits=(byte)bytes.readUB(4);
				curvedEdgeRecord.controlDeltaX=bytes.readSB((uint)curvedEdgeRecord.numBits+2);
				curvedEdgeRecord.controlDeltaY=bytes.readSB((uint)curvedEdgeRecord.numBits+2);
				curvedEdgeRecord.anchorDeltaX=bytes.readSB((uint)curvedEdgeRecord.numBits+2);
				curvedEdgeRecord.anchorDeltaY=bytes.readSB((uint)curvedEdgeRecord.numBits+2);
				record=curvedEdgeRecord;
			}
		}
		return record;
	}
	
	private ColorMapDataRecord readColorMapDataRecord(SwfByteArray bytes,uint colorTableSize,uint imageDataSize){
		var record=new ColorMapDataRecord();
		
		record.colorTableRGB=new RGBRecord[colorTableSize];
		for(uint i=0;i<colorTableSize;i++){
			record.colorTableRGB[i]=readRGBRecord(bytes);
		}
		
		record.colormapPixelData=new byte[imageDataSize];
		for(uint i=0;i<imageDataSize;i++){
			record.colormapPixelData[i]=bytes.readUI8();
		}
		return record;
	}
	
	private BitmapDataRecord readBitmapDataRecord(SwfByteArray bytes,byte bitmapformat,uint imageDataSize){
		var record=new BitmapDataRecord();
		record.bitmapPixelData=new IPixRecord[imageDataSize];
		if(bitmapformat==4){
			for(uint i=0;i<imageDataSize;i++){
				record.bitmapPixelData[i]=readPix15Record(bytes);
			}
		}else if(bitmapformat==5){
			for(uint i=0;i<imageDataSize;i++){
				record.bitmapPixelData[i]=readPix24Record(bytes);
			}
		}
		return record;
	}
	
	private Pix15Record readPix15Record(SwfByteArray bytes){
		var record=new Pix15Record();
		record.reserved=(byte)bytes.readUB(1);
		record.red=(byte)bytes.readUB(5);
		record.green=(byte)bytes.readUB(5);
		record.blue=(byte)bytes.readUB(5);
		return record;
	}
	
	private Pix24Record readPix24Record(SwfByteArray bytes){
		var record=new Pix24Record();
		record.reserved=bytes.readUI8();
		record.red=bytes.readUI8();
		record.green=bytes.readUI8();
		record.blue=bytes.readUI8();
		return record;
	}

	private AlphaColorMapDataRecord readAlphaColorMapDataRecord(SwfByteArray bytes,uint colorTableSize,uint imageDataSize){
		var record=new AlphaColorMapDataRecord();
		
		record.colorTableRGB=new RGBARecord[colorTableSize];
		for(uint i=0;i<colorTableSize;i++){
			record.colorTableRGB[i]=readRGBARecord(bytes);
		}

		record.colormapPixelData=new byte[imageDataSize];
		for(uint i=0;i<imageDataSize;i++){
			record.colormapPixelData[i]=bytes.readUI8();
		}
		return record;
	}

	private AlphaBitmapDataRecord readAlphaBitmapDataRecord(SwfByteArray bytes,uint imageDataSize){
		var record=new AlphaBitmapDataRecord();
		record.bitmapPixelData=new ARGBRecord[imageDataSize];
		for(uint i=0;i<imageDataSize;i++){
			record.bitmapPixelData[i]=readARGBRecord(bytes);
		}
		return record;
	}

	private MorphFillStyleArrayRecord readMorphFillStyleArrayRecord(SwfByteArray bytes){
		var record=new MorphFillStyleArrayRecord();
		record.fillStyleCount=bytes.readUI8();
		if(record.fillStyleCount==0xFF){
			record.fillStyleCountExtended=bytes.readUI16();
		}
		record.fillStyles=new MorphFillStyleRecord[record.fillStyleCount];
		for(var i=0;i<record.fillStyleCount;i++){
			record.fillStyles[i]=readMorphFillStyleRecord(bytes);
		}
		return record;
	}

	private MorphFillStyleRecord readMorphFillStyleRecord(SwfByteArray bytes){
		var record=new MorphFillStyleRecord();
		record.fillStyleType=bytes.readUI8();

		var type=record.fillStyleType;
		if(type==0x00){
			record.startColor=readRGBARecord(bytes);
			record.endColor=readRGBARecord(bytes);
		}else if(type==0x10||type==0x12){
			record.startGradientMatrix=readMatrixRecord(bytes);
			record.endGradientMatrix=readMatrixRecord(bytes);
			record.gradient=readMorphGradientRecord(bytes);
		}else if(type==0x40||type==0x41||type==0x42||type==0x43){
			record.bitmapId=bytes.readUI16();
			record.startBitmapMatrix=readMatrixRecord(bytes);
			record.endBitmapMatrix=readMatrixRecord(bytes);
		}
		return record;
	}

	private MorphGradientRecord readMorphGradientRecord(SwfByteArray bytes){
		var record=new MorphGradientRecord();
		record.numGradients=bytes.readUI8();
		record.gradientRecords=new MorphGradRecord[record.numGradients];
		for(var i=0;i<record.numGradients;i++){
			record.gradientRecords[i]=readMorphGradRecord(bytes);
		}
		return record;
	}

	private MorphGradRecord readMorphGradRecord(SwfByteArray bytes){
		var record=new MorphGradRecord();
		record.startRatio=bytes.readUI8();
		record.startColor=readRGBARecord(bytes);
		record.endRatio=bytes.readUI8();
		record.endColor=readRGBARecord(bytes);
		return record;
	}

	private MorphLineStyleArrayRecord readMorphLineStyleArrayRecord(SwfByteArray bytes,byte morphShapeType){
		var record=new MorphLineStyleArrayRecord();
		record.lineStyleCount=bytes.readUI8();
		if(record.lineStyleCount==0xFF){
			record.lineStyleCountExtended=bytes.readUI16();
		}
		if(morphShapeType==1){
			record.lineStyles=new MorphLineStyleRecord[record.lineStyleCount];
			for(var i=0;i<record.lineStyleCount;i++){
				record.lineStyles[i]=readMorphLineStyleRecord(bytes);
			}
		}else if(morphShapeType==2){
			record.lineStyles=new MorphLineStyle2Record[record.lineStyleCount];
			for(var i=0;i<record.lineStyleCount;i++){
				record.lineStyles[i]=readMorphLineStyle2Record(bytes);
			}
		}
		return record;
	}

	private MorphLineStyleRecord readMorphLineStyleRecord(SwfByteArray bytes){
		var record=new MorphLineStyleRecord();
		record.startWidth=bytes.readUI16();
		record.endWidth=bytes.readUI16();
		record.startColor=readRGBARecord(bytes);
		record.endColor=readRGBARecord(bytes);
		return record;
	}

	private MorphLineStyle2Record readMorphLineStyle2Record(SwfByteArray bytes){
		var record=new MorphLineStyle2Record();
		record.startWidth=bytes.readUI16();
		record.endWidth=bytes.readUI16();
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
		if(!record.hasFillFlag){
			record.startColor=readRGBARecord(bytes);
			record.endColor=readRGBARecord(bytes);
		}else{
			record.fillType=readMorphFillStyleRecord(bytes);
		}
		return record;
	}

	private SHAPE readSHAPE(SwfByteArray bytes,byte morphShapeType){
		var shape=new SHAPE();
		byte numFillBits=(byte)bytes.readUB(4);
		byte numLineBits=(byte)bytes.readUB(4);
		shape.numFillBits=numFillBits;
		shape.numLineBits=numLineBits;
		var list=new List<IShapeRecord>();
		//3:DefineMorphShape最小支持版本是SWF3与DefineShape3一样；
		//4:DefineMorphShape2最小支持版本是SWF8与DefineShape4一样
		var shapeType=morphShapeType==1?3:4;
		while(true){
			var shapeRecord=readShapeRecord(bytes,numFillBits,numLineBits,(byte)shapeType);
			list.Add(shapeRecord);
			if(shapeRecord is StyleChangeRecord){
				if(((StyleChangeRecord)shapeRecord).stateNewStyles){
					numFillBits=((StyleChangeRecord)shapeRecord).numFillBits;
					numLineBits=((StyleChangeRecord)shapeRecord).numLineBits;
				}
			}
			if(shapeRecord is EndShapeRecord)break;
		}
		shape.shapeRecords=list.ToArray();
		return shape;
	}

	private CXFormRecord readCXFormRecord(SwfByteArray bytes){
		var record=new CXFormRecord();
		record.hasAddTerms=bytes.readFlag();
		record.hasMultTerms=bytes.readFlag();
		record.nBits=(byte)bytes.readUB(4);
		if(record.hasMultTerms){
			record.redMultTerm=bytes.readSB(record.nBits);
			record.greenMultTerm=bytes.readSB(record.nBits);
			record.blueMultTerm=bytes.readSB(record.nBits);
		}
		if(record.hasAddTerms){
			record.redAddTerm=bytes.readSB(record.nBits);
			record.greenAddTerm=bytes.readSB(record.nBits);
			record.blueAddTerm=bytes.readSB(record.nBits);
		}
		return record;
	}

	private CXFormWithAlphaRecord readCXFormWithAlphaRecord(SwfByteArray bytes){
		bytes.alignBytes();//必须
		var record=new CXFormWithAlphaRecord();
		record.hasAddTerms=bytes.readFlag();
		record.hasMultTerms=bytes.readFlag();
		var nBits=(byte)bytes.readUB(4);
		record.nBits=nBits;
		if(record.hasMultTerms){
			record.redMultTerm=bytes.readSB(nBits);
			record.greenMultTerm=bytes.readSB(nBits);
			record.blueMultTerm=bytes.readSB(nBits);
			record.alphaMultTerm=bytes.readSB(nBits);
		}
		if(record.hasAddTerms){
			record.redAddTerm=bytes.readSB(nBits);
			record.greenAddTerm=bytes.readSB(nBits);
			record.blueAddTerm=bytes.readSB(nBits);
			record.alphaAddTerm=bytes.readSB(nBits);
		}
		return record;
	}

	private FilterListRecord readFilterListRecord(SwfByteArray bytes){
		var record=new FilterListRecord();
		record.numberOfFilters=bytes.readUI8();
		var filters=new FilterRecord[record.numberOfFilters];
		for(var i=0;i<filters.Length;i++){
			filters[i]=readFilterRecord(bytes);
		}
		record.filters=filters;
		return record;
	}

	private FilterRecord readFilterRecord(SwfByteArray bytes){
		var record=new FilterRecord();
		record.filterId=bytes.readUI8();
		switch(record.filterId){
			case 0:
				record.dropShadowFilter=readDropShadowFilterRecord(bytes);
				break;
			case 1:
				record.blurFilter=readBlurFilterRecord(bytes);
				break;
			case 2:
				record.glowFilter=readGlowFilterRecord(bytes);
				break;
			case 3:
				record.bevelFilter=readBevelFilterRecord(bytes);
				break;
			case 4:
				record.gradientGlowFilter=readGradientGlowFilterRecord(bytes);
				break;
			case 5:
				record.convolutionFilter=readConvolutionFilterRecord(bytes);
				break;
			case 6:
				record.colorMatrixFilter=readColorMatrixFilterRecord(bytes);
				break;
			case 7:
				record.gradientBevelFilter=readGradientBevelFilterRecord(bytes);
				break;
		}
		return record;
	}

	private DropShadowFilterRecord readDropShadowFilterRecord(SwfByteArray bytes){
		var record=new DropShadowFilterRecord();
		record.dropShadowColor=readRGBARecord(bytes);
		record.blurX=bytes.readFixed16_16();
		record.blurY=bytes.readFixed16_16();
		record.angle=bytes.readFixed16_16();
		record.distance=bytes.readFixed16_16();
		record.strength=bytes.readFixed8_8();
		record.innerShadow=bytes.readFlag();
		record.knockout=bytes.readFlag();
		record.compositeSource=bytes.readFlag();
		record.passes=(byte)bytes.readUB(5);
		return record;
	}

	private BlurFilterRecord readBlurFilterRecord(SwfByteArray bytes){
		var record=new BlurFilterRecord();
		record.blurX=bytes.readFixed16_16();
		record.blurY=bytes.readFixed16_16();
		record.passes=(byte)bytes.readUB(5);
		record.reserved=(byte)bytes.readUB(3);
		return record;
	}

	private GlowFilterRecord readGlowFilterRecord(SwfByteArray bytes){
		var record=new GlowFilterRecord();
		record.glowColor=readRGBARecord(bytes);
		record.blurX=bytes.readFixed16_16();
		record.blurY=bytes.readFixed16_16();
		record.strength=bytes.readFixed8_8();
		record.innerGlow=bytes.readFlag();
		record.knockout=bytes.readFlag();
		record.compositeSource=bytes.readFlag();
		record.passes=(byte)bytes.readUB(5);
		return record;
	}

	private BevelFilterRecord readBevelFilterRecord(SwfByteArray bytes){
		var record=new BevelFilterRecord();
		record.shadowColor=readRGBARecord(bytes);
		record.highlightColor=readRGBARecord(bytes);
		record.blurX=bytes.readFixed16_16();
		record.blurY=bytes.readFixed16_16();
		record.angle=bytes.readFixed16_16();
		record.distance=bytes.readFixed16_16();
		record.strength=bytes.readFixed8_8();
		record.innerShadow=bytes.readFlag();
		record.knockout=bytes.readFlag();
		record.compositeSource=bytes.readFlag();
		record.onTop=bytes.readFlag();
		record.passes=(byte)bytes.readUB(4);
		return record;
	}

	private GradientGlowFilterRecord readGradientGlowFilterRecord(SwfByteArray bytes){
		var record=new GradientGlowFilterRecord();
		var numColors=bytes.readUI8();
		record.numColors=numColors;
		record.gradientColors=new RGBARecord[numColors];
		record.gradientRatio=new byte[numColors];
		for(var i=0;i<numColors;i++){
			record.gradientColors[i]=readRGBARecord(bytes);
			record.gradientRatio[i]=bytes.readUI8();
		}
		record.blurX=bytes.readFixed16_16();
		record.blurY=bytes.readFixed16_16();
		record.angle=bytes.readFixed16_16();
		record.distance=bytes.readFixed16_16();
		record.strength=bytes.readFixed8_8();
		record.innerShadow=bytes.readFlag();
		record.knockout=bytes.readFlag();
		record.compositeSource=bytes.readFlag();
		record.onTop=bytes.readFlag();
		record.passes=(byte)bytes.readUB(4);
		return record;
	}

	private GradientBevelFilterRecord readGradientBevelFilterRecord(SwfByteArray bytes){
		var record=new GradientBevelFilterRecord();
		var numColors=bytes.readUI8();
		record.numColors=numColors;
		record.gradientColors=new RGBARecord[numColors];
		record.gradientRatio=new byte[numColors];
		for(var i=0;i<numColors;i++){
			record.gradientColors[i]=readRGBARecord(bytes);
			record.gradientRatio[i]=bytes.readUI8();
		}
		record.blurX=bytes.readFixed16_16();
		record.blurY=bytes.readFixed16_16();
		record.angle=bytes.readFixed16_16();
		record.distance=bytes.readFixed16_16();
		record.strength=bytes.readFixed8_8();
		record.innerShadow=bytes.readFlag();
		record.knockout=bytes.readFlag();
		record.compositeSource=bytes.readFlag();
		record.onTop=bytes.readFlag();
		record.passes=(byte)bytes.readUB(4);
		return record;
	}

	private ConvolutionFilterRecord readConvolutionFilterRecord(SwfByteArray bytes){
		var record=new ConvolutionFilterRecord();
		record.matrixX=bytes.readUI8();
		record.matrixY=bytes.readUI8();
		record.divisor=bytes.readFloat();
		record.bias=bytes.readFloat();
		int len=record.matrixX*record.matrixY;
		record.matrix=new float[len];
		for(var i=0;i<len;i++){
			record.matrix[i]=bytes.readFloat();
		}
		record.defaultColor=readRGBARecord(bytes);
		record.reserved=(byte)bytes.readUB(6);
		record.clamp=bytes.readFlag();
		record.preserveAlpha=bytes.readFlag();
		return record;
	}
		
	private ColorMatrixFilterRecord readColorMatrixFilterRecord(SwfByteArray bytes){
		var record=new ColorMatrixFilterRecord();
		record.matrix=new float[20];
		for(byte i=0;i<20;i++){
			float value=bytes.readFloat();
			record.matrix[i]=value;
		}
		return record;
	}

	private ButtonRecord readButtonRecord(SwfByteArray bytes,byte reserved,bool hasBlendMode,bool hasFilterList,bool stateHitTest,bool stateDown,bool stateOver,bool stateUp,byte buttonType){
		var record=new ButtonRecord();
		record.buttonReserved=reserved;
		record.buttonHasBlendMode=hasBlendMode;
		record.buttonHasFilterList=hasFilterList;
		record.buttonStateHitTest=stateHitTest;
		record.buttonStateDown=stateDown;
		record.buttonStateOver=stateOver;
		record.buttonStateUp=stateUp;
		record.characterID=bytes.readUI16();
		record.placeDepth=bytes.readUI16();
		record.placeMatrix=readMatrixRecord(bytes);
		if(buttonType==2){
			record.colorTransform=readCXFormWithAlphaRecord(bytes);
			if(hasFilterList){
				record.filterList=readFilterListRecord(bytes);
			}
			if(hasBlendMode){
				record.blendMode=bytes.readUI8();
			}
		}
		record.buttonType=buttonType;
		return record;
	}

	private TextRecord readTextRecord(SwfByteArray bytes,byte recordType,byte defineTextType,byte glyphBits,byte advanceBits){
		var record=new TextRecord();
		record.textRecordType=recordType;
		record.styleFlagsReserved=(byte)bytes.readUB(3);
		record.styleFlagsHasFont=bytes.readFlag();
		record.styleFlagsHasColor=bytes.readFlag();
		record.styleFlagsHasYOffset=bytes.readFlag();
		record.styleFlagsHasXOffset=bytes.readFlag();
		if(record.styleFlagsHasFont)record.fontID=bytes.readUI16();
		if(record.styleFlagsHasColor){
			if(defineTextType==2)record.textColor=readRGBARecord(bytes);
			else record.textColor=readRGBRecord(bytes);
		}
		if(record.styleFlagsHasXOffset)record.xOffset=bytes.readSI16();
		if(record.styleFlagsHasYOffset)record.yOffset=bytes.readSI16();
		if(record.styleFlagsHasFont)record.textHeight=bytes.readUI16();
		record.glyphCount=bytes.readUI8();
		record.glyphEntries=new GlyphEntryRecord[record.glyphCount];
		for(var i=0;i<record.glyphCount;i++){
			record.glyphEntries[i]=readGlyphEntryRecord(bytes,glyphBits,advanceBits);
		}
		return record;
	}

	private GlyphEntryRecord readGlyphEntryRecord(SwfByteArray bytes,byte glyphBits,byte advanceBits){
		var record=new GlyphEntryRecord();
		record.glyphIndex=bytes.readUB(glyphBits);
		record.glyphAdvance=bytes.readSB(advanceBits);
		return record;
	}
}
