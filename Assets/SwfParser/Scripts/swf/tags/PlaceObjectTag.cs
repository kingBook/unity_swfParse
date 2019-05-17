
using System.Xml;

public class PlaceObjectTag:SwfTag{
	public ushort characterId;
	public ushort depth;
	public MatrixRecord matrix;
	public CXFormRecord colorTransform;

	public override XmlElement toXml(XmlDocument doc) {
		var ele=createXmlElement(doc,"PlaceObject");
		ele.SetAttribute("characterId",characterId.ToString());
		ele.SetAttribute("depth",depth.ToString());
		ele.SetAttribute("matrix",matrix.ToString());
		ele.AppendChild(colorTransform.toXml(doc));
		return ele;
	}

}
