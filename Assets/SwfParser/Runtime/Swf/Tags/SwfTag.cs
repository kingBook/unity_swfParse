using System.Xml;

public class SwfTag {

    public TagHeaderRecord header;

    public SwfTag(TagHeaderRecord header) {
        this.header = header;
    }

    protected string GetClassName() {
        var className = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;
        return className;
    }

    protected XmlElement CreateXmlElement(XmlDocument doc, string elementName = null) {
        if (elementName == null) elementName = GetClassName();
        var ele = doc.CreateElement(elementName);
        ele.SetAttribute("type", header.type.ToString());
        ele.SetAttribute("length", header.length.ToString());
        return ele;
    }

    public virtual XmlElement ToXml(XmlDocument doc) {
        return CreateXmlElement(doc);
    }


}