using System.Xml;

public struct FilterListRecord {

    public byte numberOfFilters;
    public FilterRecord[] filters;

    public FilterListRecord(SwfByteArray bytes) {
        numberOfFilters = bytes.ReadUI8();
        var filters = new FilterRecord[numberOfFilters];
        for (var i = 0; i < filters.Length; i++) {
            filters[i] = new FilterRecord(bytes);
        }
        this.filters = filters;
    }

    public XmlElement ToXml(XmlDocument doc) {
        var ele = doc.CreateElement("FilterList");
        for (var i = 0; i < numberOfFilters; i++) {
            ele.AppendChild(filters[i].ToXml(doc));
        }
        return ele;
    }
}