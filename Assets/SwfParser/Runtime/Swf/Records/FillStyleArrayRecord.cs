using System.Xml;

[System.Serializable]
public struct FillStyleArrayRecord {

    public byte fillStyleCount;
    public ushort fillStyleCountExtended;
    public FillStyleRecord[] fillStyles;

    public FillStyleArrayRecord(SwfByteArray bytes, byte shapeType) {
        // default value
        fillStyleCountExtended = 0;
        //

        // fillStyleCount=bytes.ReadUI8();
        // var list=new FillStyleRecord[fillStyleCount];
        // for(uint i=0;i<fillStyleCount;i++){
        //     list[i]=new FillStyleRecord(bytes,shapeType);
        // }
        // fillStyles=list;


        fillStyleCount = bytes.ReadUI8();
        //if(shapeType==2||shapeType==3){
        if (fillStyleCount == 0xFF) {
            fillStyleCountExtended = bytes.ReadUI16();
        }
        //}
        var list = new FillStyleRecord[fillStyleCount];
        for (uint i = 0; i < fillStyleCount; i++) {
            list[i] = new FillStyleRecord(bytes, shapeType);
        }
        fillStyles = list;
    }

    public XmlElement ToXml(XmlDocument doc) {
        var ele = doc.CreateElement("FillStyleArray");
        ele.SetAttribute("fillStyleCount", fillStyleCount.ToString());
        ele.SetAttribute("fillStyleCountExtended", fillStyleCountExtended.ToString());
        for (int i = 0; i < fillStyles.Length; i++) {
            ele.AppendChild(fillStyles[i].ToXml(doc));
        }
        return ele;
    }
}