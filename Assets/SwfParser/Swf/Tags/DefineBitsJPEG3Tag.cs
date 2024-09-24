using System.Text;
using System.Xml;

public class DefineBitsJPEG3Tag : DefineBitsJPEG2Tag {

    //public ushort characterID;
    public uint alphaDataOffset;
    //public byte[] imageData;
    public byte[] bitmapAlphaData;

    public DefineBitsJPEG3Tag(TagHeaderRecord header) : base(header) {
        // empty constructor
    }

    public DefineBitsJPEG3Tag(SwfByteArray bytes, TagHeaderRecord header) : base(header) {
        long startPosition = bytes.GetBytePosition();
        characterID = bytes.ReadUI16();
        alphaDataOffset = bytes.ReadUI32();
        if (alphaDataOffset > 0) {
            imageData = bytes.ReadBytes((int)alphaDataOffset);
        }
        int bytesRemaining = (int)(header.length - (bytes.GetBytePosition() - startPosition));
        if (bytesRemaining > 0) {
            bitmapAlphaData = bytes.ReadBytes(bytesRemaining);
        }
    }

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