using System;
using System.IO;
using System.Text;
using UnityEngine;

#pragma warning disable

public class SwfByteArray {

    private static readonly int s_filter5 = (1 << 5) - 1;
    private static readonly int s_filter7 = (1 << 7) - 1;
    private static readonly int s_filter8 = (1 << 8) - 1;
    private static readonly int s_filter10 = (1 << 10) - 1;
    private static readonly int s_filter13 = (1 << 13) - 1;
    private static readonly int s_filter16 = (1 << 16) - 1;
    private static readonly int s_filter23 = (1 << 23) - 1;

    private MemoryStream m_memoryStream;
    private BinaryReader m_binaryReader;
    private long m_bitPosition = 0;

    /// <summary>
    /// 返回在UB中保存 number 所需的位数
    /// </summary>
    public static uint CalculateUBBits(uint number) {
        if (number == 0) return 0;
        uint bits = 0;
        uint b = number >>= 1;
        while (b > 0) bits++;
        return bits + 1;
    }

    /// <summary>
    /// 返回在SB中保存 number 所需的位数
    /// </summary>
    public static uint CalculateSBBits(int number) {
        return number == 0 ? 1 : CalculateUBBits((uint)(number < 0 ? ~number : number)) + 1;
    }

    /// <summary>
    /// 返回FB中保存 number 所需的位数
    /// </summary>
    public static uint CalculateFBBits(float number) {
        int integer = Mathf.FloorToInt(number);
        int decimalNum = Mathf.RoundToInt(Mathf.Abs(number - integer) * 0xFFFF) & s_filter16;

        int sbVersion = ((integer & s_filter16) << 16) | (decimalNum);

        return number == 0 ? 1 : CalculateSBBits(sbVersion);
    }

    private static uint Float32AsUnsignedInt(float value) {
        byte[] bytes = BitConverter.GetBytes(value);
        return BitConverter.ToUInt32(bytes, 0);
    }

    private static float UnsignedIntAsFloat32(uint value) {
        byte[] bytes = BitConverter.GetBytes(value);
        return BitConverter.ToSingle(bytes, 0);
    }

    public SwfByteArray(string swfPath) {
        var fs = File.OpenRead(swfPath);
        m_memoryStream = new MemoryStream();
        CopyStream(fs, m_memoryStream);
        m_memoryStream.Position = 0;

        m_binaryReader = new BinaryReader(m_memoryStream);

        fs.Close();
    }

    public SwfByteArray(byte[] bytes) {
        m_memoryStream = new MemoryStream();
        m_memoryStream.Write(bytes, 0, bytes.Length);
        m_memoryStream.Position = 0;

        m_binaryReader = new BinaryReader(m_memoryStream);
    }

    public void AlignBytes() {
        if (m_bitPosition != 0) {
            m_memoryStream.Position++;
            m_bitPosition = 0;
        }
    }

    public long GetBytePosition() {
        return m_memoryStream.Position;
    }

    public void SetBytePosition(long value) {
        m_bitPosition = 0;
        m_memoryStream.Position = value;
    }

    public long GetBitPosition() {
        return m_bitPosition;
    }

    public void SetBitPosition(long value) {
        m_bitPosition = value;
    }

    public long GetBytesAvailable() {
        return m_memoryStream.Length - m_memoryStream.Position;
    }

    public long GetLength() {
        return m_memoryStream.Length;
    }

    public void Compress() {
    }

    public void Decompress() {
        long msPos = m_memoryStream.Position;
        MemoryStream outMS = new MemoryStream();
        zlib.ZOutputStream outZStream = new zlib.ZOutputStream(outMS);

        CopyStream(m_memoryStream, outZStream);
        outZStream.finish();

        outMS.Position = 0;
        m_memoryStream.Position = msPos;
        CopyStream(outMS, m_memoryStream);
        m_memoryStream.Position = msPos;

        outMS.Close();
        outZStream.Close();
    }

    private void CopyStream(Stream input, Stream output) {
        byte[] buffer = new byte[2000];
        int len;
        while ((len = input.Read(buffer, 0, 2000)) > 0) {
            output.Write(buffer, 0, len);
        }
        output.Flush();
    }

    public void Clear() {
        m_bitPosition = 0;
        m_memoryStream.Dispose();
    }

    public void Dump() {
    }

    public byte[] ReadBytes(int count) {
        return m_binaryReader.ReadBytes(count);
    }

    public void WriteBytes() {
    }

    public bool ReadFlag() {
        return ReadUB(1) == 1;
    }

    public void WriteFlag() {
    }

    public sbyte ReadSI8() {
        AlignBytes();
        return m_binaryReader.ReadSByte();
    }

    public short ReadSI16() {
        AlignBytes();
        return m_binaryReader.ReadInt16();
    }

    public void WriteSI16() {
    }

    public int ReadSI32() {
        AlignBytes();
        return m_binaryReader.ReadInt32();
    }

    public void WriteSI32() {
    }

    public sbyte[] ReadSI8Array(uint length) {
        sbyte[] sbytes = new sbyte[length];
        for (uint i = 0; i < length; i++) {
            sbytes[i] = ReadSI8();
        }
        return sbytes;
    }

    public short[] ReadSI16Array(uint length) {
        short[] shorts = new short[length];
        for (uint i = 0; i < length; i++) {
            shorts[i] = ReadSI16();
        }
        return shorts;
    }

    public byte ReadUI8() {
        AlignBytes();
        return m_binaryReader.ReadByte();
    }

    public void WriteUI8() {
    }

    public ushort ReadUI16() {
        AlignBytes();
        return m_binaryReader.ReadUInt16();
    }

    public void WriteUI16() {
    }

    public uint ReadUI32() {
        AlignBytes();
        return m_binaryReader.ReadUInt32();
    }

    public void WriteUI32() {
    }

    public byte[] ReadUI8Array(uint length) {
        byte[] bytes = new byte[length];
        for (uint i = 0; i < length; i++) {
            bytes[i] = ReadUI8();
        }
        return bytes;
    }

    public ushort[] ReadUI16Array(uint length) {
        ushort[] ushorts = new ushort[length];
        for (uint i = 0; i < length; i++) {
            ushorts[i] = ReadUI16();
        }
        return ushorts;
    }

    public uint[] ReadUI24Array(uint length) {
        AlignBytes();
        uint[] list = new uint[length];
        for (uint i = 0; i < length; i++) {
            list[i] = (uint)m_binaryReader.ReadUInt16() << 8 | m_binaryReader.ReadByte();
        }
        return list;
    }

    public uint[] ReadUI32Array(uint length) {
        uint[] list = new uint[length];
        for (uint i = 0; i < length; i++) {
            list[i] = ReadUI32();
        }
        return list;
    }

    public float ReadFixed8_8() {
        AlignBytes();
        uint decimalNum = m_binaryReader.ReadByte();
        float result = m_binaryReader.ReadSByte();

        result += decimalNum / 0xFF;

        return result;
    }

    public void WriteFixed8_8() {
    }

    public float ReadFixed16_16() {
        AlignBytes();
        ushort decimalNum = m_binaryReader.ReadUInt16();
        float result = m_binaryReader.ReadInt16();

        result += (float)decimalNum / 0xFFFF;

        return result;
    }

    public float ReadFloat16() {
        uint raw = ReadUI16();

        uint sign = raw >> 15;
        uint exp = (raw >> 10) & (uint)s_filter5;
        uint sig = raw & (uint)s_filter10;

        if (exp == 31) { //Handle infinity/NaN
            exp = 255;
        } else if (exp == 0) { //Handle normalized values
            exp = 0;
            sig = 0;
        } else {
            exp += 111;
        }

        uint temp = sign << 31 | exp << 23 | sig << 13;

        return UnsignedIntAsFloat32(temp);
    }

    public void WriteFloat16() {
    }

    public float ReadFloat() {
        return m_binaryReader.ReadSingle();
    }

    public void WriteFloat() {
    }

    public double ReadDouble() {
        return m_binaryReader.ReadDouble();
    }

    public void WriteDouble() {
    }

    public uint ReadEncodedUI32() {
        AlignBytes();
        uint result = 0;
        uint bytesRead = 0;
        uint currentByte;
        bool shouldContinue = true;
        while (shouldContinue && bytesRead < 5) {
            currentByte = m_binaryReader.ReadByte();
            result = (uint)(((currentByte & s_filter7) << (int)((7 * bytesRead)) | result));
            shouldContinue = ((currentByte >> 7) == 1);
            bytesRead++;
        }
        return result;
    }

    public void WriteEncodedUI32() {
    }

    public uint ReadUB(uint length) {
        if (length <= 0) return 0;

        int totalBytes = Mathf.CeilToInt((m_bitPosition + length) / 8.0f);
        uint iter = 0;
        uint currentByte = 0;
        uint result = 0;

        while (iter < totalBytes) {
            currentByte = m_binaryReader.ReadByte();
            result = (result << 8) | currentByte;
            iter++;
        }

        int newBitPosition = (int)((m_bitPosition + length) % 8);

        int excessBits = (totalBytes * 8 - ((int)m_bitPosition + (int)length));
        result = result >> excessBits;
        int filterBit = (1 << (int)length) - 1;
        result = (uint)(result & filterBit);

        m_bitPosition = newBitPosition;
        if (m_bitPosition > 0) {
            m_memoryStream.Position--;
        }
        return result;
    }

    public void WriteUB() {
    }

    public int ReadSB(uint length) {
        if (length <= 0) return 0;

        uint result = ReadUB(length);
        uint leadingDigit = result >> ((int)length - 1);
        if (leadingDigit == 1) {
            return -((~(int)result & ((1 << (int)length) - 1)) + 1);
        }
        return (int)result;
    }

    public void WriteSB() {
    }

    public float ReadFB(uint length) {
        if (length <= 0) return 0;

        int raw = ReadSB(length);

        int integer = raw >> 16;
        float decimalNum = (raw & s_filter16) / 0xFFFF;

        return integer + decimalNum;
    }

    public void WriteFB() {
    }

    public string ReadString() {
        AlignBytes();
        int byteCount = 1;
        while (m_binaryReader.ReadByte() > 0) {
            byteCount++;
        }
        m_memoryStream.Position -= byteCount;
        byte[] bytes = m_binaryReader.ReadBytes(byteCount);
        string result = Encoding.UTF8.GetString(bytes, 0, byteCount - 1);
        return result;
    }

    public void WriteString() {

    }

    public string ReadStringWithLength(uint length) {
        AlignBytes();
        byte[] bytes = m_binaryReader.ReadBytes((int)length);
        string str = Encoding.UTF8.GetString(bytes);
        return str;
    }

    public void WriteStringWithLength() {

    }

    public void Close() {
        if (m_memoryStream != null) {
            m_memoryStream.Close();
            m_memoryStream = null;
        }
        if (m_memoryStream != null) {
            m_binaryReader.Close();
            m_binaryReader = null;
        }
    }

}