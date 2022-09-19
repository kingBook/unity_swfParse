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