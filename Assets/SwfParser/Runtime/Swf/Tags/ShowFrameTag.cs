﻿using System.Xml;

public class ShowFrameTag : SwfTag {

    public ShowFrameTag(SwfByteArray bytes, TagHeaderRecord header) : base(header) {

    }

    public override XmlElement ToXml(XmlDocument doc) {
        return CreateXmlElement(doc, "ShowFrame");
    }

    public ShowFrameTagData ToData() {
        return new ShowFrameTagData();
    }
}