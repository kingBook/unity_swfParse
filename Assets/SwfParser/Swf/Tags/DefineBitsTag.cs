using System.Collections.Generic;
using System.Text;
using System.Xml;

public class DefineBitsTag : SwfTag, ICharacterIdTag {

    public ushort characterID;
    public byte[] jpegData;

    public override XmlElement ToXml(XmlDocument doc) {
        var ele = CreateXmlElement(doc, "DefineBits");
        ele.SetAttribute("characterID", characterID.ToString());

        var jpegDataStrBuilder = new StringBuilder("");
        if (jpegData != null) {
            for (int i = 0; i < jpegData.Length; i++) {
                jpegDataStrBuilder.Append(jpegData[i]);
                if (i < jpegData.Length - 1) {
                    jpegDataStrBuilder.Append(',');
                }
            }
        }
        ele.SetAttribute("jpegData", jpegDataStrBuilder.ToString());
        return ele;
    }

    public void GetNeededCharacterIds(List<ushort> characterIds, Swf swf) {
        if (characterIds.IndexOf(characterID) < 0) {
            characterIds.Add(characterID);
        }
    }

    public ushort GetCharacterId() {
        return characterID;
    }
}