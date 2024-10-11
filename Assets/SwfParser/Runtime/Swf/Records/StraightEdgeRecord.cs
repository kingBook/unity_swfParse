using System.Xml;

[System.Serializable]
public struct StraightEdgeRecord : IEdgeRecord { //:EdgeRecord:ShapeRecord

    public bool typeFlag;
    public bool straightFlag;
    public byte numBits;
    public bool generalLineFlag;
    public bool vertLineFlag;
    public int deltaX;
    public int deltaY;

    public StraightEdgeRecord(SwfByteArray bytes, bool typeFlag, bool straightFlag) {
        // default value
        vertLineFlag = false;
        deltaX = 0;
        deltaY = 0;
        //

        this.typeFlag = typeFlag;
        this.straightFlag = straightFlag;
        numBits = (byte)bytes.ReadUB(4);
        generalLineFlag = bytes.ReadFlag();

        // if(!generalLineFlag){
        //     vertLineFlag=bytes.ReadFlag();//(sbyte)bytes.readSB(1);
        // }
        // if(generalLineFlag||!vertLineFlag){
        //     deltaX=bytes.ReadSB((uint)numBits+2);
        // }
        // if(generalLineFlag||vertLineFlag){
        //     deltaY=bytes.ReadSB((uint)numBits+2);
        // }

        if (generalLineFlag) {
            deltaX = bytes.ReadSB((uint)numBits + 2);
            deltaY = bytes.ReadSB((uint)numBits + 2);
        } else {
            vertLineFlag = bytes.ReadFlag(); //(sbyte)bytes.readSB(1);
            if (!vertLineFlag) {
                deltaX = bytes.ReadSB((uint)numBits + 2);
            } else {
                deltaY = bytes.ReadSB((uint)numBits + 2);
            }
        }
    }

    public XmlElement ToXml(XmlDocument doc) {
        var ele = doc.CreateElement("StraightEdgeRecord");
        ele.SetAttribute("typeFlag", typeFlag.ToString());
        ele.SetAttribute("straightFlag", straightFlag.ToString());
        ele.SetAttribute("numBits", numBits.ToString());
        ele.SetAttribute("generalLineFlag", generalLineFlag.ToString());
        ele.SetAttribute("vertLineFlag", vertLineFlag.ToString());
        ele.SetAttribute("deltaX", deltaX.ToString());
        ele.SetAttribute("deltaY", deltaY.ToString());
        return ele;
    }
}