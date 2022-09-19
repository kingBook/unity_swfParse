using System.Xml;

public struct CurvedEdgeRecord : IEdgeRecord { //:EdgeRecord:ShapeRecord

    public bool typeFlag;
    public bool straightFlag;
    public byte numBits;
    public int controlDeltaX;
    public int controlDeltaY;
    public int anchorDeltaX;
    public int anchorDeltaY;

    public XmlElement ToXml(XmlDocument doc) {
        var ele = doc.CreateElement("CurvedEdgeRecord");
        ele.SetAttribute("typeFlag", typeFlag.ToString());
        ele.SetAttribute("straightFlag", straightFlag.ToString());
        ele.SetAttribute("numBits", numBits.ToString());
        ele.SetAttribute("controlDeltaX", controlDeltaX.ToString());
        ele.SetAttribute("controlDeltaY", controlDeltaY.ToString());
        ele.SetAttribute("anchorDeltaX", anchorDeltaX.ToString());
        ele.SetAttribute("anchorDeltaY", anchorDeltaY.ToString());
        return ele;
    }
}