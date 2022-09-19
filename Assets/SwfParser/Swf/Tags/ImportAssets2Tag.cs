using System.Xml;

public class ImportAssets2Tag : SwfTag {

    public string url;
    public ImportAssets2Record[] list;

    public override XmlElement ToXml(XmlDocument doc) {
        var ele = CreateXmlElement(doc, "ImportAssets2");
        for (int i = 0; i < list.Length; i++) {
            var record = list[i];
            var recordEle = CreateXmlElement(doc, "Record");
            recordEle.SetAttribute("tag", record.tag.ToString());
            recordEle.SetAttribute("name", record.name);
            ele.AppendChild(recordEle);
        }
        return ele;
    }

}