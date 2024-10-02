using System.Xml;

public struct BlurFilterRecord {

    public float blurX;
    public float blurY;
    public byte passes;
    public byte reserved;

    public BlurFilterRecord(SwfByteArray bytes) {
        blurX = bytes.ReadFixed16_16();
        blurY = bytes.ReadFixed16_16();
        passes = (byte)bytes.ReadUB(5);
        reserved = (byte)bytes.ReadUB(3);
    }

    public XmlElement ToXml(XmlDocument doc) {
        var ele = doc.CreateElement("BlurFilter");
        ele.SetAttribute("blurX", blurX.ToString());
        ele.SetAttribute("blurY", blurY.ToString());
        ele.SetAttribute("passes", passes.ToString());
        ele.SetAttribute("reserved", reserved.ToString());
        return ele;
    }
}