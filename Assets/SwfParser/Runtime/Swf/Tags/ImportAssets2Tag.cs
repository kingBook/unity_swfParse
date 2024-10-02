using System.Xml;

public class ImportAssets2Tag : SwfTag {

    public string url;
    public ImportAssets2Record[] list;

    public ImportAssets2Tag(SwfByteArray bytes, TagHeaderRecord header) : base(header) {
        bytes.ReadUI8();
        bytes.ReadUI8();
        ushort count = bytes.ReadUI16();
        var list = new ImportAssets2Record[count];
        for (ushort i = 0; i < count; i++) {
            var record = new ImportAssets2Record();
            record.tag = bytes.ReadUI16();
            record.name = bytes.ReadString();
            list[i] = record;
        }
    }

    public override XmlElement ToXml(XmlDocument doc) {
        var ele = CreateXmlElement(doc, "ImportAssets2");
        for (int i = 0, len = list.Length; i < len; i++) {
            var record = list[i];
            var recordEle = CreateXmlElement(doc, "Record");
            recordEle.SetAttribute("tag", record.tag.ToString());
            recordEle.SetAttribute("name", record.name);
            ele.AppendChild(recordEle);
        }
        return ele;
    }

}