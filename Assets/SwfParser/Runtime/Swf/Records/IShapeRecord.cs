using System.Xml;

public interface IShapeRecord {
    
    XmlElement ToXml(XmlDocument doc);
}