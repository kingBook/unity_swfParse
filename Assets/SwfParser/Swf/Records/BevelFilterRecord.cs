using UnityEngine;
using System.Collections;
using System.Xml;

public struct BevelFilterRecord {
    
    public RGBARecord shadowColor;
    public RGBARecord highlightColor;
    public float blurX;
    public float blurY;
    public float angle;
    public float distance;
    public float strength;
    public bool innerShadow;
    public bool knockout;
    public bool compositeSource;
    public bool onTop;
    public byte passes;

    public XmlElement ToXml(XmlDocument doc) {
        var ele = doc.CreateElement("BevelFilter");
        ele.SetAttribute("shadowColor", shadowColor.ToString());
        ele.SetAttribute("highlightColor", highlightColor.ToString());
        ele.SetAttribute("blurX", blurX.ToString());
        ele.SetAttribute("blurY", blurY.ToString());
        ele.SetAttribute("angle", angle.ToString());
        ele.SetAttribute("distance", distance.ToString());
        ele.SetAttribute("strength", strength.ToString());
        ele.SetAttribute("innerShadow", innerShadow.ToString());
        ele.SetAttribute("knockout", knockout.ToString());
        ele.SetAttribute("compositeSource", compositeSource.ToString());
        ele.SetAttribute("onTop", onTop.ToString());
        ele.SetAttribute("passes", passes.ToString());
        return ele;
    }

}