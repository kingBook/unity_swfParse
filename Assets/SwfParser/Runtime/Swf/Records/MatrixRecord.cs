[System.Serializable]
public struct MatrixRecord {

    public bool hasScale;
    public byte nScaleBits;
    public float scaleX;
    public float scaleY;

    public bool hasRotate;
    public byte nRotateBits;
    public float rotateSkew0;
    public float rotateSkew1;

    public byte nTranslateBits;
    public int translateX;
    public int translateY;

    public MatrixRecord(SwfByteArray bytes) {
        hasScale = bytes.ReadFlag();
        if (hasScale) {
            nScaleBits = (byte)bytes.ReadUB(5);
            scaleX = bytes.ReadFB(nScaleBits);
            scaleY = bytes.ReadFB(nScaleBits);
        } else {
            nScaleBits = 0;
            scaleX = 1.0f;
            scaleY = 1.0f;
        }
        hasRotate = bytes.ReadFlag();
        if (hasRotate) {
            nRotateBits = (byte)bytes.ReadUB(5);
            rotateSkew0 = bytes.ReadFB(nRotateBits);
            rotateSkew1 = bytes.ReadFB(nRotateBits);
        } else {
            nRotateBits = 0;
            rotateSkew0 = 0;
            rotateSkew1 = 0;
        }
        nTranslateBits = (byte)bytes.ReadUB(5);
        translateX = bytes.ReadSB(nTranslateBits);
        translateY = bytes.ReadSB(nTranslateBits);
    }

    public override string ToString() {
        //[scaleX, skewX, tx,
        //  skewY, scaleY,ty]
        float scaleX = hasScale ? this.scaleX : 1;
        float scaleY = hasScale ? this.scaleY : 1;
        float skewX = hasRotate ? rotateSkew0 : 0;
        float skewY = hasRotate ? rotateSkew1 : 0;
        float tx = translateX;
        float ty = translateY;
        return scaleX + "," + skewX + "," + tx + "," + skewY + "," + scaleY + "," + ty;
    }

    public Matrix ToMatrix() {
        return new Matrix(scaleX, rotateSkew0, rotateSkew1, scaleY, translateX, translateY);
    }

}