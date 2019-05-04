using System.Xml;

public struct StraightEdgeRecord:IEdgeRecord{//:EdgeRecord:ShapeRecord
	public bool typeFlag;
	public bool straightFlag;
	public byte numBits;
	public bool generalLineFlag;
	public bool vertLineFlag;
	public int deltaX;
	public int deltaY;

	public XmlElement toXml(XmlDocument doc) {
		var ele=doc.CreateElement("StraightEdgeRecord");
		ele.SetAttribute("typeFlag",typeFlag.ToString());
		ele.SetAttribute("straightFlag",straightFlag.ToString());
		ele.SetAttribute("numBits",numBits.ToString());
		ele.SetAttribute("generalLineFlag",generalLineFlag.ToString());
		ele.SetAttribute("vertLineFlag",vertLineFlag.ToString());
		ele.SetAttribute("deltaX",deltaX.ToString());
		ele.SetAttribute("deltaY",deltaY.ToString());
		return ele;
	}
}
