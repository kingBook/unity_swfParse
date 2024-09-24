using UnityEngine;
using System.Collections;

public struct RectangleRecord {

    public int xMin;
    public int xMax;
    public int yMin;
    public int yMax;
    
    public RectangleRecord (SwfByteArray bytes) {
        bytes.AlignBytes();
        uint nBits = bytes.ReadUB(5);
        xMin = bytes.ReadSB(nBits);
        xMax = bytes.ReadSB(nBits);
        yMin = bytes.ReadSB(nBits);
        yMax = bytes.ReadSB(nBits);
    }

    public override string ToString() {
        return $"{xMin},{yMin},{xMax},{yMax}";
    }
}