
using System.Xml;

public struct CXFormWithAlphaRecord{
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

	public XmlElement toXml(XmlDocument doc){
		var ele=doc.CreateElement("CXFormWithAlpha");
		ele.SetAttribute("hasAddTerms",hasAddTerms.ToString());
		ele.SetAttribute("hasMultTerms",hasMultTerms.ToString());
		ele.SetAttribute("nBits",nBits.ToString());
		if(hasMultTerms){
			ele.SetAttribute("redMultTerm",redMultTerm.ToString());
			ele.SetAttribute("greenMultTerm",greenMultTerm.ToString());
			ele.SetAttribute("blueMultTerm",blueMultTerm.ToString());
			ele.SetAttribute("alphaMultTerm",alphaMultTerm.ToString());
		}
		if(hasAddTerms){
			ele.SetAttribute("redAddTerm",redAddTerm.ToString());
			ele.SetAttribute("greenAddTerm",greenAddTerm.ToString());
			ele.SetAttribute("blueAddTerm",blueAddTerm.ToString());
			ele.SetAttribute("alphaAddTerm",alphaAddTerm.ToString());
		}
		return ele;
	}
}
