using UnityEngine;
using System.Collections;
using System.Xml;

public class RemoveObject2Tag : SwfTag {

    public ushort depth;

    public RemoveObject2Tag(SwfByteArray bytes, TagHeaderRecord header) : base(header) {
        depth = bytes.ReadUI16();
    }

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