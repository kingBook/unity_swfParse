﻿[System.Serializable]
public struct RGBARecord {

    public byte red;
    public byte green;
    public byte blue;
    public byte alpha;

    public RGBARecord(SwfByteArray bytes) {
        red = bytes.ReadUI8();
        green = bytes.ReadUI8();
        blue = bytes.ReadUI8();
        alpha = bytes.ReadUI8();
    }

    public override string ToString() {
        uint color = red;
        color = (color << 8) | green;
        color = (color << 8) | blue;
        color = (color << 8) | alpha;
        string str = System.Convert.ToString(color, 16);
        byte headZeroCount = (byte)(8 - str.Length);
        for (byte i = 0; i < headZeroCount; i++) str = '0' + str;
        return str;
    }
}