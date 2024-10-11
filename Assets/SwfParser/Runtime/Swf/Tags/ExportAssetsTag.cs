using System.Xml;

[System.Serializable]
public class ExportAssetsTag : SwfTag {

    public ExportAssetRecord[] list;

    public ExportAssetsTag(SwfByteArray bytes, TagHeaderRecord header) : base(header) {
        ushort count = bytes.ReadUI16();
        list = new ExportAssetRecord[count];
        for (ushort i = 0; i < count; i++) {
            var record = new ExportAssetRecord();
            record.tag = bytes.ReadUI16();
            record.name = bytes.ReadString();
            list[i] = record;
        }
    }

    public override XmlElement ToXml(XmlDocument doc) {
        var ele = CreateXmlElement(doc, "ExportAssets");
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