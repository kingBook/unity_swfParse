using System;

public struct ARGBRecord {

    public byte alpha;
    public byte red;
    public byte green;
    public byte blue;

    public ARGBRecord(SwfByteArray bytes) {
        alpha = bytes.ReadUI8();
        red = bytes.ReadUI8();
        green = bytes.ReadUI8();
        blue = bytes.ReadUI8();
    }

    public override string ToString() {
        uint color = alpha;
        color = (color << 8) | red;
        color = (color << 8) | green;
        color = (color << 8) | blue;
        string str = Convert.ToString(color, 16);
        byte headZeroCount = (byte)(8 - str.Length);
        for (byte i = 0; i < headZeroCount; i++) str = '0' + str;
        return str;
    }
}