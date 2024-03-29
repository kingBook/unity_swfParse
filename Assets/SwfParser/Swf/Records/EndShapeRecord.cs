﻿using System.Xml;

public struct EndShapeRecord : IShapeRecord { //Shape Record

    public bool typeFlag;
    public uint endOfShape;

    public XmlElement ToXml(XmlDocument doc) {
        var ele = doc.CreateElement("EndShapeRecord");
        ele.SetAttribute("typeFlag", typeFlag.ToString());
        ele.SetAttribute("endOfShape", endOfShape.ToString());
        return ele;
    }
}