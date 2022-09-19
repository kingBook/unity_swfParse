using UnityEngine;
using System.Collections;
using System.Xml;

public class SetBackgroundColorTag : SwfTag {

    public RGBRecord backgroundColor;

    public override XmlElement ToXml(XmlDocument doc) {
        var ele = CreateXmlElement(doc, "SetBackgroundColor");
        ele.SetAttribute("backgroundColor", backgroundColor.ToString());
        return ele;
    }
}