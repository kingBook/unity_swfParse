using UnityEngine;
using System.Collections;
using System.Xml;

public class DefineButtonTag : SwfTag {

    public ushort buttonId;
    public ButtonRecord[] characters;

    public byte characterEndFlag;
    //public actions;
    //public actionEndFlag;

    public override XmlElement ToXml(XmlDocument doc) {
        return base.ToXml(doc);
    }

}