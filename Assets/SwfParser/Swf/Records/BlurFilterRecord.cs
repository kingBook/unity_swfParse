using UnityEngine;
using System.Collections;
using System.Xml;

public struct BlurFilterRecord {

    public float blurX;
    public float blurY;
    public byte passes;
    public byte reserved;

    public XmlElement ToXml(XmlDocument doc) {
        var ele = doc.CreateElement("BlurFilter");
        ele.SetAttribute("blurX", blurX.ToString());
        ele.SetAttribute("blurY", blurY.ToString());
        ele.SetAttribute("passes", passes.ToString());
        ele.SetAttribute("reserved", reserved.ToString());
        return ele;
    }
}