https://github.com/icsharpcode/SharpZipLib

http://www.zlib.net/




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