using System.Xml;

public struct CXFormRecord {
    
    public bool hasAddTerms;
    public bool hasMultTerms;
    public byte nBits;
    public int redMultTerm;
    public int greenMultTerm;
    public int blueMultTerm;
    public int redAddTerm;
    public int greenAddTerm;
    public int blueAddTerm;

    public XmlElement ToXml(XmlDocument doc) {
        var ele = doc.CreateElement("CXForm");
        ele.SetAttribute("hasAddTerms", hasAddTerms.ToString());
        ele.SetAttribute("hasMultTerms", hasMultTerms.ToString());
        ele.SetAttribute("nBits", hasMultTerms.ToString());
        if (hasMultTerms) {
            ele.SetAttribute("redMultTerm", redMultTerm.ToString());
            ele.SetAttribute("greenMultTerm", greenMultTerm.ToString());
            ele.SetAttribute("blueMultTerm", blueMultTerm.ToString());
        }
        if (hasAddTerms) {
            ele.SetAttribute("redAddTerm", redAddTerm.ToString());
            ele.SetAttribute("greenAddTerm", greenAddTerm.ToString());
            ele.SetAttribute("blueAddTerm", blueAddTerm.ToString());
        }
        return ele;
    }
}