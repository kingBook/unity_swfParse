using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class SwfByteArray{
	private static readonly int filter5 =	Convert.ToInt32("11111",2);
	private static readonly int filter7 =	Convert.ToInt32("1111111",2);
	private static readonly int filter8 =	Convert.ToInt32("11111111",2);
	private static readonly int filter10 =	Convert.ToInt32("1111111111",2);
	private static readonly int filter13 =	Convert.ToInt32("1111111111111",2);
	private static readonly int filter16 =	Convert.ToInt32("1111111111111111",2);
	private static readonly int filter23 =	Convert.ToInt32("11111111111111111111111",2);

	private FileStream _fileStream;
	private BinaryReader _binaryReader;
	private long bitPosition=0;

	/**
	* Returns the number of bits required to hold <code>number</code> in a UB
	*/
	public static uint calculateUBBits(uint number){
		if(number == 0) return 0;
		uint bits = 0;
        uint b=number >>= 1;
		while(b>0) bits++;
		return bits + 1;
	}

	/**
	* Returns the number of bits required to hold <code>number</code> in an SB
	*/
	public static uint calculateSBBits(int number){
		return number == 0 ? 1 : calculateUBBits((uint)(number < 0 ? ~number : number)) + 1;
	}

	/**
	* Returns the number of bits required to hold <code>number</code> in an FB
	*/
	public static uint calculateFBBits(float number){
		int integer = Mathf.FloorToInt(number);
		int decimalNum = Mathf.RoundToInt(Mathf.Abs(number - integer) * 0xFFFF) & filter16;

		int sbVersion = ((integer & filter16) << 16) | (decimalNum);
		
		return number == 0 ? 1 : calculateSBBits(sbVersion);
	}

	/*private static uint float32AsUnsignedInt(float value){
		tempByteArray.position = 0;
		tempByteArray.writeFloat(value);
		tempByteArray.position = 0;
		return tempByteArray.readUnsignedInt();
	}*/
		
	/*private static function unsignedIntAsFloat32(value:uint):Number{
		tempByteArray.position = 0;
		tempByteArray.writeUnsignedInt(value);
		tempByteArray.position = 0;
		return tempByteArray.readFloat();
	}*/
		
	/*public function SWFByteArray(bytes:ByteArray)
	{
		this.bytes = bytes;
		bytes.endian = Endian.LITTLE_ENDIAN;
		bytes.position = 0;
	}*/

	public SwfByteArray(string swfPath){
		var fs=File.OpenRead(swfPath);
		init(fs);
	}

	public void init(FileStream fileStream){
		dispose();

		_fileStream=fileStream;
		_binaryReader=new BinaryReader(fileStream,Encoding.Unicode);
	}

	public void alignBytes(){
		if(bitPosition!=0){
			_fileStream.Position++;
			bitPosition=0;
		}
	}

	public string readStringWithLength(uint length){
		alignBytes();
		byte[] bytes=_binaryReader.ReadBytes((int)length);
		string str=Encoding.UTF8.GetString(bytes);
		return str;
	}

	public byte readUI8(){
		alignBytes();
		return _binaryReader.ReadByte();
	}

	public uint readUI32(){
		alignBytes();
		return _binaryReader.ReadUInt32();
	}

	public void readBytes(FileStream outFileStream,int offset=0,int length=0){
		alignBytes();
		length=length<=0?(int)_fileStream.Length:length;
		Debug.Log("_fileStream.Position:"+_fileStream.Position);
		Debug.Log("_fileStream.Length:"+_fileStream.Length);
		Debug.Log("outFileStream.Length:"+outFileStream.Length);
		byte[] bytes=_binaryReader.ReadBytes(length);
		outFileStream.Write(bytes,offset,length);
	}

	public long getBytePosition(){
		return _fileStream.Position;
	}

	public void dispose(){
		if(_fileStream!=null){
			_fileStream.Dispose();
			_fileStream=null;
		}
		if(_fileStream!=null){
			_binaryReader.Dispose();
			_binaryReader=null;
		}
	}

	public string getFileStreamName(){
		return _fileStream.Name;
	}

	
}
