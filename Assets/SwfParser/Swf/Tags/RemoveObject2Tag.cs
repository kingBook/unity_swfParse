﻿using UnityEngine;
using System.Collections;
using System.Xml;

public class RemoveObject2Tag : SwfTag {

    public ushort depth;

    public override XmlElement ToXml(XmlDocument doc) {
        var ele = CreateXmlElement(doc, "RemoveObject2");
        ele.SetAttribute("depth", depth.ToString());
        return ele;
    }
    
    public RemoveObject2TagData ToData() {
        var data = new RemoveObject2TagData();
        data.depth = depth;
        return data;
    }

}