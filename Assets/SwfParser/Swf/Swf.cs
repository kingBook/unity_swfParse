using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Text;

public class Swf {

    public SwfHeader header;
    public readonly List<SwfTag> tags = new List<SwfTag>(256);
    public readonly List<SymbolClassTag> symbolClassTags = new List<SymbolClassTag>(24);
    public readonly List<DefineSpriteTag> defineSpriteTags = new List<DefineSpriteTag>(128);

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

    #region GetImageDatas
    /// <summary>
    /// 获取所有图像数据
    /// </summary>
    /// <param name="isOnlyExportLinkage"> 仅导出有链接类名的库元件 </param>
    /// <returns></returns>
    public ImageData[] GetImageDatas(bool isOnlyExportLinkage) {
        // 获取有链接类名的 DefineSprite(在SymbolClassTag中定义) 的所有 CharacterId
        List<ushort> characterIds = null;
        if (isOnlyExportLinkage) {
            characterIds = new List<ushort>();
            GetLinkageDefineSpriteTagsNeededCharacterIds(characterIds);
        }

        var imageDatas = new List<ImageData>();
        for (int i = 0, len = tags.Count; i < len; i++) {
            var tag = tags[i];
            if (tag.header.type == (uint)TagType.DefineBits) {
                var defineBitsTag = (DefineBitsTag)tag;
                if (isOnlyExportLinkage && characterIds.IndexOf(defineBitsTag.characterID) > -1) {
                    var imageData = GetDefineBitsImageData(defineBitsTag);
                    if (imageData.bytes != null && imageData.bytes.Length > 0) {
                        imageDatas.Add(imageData);
                    }
                }
            } else if (tag.header.type == (uint)TagType.DefineBitsJPEG2) {
                var defineBitsJpeg2Tag = (DefineBitsJPEG2Tag)tag;
                if (isOnlyExportLinkage && characterIds.IndexOf(defineBitsJpeg2Tag.characterID) > -1) {
                    var imageData = GetDefineBitsJpeg2ImageData(defineBitsJpeg2Tag);
                    imageDatas.Add(imageData);
                }
            } else if (tag.header.type == (uint)TagType.DefineBitsJPEG3) {
                var defineBitsJpeg3Tag = (DefineBitsJPEG3Tag)tag;
                if (isOnlyExportLinkage && characterIds.IndexOf(defineBitsJpeg3Tag.characterID) > -1) {
                    var imageData = GetDefineBitsJpeg3ImageData(defineBitsJpeg3Tag);
                    imageDatas.Add(imageData);
                }
            } else if (tag.header.type == (uint)TagType.DefineBitsLossless) {
                var defineBitsLosslessTag = (DefineBitsLosslessTag)tag;
                if (isOnlyExportLinkage && characterIds.IndexOf(defineBitsLosslessTag.characterID) > -1) {
                    var imageData = GetDefineBitsLosslessImageData(defineBitsLosslessTag);
                    imageDatas.Add(imageData);
                }
            } else if (tag.header.type == (uint)TagType.DefineBitsLossless2) {
                var defineBitsLossless2Tag = (DefineBitsLossless2Tag)tag;
                if (isOnlyExportLinkage && characterIds.IndexOf(defineBitsLossless2Tag.characterID) > -1) {
                    var imageData = GetDefineBitsLossless2ImageData(defineBitsLossless2Tag);
                    imageDatas.Add(imageData);
                }
            } else if (tag.header.type == (uint)TagType.DefineBitsJPEG4) {
                var defineBitsJpeg4Tag = (DefineBitsJPEG4Tag)tag;
                if (isOnlyExportLinkage && characterIds.IndexOf(defineBitsJpeg4Tag.characterID) > -1) {
                    var imageData = GetDefineBitsJPEG4ImageData(defineBitsJpeg4Tag);
                    imageDatas.Add(imageData);
                }
            }
        }
        return imageDatas.ToArray();
    }

    private void GetLinkageDefineSpriteTagsNeededCharacterIds(List<ushort> characterIds) {
        for (int i = 0, len = defineSpriteTags.Count; i < len; i++) {
            var defineSpriteTag = defineSpriteTags[i];
            bool isLinkageDefineSpriteTag = false; // 是否为导出链接类的 DefineSpriteTag
            for (int j = 0, lenJ = symbolClassTags.Count; j < lenJ; j++) {
                var symbols = symbolClassTags[j].symbols;
                for (int k = 0, lenK = symbols.Length; k < lenK; k++) {
                    if (defineSpriteTag.spriteId == symbols[k].tag) {
                        isLinkageDefineSpriteTag = true;
                        break;
                    }
                }
                if (isLinkageDefineSpriteTag) break;
            }
            if (isLinkageDefineSpriteTag) {
                defineSpriteTag.GetNeededCharacterIds(characterIds, this);
            }
        }
    }


    private ImageData GetDefineBitsImageData(DefineBitsTag defineBitsTag) {
        var imageData = new ImageData();
        if (defineBitsTag.jpegData != null) {
            imageData.characterID = defineBitsTag.characterID;
            imageData.type = ImageType.Jpg;
            imageData.bytes = defineBitsTag.jpegData;
        }
        return imageData;
    }

    private ImageData GetDefineBitsJpeg2ImageData(DefineBitsJPEG2Tag defineBitsJpeg2Tag) {
        var imageData = new ImageData();
        imageData.characterID = defineBitsJpeg2Tag.characterID;
        bool isJpg = defineBitsJpeg2Tag.imageData[0] == 0xFF && (defineBitsJpeg2Tag.imageData[1] == 0xD8 || defineBitsJpeg2Tag.imageData[1] == 0xD9);
        bool isPng = defineBitsJpeg2Tag.imageData[0] == 0x89
                     && defineBitsJpeg2Tag.imageData[1] == 0x50
                     && defineBitsJpeg2Tag.imageData[2] == 0x4E
                     && defineBitsJpeg2Tag.imageData[3] == 0x47
                     && defineBitsJpeg2Tag.imageData[4] == 0x0D
                     && defineBitsJpeg2Tag.imageData[5] == 0x0A
                     && defineBitsJpeg2Tag.imageData[6] == 0x1A
                     && defineBitsJpeg2Tag.imageData[7] == 0x0A;
        bool isGif = defineBitsJpeg2Tag.imageData[0] == 0x47
                     && defineBitsJpeg2Tag.imageData[1] == 0x49
                     && defineBitsJpeg2Tag.imageData[2] == 0x46
                     && defineBitsJpeg2Tag.imageData[3] == 0x38
                     && defineBitsJpeg2Tag.imageData[4] == 0x39
                     && defineBitsJpeg2Tag.imageData[5] == 0x61;
        if (isPng) {
            imageData.type = ImageType.Png;
        } else if (isJpg || isGif) {
            imageData.type = ImageType.Jpg;
        }
        imageData.bytes = defineBitsJpeg2Tag.imageData;
        return imageData;
    }

    private ImageData GetDefineBitsJpeg3ImageData(DefineBitsJPEG3Tag defineBitsJpeg3Tag) {
        var imageData = new ImageData();
        imageData.characterID = defineBitsJpeg3Tag.characterID;
        bool isJpg = defineBitsJpeg3Tag.imageData[0] == 0xFF && (defineBitsJpeg3Tag.imageData[1] == 0xD8 || defineBitsJpeg3Tag.imageData[1] == 0xD9);
        bool isPng = defineBitsJpeg3Tag.imageData[0] == 0x89
                     && defineBitsJpeg3Tag.imageData[1] == 0x50
                     && defineBitsJpeg3Tag.imageData[2] == 0x4E
                     && defineBitsJpeg3Tag.imageData[3] == 0x47
                     && defineBitsJpeg3Tag.imageData[4] == 0x0D
                     && defineBitsJpeg3Tag.imageData[5] == 0x0A
                     && defineBitsJpeg3Tag.imageData[6] == 0x1A
                     && defineBitsJpeg3Tag.imageData[7] == 0x0A;
        bool isGif = defineBitsJpeg3Tag.imageData[0] == 0x47
                     && defineBitsJpeg3Tag.imageData[1] == 0x49
                     && defineBitsJpeg3Tag.imageData[2] == 0x46
                     && defineBitsJpeg3Tag.imageData[3] == 0x38
                     && defineBitsJpeg3Tag.imageData[4] == 0x39
                     && defineBitsJpeg3Tag.imageData[5] == 0x61;
        if (isPng) {
            imageData.type = ImageType.Png;
            var texture = new Texture2D(16, 16); //宽高可以任意LoadImage()时会自动调整
            texture.LoadImage(defineBitsJpeg3Tag.imageData);
            texture.Apply();
            var colors = texture.GetPixels32();
            var len = defineBitsJpeg3Tag.bitmapAlphaData.Length;

            var alphaData = new byte[len];
            Array.Copy(defineBitsJpeg3Tag.bitmapAlphaData, alphaData, len);
            FlipVerticalBitmapAlphaData(alphaData, (ushort)texture.width, (ushort)texture.height);
            for (var i = 0; i < len; i++) {
                colors[i].a = alphaData[i];
            }
            texture.SetPixels32(colors);
            texture.Apply();
            imageData.bytes = texture.EncodeToJPG(100);
        } else if (isJpg || isGif) {
            imageData.type = ImageType.Jpg; //.gif也导出为jpg
            imageData.bytes = defineBitsJpeg3Tag.imageData;
        }
        return imageData;
    }

    private ImageData GetDefineBitsLosslessImageData(DefineBitsLosslessTag defineBitsLosslessTag) {
        var texture = new Texture2D(defineBitsLosslessTag.bitmapWidth, defineBitsLosslessTag.bitmapHeight);
        if (defineBitsLosslessTag.bitmapFormat == 3) {
            //ColorMapDataRecord
            var colorMapDataRecord = (ColorMapDataRecord)defineBitsLosslessTag.zlibBitmapData;
            int length = colorMapDataRecord.colormapPixelData.Length;
            var colors = new Color32[length];
            for (int i = 0; i < length; i++) {
                var colorIndex = colorMapDataRecord.colormapPixelData[i];
                var rgb = colorMapDataRecord.colorTableRGB[colorIndex];
                colors[i] = new Color32(rgb.red, rgb.green, rgb.blue, 255);
            }
            colors = FlipVerticalBitmapColors(colors, defineBitsLosslessTag.bitmapWidth, defineBitsLosslessTag.bitmapHeight);
            texture.SetPixels32(colors);
            texture.Apply();
        } else if (defineBitsLosslessTag.bitmapFormat == 4 || defineBitsLosslessTag.bitmapFormat == 5) {
            //BitmapDataRecord
            var bitmapDataRecord = (BitmapDataRecord)defineBitsLosslessTag.zlibBitmapData;
            int length = bitmapDataRecord.bitmapPixelData.Length;
            var colors = new Color32[length];
            if (defineBitsLosslessTag.bitmapFormat == 4) {
                for (int i = 0; i < length; i++) {
                    var pix15 = (Pix15Record)bitmapDataRecord.bitmapPixelData[i];
                    colors[i] = new Color32(pix15.red, pix15.green, pix15.blue, 255);
                }
            } else if (defineBitsLosslessTag.bitmapFormat == 5) {
                for (int i = 0; i < length; i++) {
                    var pix24 = (Pix24Record)bitmapDataRecord.bitmapPixelData[i];
                    colors[i] = new Color32(pix24.red, pix24.green, pix24.blue, 255);
                }
            }
            colors = FlipVerticalBitmapColors(colors, defineBitsLosslessTag.bitmapWidth, defineBitsLosslessTag.bitmapHeight);
            texture.SetPixels32(colors);
            texture.Apply();
        }
        var imageData = new ImageData();
        imageData.characterID = defineBitsLosslessTag.characterID;
        //Png或Jpg都可以
        //imageData.type=ImageType.Png;
        imageData.type = ImageType.Jpg;
        imageData.bytes = texture.EncodeToJPG();
        return imageData;
    }

    private ImageData GetDefineBitsLossless2ImageData(DefineBitsLossless2Tag defineBitsLossless2Tag) {
        var texture = new Texture2D(defineBitsLossless2Tag.bitmapWidth, defineBitsLossless2Tag.bitmapHeight);
        if (defineBitsLossless2Tag.bitmapFormat == 3) {
            //AlphaColorMapDataRecord
            uint bitmapWidth = defineBitsLossless2Tag.bitmapWidth;
            while ((bitmapWidth % 4) != 0) {
                bitmapWidth = (bitmapWidth / 4 + 1) * 4;
            }
            var alphaColorMapDataRecord = (AlphaColorMapDataRecord)defineBitsLossless2Tag.zlibBitmapData;
            var colors = new Color32[defineBitsLossless2Tag.bitmapWidth * defineBitsLossless2Tag.bitmapHeight];
            int length = alphaColorMapDataRecord.colormapPixelData.Length;
            int idx = 0;
            for (int j = 0; j < length; j++) {
                var colorIndex = alphaColorMapDataRecord.colormapPixelData[j];
                var rgba = alphaColorMapDataRecord.colorTableRGB[colorIndex];
                long index = j % bitmapWidth;
                if (index < defineBitsLossless2Tag.bitmapWidth) {
                    colors[idx++] = new Color32(rgba.red, rgba.green, rgba.blue, rgba.alpha);
                }
            }
            colors = FlipVerticalBitmapColors(colors, defineBitsLossless2Tag.bitmapWidth, defineBitsLossless2Tag.bitmapHeight);
            texture.SetPixels32(colors);
            texture.Apply();
        } else if (defineBitsLossless2Tag.bitmapFormat == 4 || defineBitsLossless2Tag.bitmapFormat == 5) {
            //AlphaBitmapDataRecord
            var alphaBitmapDataRecord = (AlphaBitmapDataRecord)defineBitsLossless2Tag.zlibBitmapData;
            int length = alphaBitmapDataRecord.bitmapPixelData.Length;
            var colors = new Color32[length];
            for (int j = 0; j < length; j++) {
                var argb = alphaBitmapDataRecord.bitmapPixelData[j];
                colors[j] = new Color32(argb.red, argb.green, argb.blue, argb.alpha);
            }
            colors = FlipVerticalBitmapColors(colors, defineBitsLossless2Tag.bitmapWidth, defineBitsLossless2Tag.bitmapHeight);
            texture.SetPixels32(colors);
            texture.Apply();
        }
        var imageData = new ImageData();
        imageData.characterID = defineBitsLossless2Tag.characterID;
        imageData.type = ImageType.Png;
        imageData.bytes = texture.EncodeToPNG();
        return imageData;
    }

    private ImageData GetDefineBitsJPEG4ImageData(DefineBitsJPEG4Tag defineBitsJpeg4Tag) {
        var imageData = new ImageData();
        imageData.characterID = defineBitsJpeg4Tag.characterID;
        bool isJpg = defineBitsJpeg4Tag.imageData[0] == 0xFF && (defineBitsJpeg4Tag.imageData[1] == 0xD8 || defineBitsJpeg4Tag.imageData[1] == 0xD9);
        bool isPng = defineBitsJpeg4Tag.imageData[0] == 0x89
                     && defineBitsJpeg4Tag.imageData[1] == 0x50
                     && defineBitsJpeg4Tag.imageData[2] == 0x4E
                     && defineBitsJpeg4Tag.imageData[3] == 0x47
                     && defineBitsJpeg4Tag.imageData[4] == 0x0D
                     && defineBitsJpeg4Tag.imageData[5] == 0x0A
                     && defineBitsJpeg4Tag.imageData[6] == 0x1A
                     && defineBitsJpeg4Tag.imageData[7] == 0x0A;
        bool isGif = defineBitsJpeg4Tag.imageData[0] == 0x47
                     && defineBitsJpeg4Tag.imageData[1] == 0x49
                     && defineBitsJpeg4Tag.imageData[2] == 0x46
                     && defineBitsJpeg4Tag.imageData[3] == 0x38
                     && defineBitsJpeg4Tag.imageData[4] == 0x39
                     && defineBitsJpeg4Tag.imageData[5] == 0x61;
        if (isPng) {
            imageData.type = ImageType.Png;
            var texture = new Texture2D(16, 16); //宽高可以任意LoadImage()时会自动调整
            texture.LoadImage(defineBitsJpeg4Tag.imageData);
            texture.Apply();
            var colors = texture.GetPixels32();
            var len = defineBitsJpeg4Tag.bitmapAlphaData.Length;

            var alphaData = new byte[len];
            Array.Copy(defineBitsJpeg4Tag.bitmapAlphaData, alphaData, len);
            FlipVerticalBitmapAlphaData(alphaData, (ushort)texture.width, (ushort)texture.height);
            for (var i = 0; i < len; i++) {
                colors[i].a = alphaData[i];
            }
            texture.SetPixels32(colors);
            texture.Apply();
            imageData.bytes = texture.EncodeToJPG(100);
        } else if (isJpg || isGif) {
            imageData.type = ImageType.Jpg; //.gif也导出为jpg
            imageData.bytes = defineBitsJpeg4Tag.imageData;
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
    #endregion

}