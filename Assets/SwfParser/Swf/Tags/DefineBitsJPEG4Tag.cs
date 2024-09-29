using System;
using System.Text;
using System.Xml;
using UnityEngine;

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

    public override ImageData ToImageData() {
        var imgData = new ImageData();
        imgData.characterID = characterID;
        bool isJpg = imageData[0] == 0xFF && (imageData[1] == 0xD8 || imageData[1] == 0xD9);
        bool isPng = imageData[0] == 0x89
                     && imageData[1] == 0x50
                     && imageData[2] == 0x4E
                     && imageData[3] == 0x47
                     && imageData[4] == 0x0D
                     && imageData[5] == 0x0A
                     && imageData[6] == 0x1A
                     && imageData[7] == 0x0A;
        bool isGif = imageData[0] == 0x47
                     && imageData[1] == 0x49
                     && imageData[2] == 0x46
                     && imageData[3] == 0x38
                     && imageData[4] == 0x39
                     && imageData[5] == 0x61;
        if (isPng) {
            imgData.type = ImageType.Png;
            var texture = new Texture2D(16, 16); //宽高可以任意LoadImage()时会自动调整
            texture.LoadImage(imageData);
            texture.Apply();
            var colors = texture.GetPixels32();
            var len = bitmapAlphaData.Length;

            var alphaData = new byte[len];
            Array.Copy(bitmapAlphaData, alphaData, len);
            alphaData = BitmapUtil.FlipVerticalBitmapAlphaData(alphaData, (ushort)texture.width, (ushort)texture.height);
            for (var i = 0; i < len; i++) {
                colors[i].a = alphaData[i];
            }
            texture.SetPixels32(colors);
            texture.Apply();
            imgData.bytes = texture.EncodeToJPG(100);
        } else if (isJpg || isGif) {
            imgData.type = ImageType.Jpg; //.gif也导出为jpg
            imgData.bytes = imageData;
        }
        return imgData;
    }

}