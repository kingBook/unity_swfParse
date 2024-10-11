using System.Xml;

[System.Serializable]
public class ScriptLimitsTag : SwfTag {

    public ushort maxRecursionDepth;
    public ushort scriptTimeoutSeconds;

    public ScriptLimitsTag(SwfByteArray bytes, TagHeaderRecord header) : base(header) {
        maxRecursionDepth = bytes.ReadUI16();
        scriptTimeoutSeconds = bytes.ReadUI16();
    }

    public override XmlElement ToXml(XmlDocument doc) {
        var ele = CreateXmlElement(doc, "ScriptLimits");
        ele.SetAttribute("maxRecursionDepth", maxRecursionDepth.ToString());
        ele.SetAttribute("scriptTimeoutSeconds", scriptTimeoutSeconds.ToString());
        return ele;
    }

}