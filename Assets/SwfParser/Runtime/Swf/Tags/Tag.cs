using System.Xml;

[System.Serializable]
public abstract class Tag {

    public TagHeaderRecord header;

    public Tag(TagHeaderRecord header) {
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

    public virtual void Load(Swf swf, MeshHelperBase meshHelper, DisplayObjectContainer parent) {
        // 子类重写
        Debug2.Log((TagType)header.type+" Load() 方法未实现");
    }

    public virtual XmlElement ToXml(XmlDocument doc) {
        return CreateXmlElement(doc);
    }


}