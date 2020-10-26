
using System.IO;
using UnityEngine;
using zlib;

public class ZlibUtil{
	/// <summary>
	/// 复制流
	/// </summary>
	/// <param name="input">原始流</param>
	/// <param name="output">目标流</param>
	private static void CopyStream(System.IO.Stream input, System.IO.Stream output){
		byte[] buffer = new byte[2000];
		int len;
		while ((len = input.Read(buffer, 0, 2000)) > 0){
			output.Write(buffer, 0, len);
		}
		output.Flush();
	}
	/// <summary>
	/// 压缩字节数组
	/// </summary>
	/// <param name="sourceByte">需要被压缩的字节数组</param>
	/// <returns>压缩后的字节数组</returns>
	public static byte[] CompressBytes(byte[] sourceByte){
		MemoryStream inputStream = new MemoryStream(sourceByte);
		Stream outStream = CompressStream(inputStream);
		byte[] outPutByteArray = new byte[outStream.Length];
		outStream.Position = 0;
		outStream.Read(outPutByteArray, 0, outPutByteArray.Length);
		outStream.Close();
		inputStream.Close();
		return outPutByteArray;
	}
	/// <summary>
	/// 解压缩字节数组
	/// </summary>
	/// <param name="sourceByte">需要被解压缩的字节数组</param>
	/// <returns>解压后的字节数组</returns>
	public static byte[] DeCompressBytes(byte[] sourceByte){
		MemoryStream inputStream = new MemoryStream(sourceByte);
		Stream outputStream = DeCompressStream(inputStream);
		byte[] outputBytes = new byte[outputStream.Length];
		outputStream.Position = 0;
		outputStream.Read(outputBytes, 0, outputBytes.Length);
		outputStream.Close();
		inputStream.Close();
		return outputBytes;
	}
	/// <summary>
	/// 压缩流
	/// </summary>
	/// <param name="sourceStream">需要被压缩的流</param>
	/// <returns>压缩后的流</returns>
	public static Stream CompressStream(Stream sourceStream){
		MemoryStream streamOut = new MemoryStream();
		ZOutputStream streamZOut = new ZOutputStream(streamOut, zlibConst.Z_DEFAULT_COMPRESSION);
		CopyStream(sourceStream, streamZOut);
		streamZOut.finish();
		return streamOut;
	}
	/// <summary>
	/// 解压缩流
	/// </summary>
	/// <param name="sourceStream">需要被解压缩的流</param>
	/// <returns>解压后的流</returns>
	public static Stream DeCompressStream(Stream sourceStream){
		MemoryStream outStream = new MemoryStream();
		ZOutputStream outZStream = new ZOutputStream(outStream);
		CopyStream(sourceStream, outZStream);
		outZStream.finish();
		return outStream;
	}
}
