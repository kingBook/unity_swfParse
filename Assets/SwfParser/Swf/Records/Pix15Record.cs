using System;

public struct Pix15Record : IPixRecord {

    public byte reserved;
    public byte red;
    public byte green;
    public byte blue;

    public override string ToString() {
        uint color = red;
        color = (color << 8) | green;
        color = (color << 8) | blue;
        string str = Convert.ToString(color, 16);
        byte headZeroCount = (byte)(6 - str.Length);
        for (byte i = 0; i < headZeroCount; i++) str = '0' + str;
        return str;
    }
}