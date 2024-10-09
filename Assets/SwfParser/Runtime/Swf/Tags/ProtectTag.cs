﻿using System.Xml;

public class ProtectTag : SwfTag {

    public ProtectTag(SwfByteArray bytes, TagHeaderRecord header) : base(header) {
    }

    public override XmlElement ToXml(XmlDocument doc) {
        var ele = CreateXmlElement(doc, "Protect");
        return ele;
    }
}