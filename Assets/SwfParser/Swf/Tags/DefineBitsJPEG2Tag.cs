using System.Collections.Generic;
using System.Text;
using System.Xml;

public class DefineBitsJPEG2Tag : SwfTag, ICharacterIdTag {

    public ushort characterID;
    public byte[] imageData;

    public override XmlElement ToXml(XmlDocument doc) {
        var ele = CreateXmlElement(doc, "DefineBitsJPEG2");
        ele.SetAttribute("characterID", characterID.ToString());
        var imageDataStrBuilder = new StringBuilder("");
        for (int i = 0; i < imageData.Length; i++) {
            imageDataStrBuilder.Append(imageData[i]);
            if (i < imageData.Length - 1) {
                imageDataStrBuilder.Append(',');
            }
        }
        ele.SetAttribute("imageData", imageDataStrBuilder.ToString());
        return ele;
    }

    public void GetNeededCharacterIds(List<ushort> characterIds, Swf swf) {
        if (characterIds.IndexOf(characterID) < 0) {
            characterIds.Add(characterID);
        }
    }
}