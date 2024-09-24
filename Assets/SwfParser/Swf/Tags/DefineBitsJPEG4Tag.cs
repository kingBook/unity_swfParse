using System.Text;
using System.Xml;

public class DefineBitsJPEG4Tag : DefineBitsJPEG3Tag {

    //public ushort characterID;
    //public uint alphaDataOffset;
    public float deblockParam;
    //public byte[] imageData;
    //public byte[] bitmapAlphaData;

    public DefineBitsJPEG4Tag(SwfByteArray bytes, TagHeaderRecord header) : base(header) {
        long startPosition = bytes.GetBytePosition();
        characterID = bytes.ReadUI16();
        alphaDataOffset = bytes.ReadUI32();
        deblockParam = bytes.ReadFixed8_8();
        if (alphaDataOffset > 0) {
            imageData = bytes.ReadBytes((int)alphaDataOffset);
        }
        int bytesRemaining = (int)(header.length - (bytes.GetBytePosition() - startPosition));
        if (bytesRemaining > 0) {
            bitmapAlphaData = bytes.ReadBytes(bytesRemaining);
        }
    }

    public override XmlElement ToXml(XmlDocument doc) {
        var ele = CreateXmlElement(doc, "DefineBitsJPEG4");
        ele.SetAttribute("characterID", characterID.ToString());
        ele.SetAttribute("alphaDataOffset", alphaDataOffset.ToString());
        ele.SetAttribute("deblockParam", deblockParam.ToString());

        var imageDataBuilder = new StringBuilder();
        int len = imageData.Length;
        int maxId = len - 1;
        for (var i = 0; i < len; i++) {
            imageDataBuilder.Append(imageData[i]);
            if (i < maxId) imageDataBuilder.Append(',');
        }
        ele.SetAttribute("imageData", imageDataBuilder.ToString());

        var bitmapAlphaDataBuilder = new StringBuilder();
        len = bitmapAlphaData.Length;
        maxId = len - 1;
        for (var i = 0; i < len; i++) {
            bitmapAlphaDataBuilder.Append(bitmapAlphaData[i]);
            if (i < maxId) bitmapAlphaDataBuilder.Append(',');
        }
        ele.SetAttribute("bitmapAlphaData", bitmapAlphaDataBuilder.ToString());
        return ele;
    }

}