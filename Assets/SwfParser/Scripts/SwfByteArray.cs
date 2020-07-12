using System;
using System.IO;
using System.Text;
using UnityEngine;
#pragma warning disable

public class SwfByteArray{
	private static readonly int filter5 =	(1<<5)-1;
	private static readonly int filter7 =	(1<<7)-1;
	private static readonly int filter8 =	(1<<8)-1;
	private static readonly int filter10 =	(1<<10)-1;
	private static readonly int filter13 =	(1<<13)-1;
	private static readonly int filter16 =	(1<<16)-1;
	private static readonly int filter23 =	(1<<23)-1;

	private MemoryStream _ms;
	private BinaryReader _br;
	private long bitPosition=0;

	/// <summary>
	/// 返回在UB中保存 number 所需的位数
	/// </summary>
	public static uint calculateUBBits(uint number){
		if(number == 0) return 0;
		uint bits = 0;
        uint b=number >>= 1;
		while(b>0) bits++;
		return bits + 1;
	}

	/// <summary>
	/// 返回在SB中保存 number 所需的位数
	/// </summary>
	public static uint calculateSBBits(int number){
		return number == 0 ? 1 : calculateUBBits((uint)(number < 0 ? ~number : number)) + 1;
	}
	
	/// <summary>
	/// 返回FB中保存 number 所需的位数
	/// </summary>
	public static uint calculateFBBits(float number){
		int integer = Mathf.FloorToInt(number);
		int decimalNum = Mathf.RoundToInt(Mathf.Abs(number - integer) * 0xFFFF) & filter16;

		int sbVersion = ((integer & filter16) << 16) | (decimalNum);
		
		return number == 0 ? 1 : calculateSBBits(sbVersion);
	}

	private static uint float32AsUnsignedInt(float value){
		byte[] bytes=BitConverter.GetBytes(value);
		return BitConverter.ToUInt32(bytes,0);
	}
		
	private static float unsignedIntAsFloat32(uint value){
		byte[] bytes=BitConverter.GetBytes(value);
		return BitConverter.ToSingle(bytes,0);
	}

	public SwfByteArray(string swfPath){
		var fs=File.OpenRead(swfPath);
		_ms=new MemoryStream();
		copyStream(fs,_ms);
		_ms.Position=0;
		
		_br=new BinaryReader(_ms);

		fs.Close();
	}

	public SwfByteArray(byte[] bytes){
		_ms=new MemoryStream();
		_ms.Write(bytes,0,bytes.Length);
		_ms.Position=0;

		_br=new BinaryReader(_ms);
	}

	public void alignBytes(){
		if(bitPosition!=0){
			_ms.Position++;
			bitPosition=0;
		}
	}

	public long getBytePosition(){
		return _ms.Position;
	}

	public void setBytePosition(long value){
		bitPosition=0;
		_ms.Position=value;
	}

	public long getBitPosition(){
		return bitPosition;
	}

	public void setBitPosition(long value){
		bitPosition=value;
	}

	public long getBytesAvailable(){
		return _ms.Length-_ms.Position;
	}

	public long getLength(){
		return _ms.Length;
	}

	public void compress(){ }

	public void decompress(){
		long msPos=_ms.Position;
		MemoryStream outMS=new MemoryStream();
		zlib.ZOutputStream outZStream=new zlib.ZOutputStream(outMS);
		
		copyStream(_ms,outZStream);
		outZStream.finish();

		outMS.Position=0;
		_ms.Position=msPos;
		copyStream(outMS,_ms);
		_ms.Position=msPos;
		
		outMS.Close();
		outZStream.Close();
	}
	private void copyStream(Stream input,Stream output){
		byte[] buffer=new byte[2000];
		int len;
		while((len=input.Read(buffer,0,2000))>0){
			output.Write(buffer,0,len);
		}
		output.Flush();
	}

	public void clear(){
		bitPosition=0;
		_ms.Dispose();
	}

	public void dump(){ }

	public byte[] readBytes(int count){
		return _br.ReadBytes(count);
	}

	public void writeBytes(){ }

	public bool readFlag(){
		return readUB(1)==1;
	}

	public void writeFlag(){ }

	public sbyte readSI8(){
		alignBytes();
		return _br.ReadSByte();
	}

	public short readSI16(){
		alignBytes();
		return _br.ReadInt16();
	}

	public void writeSI16(){ }

	public int readSI32(){
		alignBytes();
		return _br.ReadInt32();
	}

	public void writeSI32(){ }

	public sbyte[] readSI8Array(uint length){
		sbyte[] sbytes=new sbyte[length];
		for(uint i=0;i<length;i++){
			sbytes[i]=readSI8();
		}
		return sbytes;
	}

	public short[] readSI16Array(uint length){
		short[] shorts=new short[length];
		for(uint i=0;i<length;i++){
			shorts[i]=readSI16();
		}
		return shorts;
	}

	public byte readUI8(){
		alignBytes();
		return _br.ReadByte();
	}

	public void writeUI8(){ }

	public ushort readUI16(){
		alignBytes();
		return _br.ReadUInt16();
	}

	public void writeUI16(){ }

	public uint readUI32(){
		alignBytes();
		return _br.ReadUInt32();
	}

	public void writeUI32(){ }

	public byte[] readUI8Array(uint length){
		byte[] bytes =new byte[length];
		for(uint i=0;i<length;i++){
			bytes[i]=readUI8();
		}
		return bytes;
	}

	public ushort[] readUI16Array(uint length){
		ushort[] ushorts =new ushort[length];
		for(uint i=0;i<length;i++){
			ushorts[i]=readUI16();
		}
		return ushorts;
	}

	public uint[] readUI24Array(uint length){
		alignBytes();
		uint[] list=new uint[length];
		for(uint i=0;i<length;i++){
			list[i]=(uint)_br.ReadUInt16() << 8 | _br.ReadByte();
		}
		return list;
	}

	public uint[] readUI32Array(uint length){
		uint[] list=new uint[length];
		for(uint i=0;i<length;i++){
			list[i]=readUI32();
		}
		return list;
	}

	public float readFixed8_8(){
		alignBytes();
		uint decimalNum = _br.ReadByte();
		float result = _br.ReadSByte();
		
		result += decimalNum / 0xFF;
			
		return result;
	}

	public void writeFixed8_8(){ }

	public float readFixed16_16(){
		alignBytes();
		ushort decimalNum = _br.ReadUInt16();
		float result = _br.ReadInt16();
			
		result += (float)decimalNum / 0xFFFF;
			
		return result;
	}

	public float readFloat16(){
		uint raw = readUI16();
			
		uint sign = raw >> 15;
		uint exp = (raw >> 10) & (uint)filter5;
		uint sig = raw & (uint)filter10;
			
		if(exp == 31){//Handle infinity/NaN
			exp = 255;
		}else if(exp == 0){//Handle normalized values
			exp = 0;
			sig = 0;
		}else{
			exp += 111;
		}
			
		uint temp = sign << 31 | exp << 23 | sig << 13;
			
		return unsignedIntAsFloat32(temp);
	}

	public void writeFloat16(){ }

	public float readFloat(){
		return _br.ReadSingle();
	}
	public void writeFloat(){ }

	public double readDouble(){
		return _br.ReadDouble();
	}
	public void writeDouble(){ }

	public uint readEncodedUI32(){
		alignBytes();
		uint result=0;
		uint bytesRead=0;
		uint currentByte;
		bool shouldContinue = true;
		while(shouldContinue && bytesRead < 5)
		{
			currentByte = _br.ReadByte();
			result = (uint)( ((currentByte & filter7) << (int)((7 * bytesRead)) | result) );
			shouldContinue = ((currentByte >> 7) == 1);
			bytesRead++;
		}
		return result;
	}

	public void writeEncodedUI32(){ }

	public uint readUB(uint length){
		if(length<=0) return 0;
		
		int totalBytes = Mathf.CeilToInt((bitPosition + length) / 8.0f);
		uint iter = 0;
		uint currentByte = 0;
		uint result = 0;
		
		while(iter < totalBytes){
			currentByte = _br.ReadByte();
			result = (result << 8) | currentByte;
			iter++;
		}
			
		int newBitPosition = (int)((bitPosition + length) % 8);
			
		int excessBits = (totalBytes * 8 - ((int)bitPosition + (int)length));
		result = result >> excessBits;
		int filterBit=(1<<(int)length)-1;
		result = (uint)(result & filterBit);
			
		bitPosition = newBitPosition;
		if(bitPosition > 0){
			_ms.Position--;
		}
		return result;
	}

	public void writeUB(){ }

	public int readSB(uint length){
		if(length<=0) return 0;
			
		uint result = readUB(length);
		uint leadingDigit = result >> ((int)length - 1);
		if(leadingDigit == 1)
		{
			return -( (~(int)result & ((1<<(int)length)-1) ) + 1);
		}
		return (int)result;
	}

	public void writeSB(){ }

	public float readFB(uint length){
		if(length<=0) return 0;
			
		int raw = readSB(length);
			
		int integer = raw >> 16;
		float decimalNum = (raw & filter16)/0xFFFF; 
			
		return integer + decimalNum;
	}

	public void writeFB(){ }

	public string readString(){
		alignBytes();
		int byteCount = 1;
		while(_br.ReadByte()>0){
			byteCount++;
		}
		_ms.Position -= byteCount;
		byte[] bytes = _br.ReadBytes(byteCount);
		string result = Encoding.UTF8.GetString(bytes,0,byteCount-1);
		return result;
	}
	public void writeString(){
		
	}

	public string readStringWithLength(uint length){
		alignBytes();
		byte[] bytes=_br.ReadBytes((int)length);
		string str=Encoding.UTF8.GetString(bytes);
		return str;
	}

	public void writeStringWithLength(){
		
	}

	public void close(){
		if(_ms!=null){
			_ms.Close();
			_ms=null;
		}
		if(_ms!=null){
			_br.Close();
			_br=null;
		}
	}
	
}
