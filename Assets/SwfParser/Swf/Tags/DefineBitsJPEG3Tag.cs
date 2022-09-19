using System.Text;
using System.Xml;

public class DefineBitsJPEG3Tag : SwfTag {

    public ushort characterID;
    public uint alphaDataOffset;
    public byte[] imageData;
    public byte[] bitmapAlphaData;

    public override XmlElement ToXml(XmlDocument doc) {
        var ele = CreateXmlElement(doc, "DefineBitsJPEG3");
        ele.SetAttribute("characterID", characterID.ToString());
        ele.SetAttribute("alphaDataOffset", alphaDataOffset.ToString());

        var imageDataStrBuilder = new StringBuilder("");
        for (int i = 0; i < imageData.Length; i++) {
            imageDataStrBuilder.Append(imageData[i]);
            if (i < imageData.Length - 1) {
                imageDataStrBuilder.Append(',');
            }
        }
        ele.SetAttribute("imageData", imageDataStrBuilder.ToString());

        var alphaDataStrBuilder = new StringBuilder("");
        for (int i = 0; i < bitmapAlphaData.Length; i++) {
            alphaDataStrBuilder.Append(bitmapAlphaData[i]);
            if (i < bitmapAlphaData.Length - 1) {
                alphaDataStrBuilder.Append(',');
            }
        }
        ele.SetAttribute("bitmapAlphaData", alphaDataStrBuilder.ToString());
        return ele;
    }

}