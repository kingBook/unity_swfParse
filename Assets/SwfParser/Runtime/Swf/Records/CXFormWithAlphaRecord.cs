using System.Xml;

[System.Serializable]
public struct CXFormWithAlphaRecord {

    public bool hasAddTerms;
    public bool hasMultTerms;
    public byte nBits;
    public int redMultTerm;
    public int greenMultTerm;
    public int blueMultTerm;
    public int alphaMultTerm;
    public int redAddTerm;
    public int greenAddTerm;
    public int blueAddTerm;
    public int alphaAddTerm;

    public CXFormWithAlphaRecord(SwfByteArray bytes) {
        bytes.AlignBytes(); //必须

        hasAddTerms = bytes.ReadFlag();
        hasMultTerms = bytes.ReadFlag();
        nBits = (byte)bytes.ReadUB(4);
        if (hasMultTerms) {
            redMultTerm = bytes.ReadSB(nBits);
            greenMultTerm = bytes.ReadSB(nBits);
            blueMultTerm = bytes.ReadSB(nBits);
            alphaMultTerm = bytes.ReadSB(nBits);
        } else {
            redMultTerm = 0;
            greenMultTerm = 0;
            blueMultTerm = 0;
            alphaMultTerm = 0;
        }
        if (hasAddTerms) {
            redAddTerm = bytes.ReadSB(nBits);
            greenAddTerm = bytes.ReadSB(nBits);
            blueAddTerm = bytes.ReadSB(nBits);
            alphaAddTerm = bytes.ReadSB(nBits);
        } else {
            redAddTerm = 0;
            greenAddTerm = 0;
            blueAddTerm = 0;
            alphaAddTerm = 0;
        }
    }

    public XmlElement ToXml(XmlDocument doc) {
        var ele = doc.CreateElement("CXFormWithAlpha");
        ele.SetAttribute("hasAddTerms", hasAddTerms.ToString());
        ele.SetAttribute("hasMultTerms", hasMultTerms.ToString());
        ele.SetAttribute("nBits", nBits.ToString());
        if (hasMultTerms) {
            ele.SetAttribute("redMultTerm", redMultTerm.ToString());
            ele.SetAttribute("greenMultTerm", greenMultTerm.ToString());
            ele.SetAttribute("blueMultTerm", blueMultTerm.ToString());
            ele.SetAttribute("alphaMultTerm", alphaMultTerm.ToString());
        }
        if (hasAddTerms) {
            ele.SetAttribute("redAddTerm", redAddTerm.ToString());
            ele.SetAttribute("greenAddTerm", greenAddTerm.ToString());
            ele.SetAttribute("blueAddTerm", blueAddTerm.ToString());
            ele.SetAttribute("alphaAddTerm", alphaAddTerm.ToString());
        }
        return ele;
    }
}