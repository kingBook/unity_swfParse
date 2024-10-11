public static class ShapeRecordReader {

    public static IShapeRecord ReadShapeRecord(SwfByteArray bytes, byte numFillBits, byte numLineBits, byte shapeType) {
        IShapeRecord record;
        bool typeFlag = bytes.ReadFlag();
        long start = bytes.GetBytePosition();
        if (!typeFlag) {
            bool stateNewStyles = bytes.ReadFlag();
            bool stateLineStyle = bytes.ReadFlag();
            bool stateFillStyle1 = bytes.ReadFlag();
            bool stateFillStyle0 = bytes.ReadFlag();
            bool stateMoveTo = bytes.ReadFlag();

            bool isEndShapeRecord = !stateNewStyles && !stateLineStyle && !stateFillStyle1 && !stateFillStyle0 && !stateMoveTo;
            if (isEndShapeRecord) {
                record = new EndShapeRecord(bytes, typeFlag);
            } else {
                record = new StyleChangeRecord(bytes, typeFlag, stateNewStyles, stateLineStyle,
                    stateFillStyle1, stateFillStyle0, stateMoveTo, numFillBits, numLineBits, shapeType);
            }
        } else {
            bool straightFlag = bytes.ReadFlag();
            if (straightFlag) {
                record = new StraightEdgeRecord(bytes, typeFlag, straightFlag);
            } else {
                record = new CurvedEdgeRecord(bytes, typeFlag, straightFlag);
            }
        }
        return record;
    }

}