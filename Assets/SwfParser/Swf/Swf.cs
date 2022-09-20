using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Text;

public class Swf {

    public SwfHeader header;
    public List<SwfTag> tags;

    public XmlDocument ToXml() {
        var doc = new XmlDocument();
        XmlDeclaration declaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
        doc.AppendChild(declaration);

        var swfElement = doc.CreateElement("Swf");
        doc.AppendChild(swfElement);
        var sw = new System.Diagnostics.Stopwatch();
        for (int i = 0, len = tags.Count; i < len; i++) {
            var tag = tags[i];

            sw.Restart();
            var tagXml = tag.ToXml(doc);
            sw.Stop();
            Debug.LogFormat("type:{0},time:{1}", tag.header.type, sw.ElapsedMilliseconds);

            swfElement.AppendChild(tagXml);
        }
        return doc;
    }

    /// <summary>
    /// 获取所有图像数据
    /// </summary>
    public ImageData[] GetImageDatas() {
        var imageDatas = new List<ImageData>();
        for (int i = 0, len = tags.Count; i < len; i++) {
            var tag = tags[i];
            if (tag.header.type == (uint)TagType.DefineBits) {
                var imageData = GetDefineBitsImageData((DefineBitsTag)tag);
                if (imageData.bytes != null && imageData.bytes.Length > 0) {
                    imageDatas.Add(imageData);
                }
            } else if (tag.header.type == (uint)TagType.DefineBitsJPEG2) {
                var imageData = GetDefineBitsJPEG2ImageData((DefineBitsJPEG2Tag)tag);
                imageDatas.Add(imageData);
            } else if (tag.header.type == (uint)TagType.DefineBitsJPEG3) {
                var imageData = GetDefineBitsJPEG3ImageData((DefineBitsJPEG3Tag)tag);
                imageDatas.Add(imageData);
            } else if (tag.header.type == (uint)TagType.DefineBitsLossless) {
                var imageData = GetDefineBitsLosslessImageData((DefineBitsLosslessTag)tag);
                imageDatas.Add(imageData);
            } else if (tag.header.type == (uint)TagType.DefineBitsLossless2) {
                var imageData = GetDefineBitsLossless2ImageData((DefineBitsLossless2Tag)tag);
                imageDatas.Add(imageData);
            } else if (tag.header.type == (uint)TagType.DefineBitsJPEG4) {
                var imageData = GetDefineBitsJPEG4ImageData((DefineBitsJPEG4Tag)tag);
                imageDatas.Add(imageData);
            }
        }
        return imageDatas.ToArray();
    }

    private ImageData GetDefineBitsImageData(DefineBitsTag defineBits) {
        var imageData = new ImageData();
        if (defineBits.jpegData != null) {
            imageData.characterID = defineBits.characterID;
            imageData.type = ImageType.Jpg;
            imageData.bytes = defineBits.jpegData;
        }
        return imageData;
    }

    private ImageData GetDefineBitsJPEG2ImageData(DefineBitsJPEG2Tag defineBitsJPEG2) {
        var imageData = new ImageData();
        imageData.characterID = defineBitsJPEG2.characterID;
        bool isJpg = defineBitsJPEG2.imageData[0] == 0xFF && (defineBitsJPEG2.imageData[1] == 0xD8 || defineBitsJPEG2.imageData[1] == 0xD9);
        bool isPng = defineBitsJPEG2.imageData[0] == 0x89
                     && defineBitsJPEG2.imageData[1] == 0x50
                     && defineBitsJPEG2.imageData[2] == 0x4E
                     && defineBitsJPEG2.imageData[3] == 0x47
                     && defineBitsJPEG2.imageData[4] == 0x0D
                     && defineBitsJPEG2.imageData[5] == 0x0A
                     && defineBitsJPEG2.imageData[6] == 0x1A
                     && defineBitsJPEG2.imageData[7] == 0x0A;
        bool isGif = defineBitsJPEG2.imageData[0] == 0x47
                     && defineBitsJPEG2.imageData[1] == 0x49
                     && defineBitsJPEG2.imageData[2] == 0x46
                     && defineBitsJPEG2.imageData[3] == 0x38
                     && defineBitsJPEG2.imageData[4] == 0x39
                     && defineBitsJPEG2.imageData[5] == 0x61;
        if (isPng) {
            imageData.type = ImageType.Png;
        } else if (isJpg || isGif) {
            imageData.type = ImageType.Jpg;
        }
        imageData.bytes = defineBitsJPEG2.imageData;
        return imageData;
    }

    private ImageData GetDefineBitsJPEG3ImageData(DefineBitsJPEG3Tag defineBitsJPEG3) {
        var imageData = new ImageData();
        imageData.characterID = defineBitsJPEG3.characterID;
        bool isJpg = defineBitsJPEG3.imageData[0] == 0xFF && (defineBitsJPEG3.imageData[1] == 0xD8 || defineBitsJPEG3.imageData[1] == 0xD9);
        bool isPng = defineBitsJPEG3.imageData[0] == 0x89
                     && defineBitsJPEG3.imageData[1] == 0x50
                     && defineBitsJPEG3.imageData[2] == 0x4E
                     && defineBitsJPEG3.imageData[3] == 0x47
                     && defineBitsJPEG3.imageData[4] == 0x0D
                     && defineBitsJPEG3.imageData[5] == 0x0A
                     && defineBitsJPEG3.imageData[6] == 0x1A
                     && defineBitsJPEG3.imageData[7] == 0x0A;
        bool isGif = defineBitsJPEG3.imageData[0] == 0x47
                     && defineBitsJPEG3.imageData[1] == 0x49
                     && defineBitsJPEG3.imageData[2] == 0x46
                     && defineBitsJPEG3.imageData[3] == 0x38
                     && defineBitsJPEG3.imageData[4] == 0x39
                     && defineBitsJPEG3.imageData[5] == 0x61;
        if (isPng) {
            imageData.type = ImageType.Png;
            var texture = new Texture2D(16, 16); //宽高可以任意LoadImage()时会自动调整
            texture.LoadImage(defineBitsJPEG3.imageData);
            texture.Apply();
            var colors = texture.GetPixels32();
            var len = defineBitsJPEG3.bitmapAlphaData.Length;

            var alphaData = new byte[len];
            Array.Copy(defineBitsJPEG3.bitmapAlphaData, alphaData, len);
            FlipVerticalBitmapAlphaData(alphaData, (ushort)texture.width, (ushort)texture.height);
            for (var i = 0; i < len; i++) {
                colors[i].a = alphaData[i];
            }
            texture.SetPixels32(colors);
            texture.Apply();
            imageData.bytes = texture.EncodeToJPG(100);
        } else if (isJpg || isGif) {
            imageData.type = ImageType.Jpg; //.gif也导出为jpg
            imageData.bytes = defineBitsJPEG3.imageData;
        }
        return imageData;
    }

    private ImageData GetDefineBitsLosslessImageData(DefineBitsLosslessTag defineBitsLossless) {
        var texture = new Texture2D(defineBitsLossless.bitmapWidth, defineBitsLossless.bitmapHeight);
        if (defineBitsLossless.bitmapFormat == 3) {
            //ColorMapDataRecord
            var colorMapDataRecord = (ColorMapDataRecord)defineBitsLossless.zlibBitmapData;
            int length = colorMapDataRecord.colormapPixelData.Length;
            var colors = new Color32[length];
            for (int i = 0; i < length; i++) {
                var colorIndex = colorMapDataRecord.colormapPixelData[i];
                var rgb = colorMapDataRecord.colorTableRGB[colorIndex];
                colors[i] = new Color32(rgb.red, rgb.green, rgb.blue, 255);
            }
            colors = FlipVerticalBitmapColors(colors, defineBitsLossless.bitmapWidth, defineBitsLossless.bitmapHeight);
            texture.SetPixels32(colors);
            texture.Apply();
        } else if (defineBitsLossless.bitmapFormat == 4 || defineBitsLossless.bitmapFormat == 5) {
            //BitmapDataRecord
            var bitmapDataRecord = (BitmapDataRecord)defineBitsLossless.zlibBitmapData;
            int length = bitmapDataRecord.bitmapPixelData.Length;
            var colors = new Color32[length];
            if (defineBitsLossless.bitmapFormat == 4) {
                for (int i = 0; i < length; i++) {
                    var pix15 = (Pix15Record)bitmapDataRecord.bitmapPixelData[i];
                    colors[i] = new Color32(pix15.red, pix15.green, pix15.blue, 255);
                }
            } else if (defineBitsLossless.bitmapFormat == 5) {
                for (int i = 0; i < length; i++) {
                    var pix24 = (Pix24Record)bitmapDataRecord.bitmapPixelData[i];
                    colors[i] = new Color32(pix24.red, pix24.green, pix24.blue, 255);
                }
            }
            colors = FlipVerticalBitmapColors(colors, defineBitsLossless.bitmapWidth, defineBitsLossless.bitmapHeight);
            texture.SetPixels32(colors);
            texture.Apply();
        }
        var imageData = new ImageData();
        imageData.characterID = defineBitsLossless.characterID;
        //Png或Jpg都可以
        //imageData.type=ImageType.Png;
        imageData.type = ImageType.Jpg;
        imageData.bytes = texture.EncodeToJPG();
        return imageData;
    }

    private ImageData GetDefineBitsLossless2ImageData(DefineBitsLossless2Tag defineBitsLossless2) {
        var texture = new Texture2D(defineBitsLossless2.bitmapWidth, defineBitsLossless2.bitmapHeight);
        if (defineBitsLossless2.bitmapFormat == 3) {
            //AlphaColorMapDataRecord
            uint bitmapWidth = defineBitsLossless2.bitmapWidth;
            while ((bitmapWidth % 4) != 0) {
                bitmapWidth = (bitmapWidth / 4 + 1) * 4;
            }
            var alphaColorMapDataRecord = (AlphaColorMapDataRecord)defineBitsLossless2.zlibBitmapData;
            var colors = new Color32[defineBitsLossless2.bitmapWidth * defineBitsLossless2.bitmapHeight];
            int length = alphaColorMapDataRecord.colormapPixelData.Length;
            int idx = 0;
            for (int j = 0; j < length; j++) {
                var colorIndex = alphaColorMapDataRecord.colormapPixelData[j];
                var rgba = alphaColorMapDataRecord.colorTableRGB[colorIndex];
                long index = j % bitmapWidth;
                if (index < defineBitsLossless2.bitmapWidth) {
                    colors[idx++] = new Color32(rgba.red, rgba.green, rgba.blue, rgba.alpha);
                }
            }
            colors = FlipVerticalBitmapColors(colors, defineBitsLossless2.bitmapWidth, defineBitsLossless2.bitmapHeight);
            texture.SetPixels32(colors);
            texture.Apply();
        } else if (defineBitsLossless2.bitmapFormat == 4 || defineBitsLossless2.bitmapFormat == 5) {
            //AlphaBitmapDataRecord
            var alphaBitmapDataRecord = (AlphaBitmapDataRecord)defineBitsLossless2.zlibBitmapData;
            int length = alphaBitmapDataRecord.bitmapPixelData.Length;
            var colors = new Color32[length];
            for (int j = 0; j < length; j++) {
                var argb = alphaBitmapDataRecord.bitmapPixelData[j];
                colors[j] = new Color32(argb.red, argb.green, argb.blue, argb.alpha);
            }
            colors = FlipVerticalBitmapColors(colors, defineBitsLossless2.bitmapWidth, defineBitsLossless2.bitmapHeight);
            texture.SetPixels32(colors);
            texture.Apply();
        }
        var imageData = new ImageData();
        imageData.characterID = defineBitsLossless2.characterID;
        imageData.type = ImageType.Png;
        imageData.bytes = texture.EncodeToPNG();
        return imageData;
    }

    private ImageData GetDefineBitsJPEG4ImageData(DefineBitsJPEG4Tag defineBitsJPEG4) {
        var imageData = new ImageData();
        imageData.characterID = defineBitsJPEG4.characterID;
        bool isJpg = defineBitsJPEG4.imageData[0] == 0xFF && (defineBitsJPEG4.imageData[1] == 0xD8 || defineBitsJPEG4.imageData[1] == 0xD9);
        bool isPng = defineBitsJPEG4.imageData[0] == 0x89
                     && defineBitsJPEG4.imageData[1] == 0x50
                     && defineBitsJPEG4.imageData[2] == 0x4E
                     && defineBitsJPEG4.imageData[3] == 0x47
                     && defineBitsJPEG4.imageData[4] == 0x0D
                     && defineBitsJPEG4.imageData[5] == 0x0A
                     && defineBitsJPEG4.imageData[6] == 0x1A
                     && defineBitsJPEG4.imageData[7] == 0x0A;
        bool isGif = defineBitsJPEG4.imageData[0] == 0x47
                     && defineBitsJPEG4.imageData[1] == 0x49
                     && defineBitsJPEG4.imageData[2] == 0x46
                     && defineBitsJPEG4.imageData[3] == 0x38
                     && defineBitsJPEG4.imageData[4] == 0x39
                     && defineBitsJPEG4.imageData[5] == 0x61;
        if (isPng) {
            imageData.type = ImageType.Png;
            var texture = new Texture2D(16, 16); //宽高可以任意LoadImage()时会自动调整
            texture.LoadImage(defineBitsJPEG4.imageData);
            texture.Apply();
            var colors = texture.GetPixels32();
            var len = defineBitsJPEG4.bitmapAlphaData.Length;

            var alphaData = new byte[len];
            Array.Copy(defineBitsJPEG4.bitmapAlphaData, alphaData, len);
            FlipVerticalBitmapAlphaData(alphaData, (ushort)texture.width, (ushort)texture.height);
            for (var i = 0; i < len; i++) {
                colors[i].a = alphaData[i];
            }
            texture.SetPixels32(colors);
            texture.Apply();
            imageData.bytes = texture.EncodeToJPG(100);
        } else if (isJpg || isGif) {
            imageData.type = ImageType.Jpg; //.gif也导出为jpg
            imageData.bytes = defineBitsJPEG4.imageData;
        }
        return imageData;
    }

    /// <summary>
    /// 垂直翻转位图颜色数据
    /// </summary>
    private Color32[] FlipVerticalBitmapColors(Color32[] colors, ushort bitmapWidth, ushort bitmapHeight) {
        var tempColors = new Color32[colors.Length];
        int i = bitmapHeight;
        while (--i >= 0) {
            var sourceStartIndex = i * bitmapWidth;
            var destStartIndex = (bitmapHeight - i - 1) * bitmapWidth;
            Array.Copy(colors, sourceStartIndex, tempColors, destStartIndex, bitmapWidth);
        }
        return tempColors;
    }

    /// <summary>
    /// 垂直翻转位图Alpha数据
    /// </summary>
    private byte[] FlipVerticalBitmapAlphaData(byte[] bitmapAlphaData, ushort bitmapWidth, ushort bitmapHeight) {
        var tempAlphaData = new byte[bitmapAlphaData.Length];
        int i = bitmapHeight;
        while (--i >= 0) {
            var sourceStartIndex = i * bitmapWidth;
            var destStartIndex = (bitmapHeight - i - 1) * bitmapWidth;
            Array.Copy(bitmapAlphaData, sourceStartIndex, tempAlphaData, destStartIndex, bitmapWidth);
        }
        return tempAlphaData;
    }

}