using UnityEngine;
using System.Collections;

public class SwfHeader {
    public static readonly string UNCOMPRESSED_SIGNATURE = "FWS";
    public static readonly string COMPRESSED_SIGNATURE = "CWS";

    public string signature;
    public byte fileVersion;
    public uint uncompressedSize;

    public RectangleRecord frameSize;
    public float frameRate;
    public uint frameCount;

    public SwfHeader(SwfByteArray bytes) {
        signature = bytes.ReadStringWithLength(3);
        fileVersion = bytes.ReadUI8();
        uncompressedSize = bytes.ReadUI32();
        if (signature == COMPRESSED_SIGNATURE) {
            bytes.Decompress();
        }
        frameSize = new RectangleRecord(bytes);
        frameRate = bytes.ReadFixed8_8();
        frameCount = bytes.ReadUI16();
    }

    public override string ToString() {
        string str = "";
        str += "{\n";
        str += "  signature:" + signature + ",\n";
        str += "  fileVersion:" + fileVersion + ",\n";
        str += "  uncompressedSize:" + uncompressedSize + ",\n";
        str += "  frameSize:" + frameSize.ToString() + ",\n";
        str += "  frameRate:" + frameRate + ",\n";
        str += "  frameCount:" + frameCount + "\n";
        str += "}";
        return str;
    }
}