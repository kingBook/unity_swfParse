using System.Xml;

public interface ILineStyleRecord {
    
    XmlElement ToXml(XmlDocument doc);
}