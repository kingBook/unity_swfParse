﻿using System.Xml;

[System.Serializable]
public class SetBackgroundColorTag : Tag {

    public RGBRecord backgroundColor;

    public SetBackgroundColorTag(SwfByteArray bytes, TagHeaderRecord header) : base(header) {
        backgroundColor = new RGBRecord(bytes);
    }

    public override XmlElement ToXml(XmlDocument doc) {
        var ele = CreateXmlElement(doc, "SetBackgroundColor");
        ele.SetAttribute("backgroundColor", backgroundColor.ToString());
        return ele;
    }
}