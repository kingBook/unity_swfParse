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

}