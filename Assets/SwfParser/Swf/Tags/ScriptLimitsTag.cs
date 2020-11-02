using System.Xml;

public class ScriptLimitsTag:SwfTag {
	public ushort maxRecursionDepth;
	public ushort scriptTimeoutSeconds;

	public override XmlElement ToXml(XmlDocument doc) {
		var ele=CreateXmlElement(doc,"ScriptLimits");
		ele.SetAttribute("maxRecursionDepth",maxRecursionDepth.ToString());
		ele.SetAttribute("scriptTimeoutSeconds",scriptTimeoutSeconds.ToString());
		return ele;
	}

}
