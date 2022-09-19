using System.Xml;

public class PlaceObjectTag : SwfTag {

    public ushort characterId;
    public ushort depth;
    public MatrixRecord matrix;
    public CXFormRecord colorTransform;

    public override XmlElement ToXml(XmlDocument doc) {
        var ele = CreateXmlElement(doc, "PlaceObject");
        ele.SetAttribute("characterId", characterId.ToString());
        ele.SetAttribute("depth", depth.ToString());
        ele.SetAttribute("matrix", matrix.ToString());
        ele.AppendChild(colorTransform.ToXml(doc));
        return ele;
    }

}