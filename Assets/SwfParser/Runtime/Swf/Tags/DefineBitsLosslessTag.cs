using System.Collections.Generic;
using System.Xml;
using UnityEngine;

[System.Serializable]
public class DefineBitsLosslessTag : Tag, ICharacterIdTag {

    public ushort characterID;
    public byte bitmapFormat;
    public ushort bitmapWidth;
    public ushort bitmapHeight;
    public byte bitmapColorTableSize;
    public IMapData zlibBitmapData;

    public DefineBitsLosslessTag(SwfByteArray bytes, TagHeaderRecord header) : base(header) {
        long startPosition = bytes.GetBytePosition();
        characterID = bytes.ReadUI16();
        bitmapFormat = bytes.ReadUI8();
        bitmapWidth = bytes.ReadUI16();
        bitmapHeight = bytes.ReadUI16();
        if (bitmapFormat == 3) {
            bitmapColorTableSize = bytes.ReadUI8();
        }
        if (bitmapFormat == 3 || bitmapFormat == 4 || bitmapFormat == 5) {
            byte[] unzippedData = null;
            long bytesRead = bytes.GetBytePosition() - startPosition;
            int remainingBytes = (int)(header.length - bytesRead);
            if (remainingBytes > 0) {
                unzippedData = bytes.ReadBytes(remainingBytes);
            }
            unzippedData = ZlibUtil.DeCompressBytes(unzippedData);
            var unzippedSwfArray = new SwfByteArray(unzippedData);
            if (bitmapFormat == 3) {
                uint imageDataSize = (uint)(bitmapWidth * bitmapHeight);
                zlibBitmapData = new ColorMapDataRecord(unzippedSwfArray, (uint)(bitmapColorTableSize + 1), imageDataSize);
            } else if (bitmapFormat == 4 || bitmapFormat == 5) {
                uint imageDataSize = (uint)(bitmapWidth * bitmapHeight);
                zlibBitmapData = new BitmapDataRecord(unzippedSwfArray, bitmapFormat, imageDataSize);
            }
        }
    }

    public override XmlElement ToXml(XmlDocument doc) {
        var ele = CreateXmlElement(doc, "DefineBitsLossless");
        ele.SetAttribute("characterID", characterID.ToString());
        ele.SetAttribute("bitmapFormat", bitmapFormat.ToString());
        ele.SetAttribute("bitmapWidth", bitmapWidth.ToString());
        ele.SetAttribute("bitmapHeight", bitmapHeight.ToString());
        ele.SetAttribute("bitmapColorTableSize", bitmapFormat.ToString());
        ele.AppendChild(zlibBitmapData.ToXml(doc));
        return ele;
    }

    public ImageData ToImageData() {
        var texture = new Texture2D(bitmapWidth, bitmapHeight);
        if (bitmapFormat == 3) {
            //ColorMapDataRecord
            var colorMapDataRecord = (ColorMapDataRecord)zlibBitmapData;
            int length = colorMapDataRecord.colormapPixelData.Length;
            var colors = new Color32[length];
            for (int i = 0; i < length; i++) {
                var colorIndex = colorMapDataRecord.colormapPixelData[i];
                var rgb = colorMapDataRecord.colorTableRGB[colorIndex];
                colors[i] = new Color32(rgb.red, rgb.green, rgb.blue, 255);
            }
            colors = BitmapUtil.FlipVerticalBitmapColors(colors, bitmapWidth, bitmapHeight);
            texture.SetPixels32(colors);
            texture.Apply();
        } else if (bitmapFormat == 4 || bitmapFormat == 5) {
            //BitmapDataRecord
            var bitmapDataRecord = (BitmapDataRecord)zlibBitmapData;
            int length = bitmapDataRecord.bitmapPixelData.Length;
            var colors = new Color32[length];
            if (bitmapFormat == 4) {
                for (int i = 0; i < length; i++) {
                    var pix15 = (Pix15Record)bitmapDataRecord.bitmapPixelData[i];
                    colors[i] = new Color32(pix15.red, pix15.green, pix15.blue, 255);
                }
            } else if (bitmapFormat == 5) {
                for (int i = 0; i < length; i++) {
                    var pix24 = (Pix24Record)bitmapDataRecord.bitmapPixelData[i];
                    colors[i] = new Color32(pix24.red, pix24.green, pix24.blue, 255);
                }
            }
            colors = BitmapUtil.FlipVerticalBitmapColors(colors, bitmapWidth, bitmapHeight);
            texture.SetPixels32(colors);
            texture.Apply();
        }
        var imageData = new ImageData();
        imageData.characterID = characterID;
        // Png或Jpg都可以
        //imageData.type=ImageType.Png;
        imageData.type = ImageType.Jpg;
        imageData.bytes = texture.EncodeToJPG();
        return imageData;
    }

    public void FindUsedCharacterIds(List<ushort> characterIds, Swf swf) {
        if (characterIds.IndexOf(characterID) < 0) {
            characterIds.Add(characterID);
        }
    }

    public ushort GetCharacterId() {
        return characterID;
    }

}