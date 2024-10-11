using System;

[System.Serializable]
public struct Pix15Record : IPixRecord {

    public byte reserved;
    public byte red;
    public byte green;
    public byte blue;

    public Pix15Record(SwfByteArray bytes) {
        reserved = (byte)bytes.ReadUB(1);
        red = (byte)bytes.ReadUB(5);
        green = (byte)bytes.ReadUB(5);
        blue = (byte)bytes.ReadUB(5);
    }

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