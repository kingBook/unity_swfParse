using UnityEngine;
using System.Collections;
using System.Xml;

public class ShowFrameTag : SwfTag {
    
    public override XmlElement ToXml(XmlDocument doc) {
        return CreateXmlElement(doc, "ShowFrame");
    }

    public ShowFrameTagData ToData() {
        return new ShowFrameTagData();
    }
}