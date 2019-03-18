using System.Xml;

public interface IShapeRecord {
	XmlElement toXml(XmlDocument doc);
}

