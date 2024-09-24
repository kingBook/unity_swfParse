using UnityEngine;
using System.Collections;
using System.Xml;

public class FileAttributesTag : SwfTag {

    public bool useDirectBlit;
    public bool useGPU;
    public bool hasMetadata;
    public bool actionScript3;
    public bool useNetwork;

    public FileAttributesTag(SwfByteArray bytes, TagHeaderRecord header) : base(header) {
        bytes.ReadUB(1);
        useDirectBlit = bytes.ReadFlag();
        useGPU = bytes.ReadFlag();
        hasMetadata = bytes.ReadFlag();
        actionScript3 = bytes.ReadFlag();
        bytes.ReadUB(2);
        useNetwork = bytes.ReadFlag();
        bytes.ReadUB(24);
    }

    public override XmlElement ToXml(XmlDocument doc) {
        var ele = CreateXmlElement(doc, "FileAttributes");
        ele.SetAttribute("useDirectBlit", useDirectBlit.ToString());
        ele.SetAttribute("useGPU", useGPU.ToString());
        ele.SetAttribute("hasMetadata", hasMetadata.ToString());
        ele.SetAttribute("actionScript3", actionScript3.ToString());
        ele.SetAttribute("useNetwork", useNetwork.ToString());
        return ele;
    }
}