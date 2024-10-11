using System.Xml;

[System.Serializable]
public class RemoveObject2Tag : Tag {

    public ushort depth;

    public RemoveObject2Tag(SwfByteArray bytes, TagHeaderRecord header) : base(header) {
        depth = bytes.ReadUI16();
    }

    public override XmlElement ToXml(XmlDocument doc) {
        var ele = CreateXmlElement(doc, "RemoveObject2");
        ele.SetAttribute("depth", depth.ToString());
        return ele;
    }

}