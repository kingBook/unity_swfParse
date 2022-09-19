﻿using System.Text;
using System.Xml;

public struct AlphaColorMapDataRecord : IAlphaMapData {

    public RGBARecord[] colorTableRGB;
    public byte[] colormapPixelData;

    public XmlElement ToXml(XmlDocument doc) {
        var ele = doc.CreateElement("AlphaColorMapData");

        var colorTableRGBStrBuilder = new StringBuilder("");
        for (int i = 0; i < colorTableRGB.Length; i++) {
            colorTableRGBStrBuilder.Append(colorTableRGB[i]);
            if (i < colorTableRGB.Length - 1) {
                colorTableRGBStrBuilder.Append(',');
            }
        }
        ele.SetAttribute("colorTableRGB", colorTableRGBStrBuilder.ToString());

        var colormapPixelDataStrBuilder = new StringBuilder();
        int len = colormapPixelData.Length;
        int maxId = len - 1;
        for (var i = 0; i < len; i++) {
            colormapPixelDataStrBuilder.Append(colormapPixelData[i].ToString());
            if (i < maxId) {
                colormapPixelDataStrBuilder.Append(',');
            }
        }
        ele.SetAttribute("colormapPixelData", colormapPixelDataStrBuilder.ToString());
        return ele;
    }
}