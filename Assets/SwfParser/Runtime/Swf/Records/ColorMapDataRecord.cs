using System.Text;
using System.Xml;

[System.Serializable]
public struct ColorMapDataRecord : IMapData {

    public RGBRecord[] colorTableRGB;
    public byte[] colormapPixelData;

    public ColorMapDataRecord(SwfByteArray bytes, uint colorTableSize, uint imageDataSize) {
        colorTableRGB = new RGBRecord[colorTableSize];
        for (uint i = 0; i < colorTableSize; i++) {
            colorTableRGB[i] = new RGBRecord(bytes);
        }

        colormapPixelData = new byte[imageDataSize];
        for (uint i = 0; i < imageDataSize; i++) {
            colormapPixelData[i] = bytes.ReadUI8();
        }
    }

    public XmlElement ToXml(XmlDocument doc) {
        var ele = doc.CreateElement("ColorMapData");

        var colorTableRGBStrBuilder = new StringBuilder("");
        for (int i = 0, length = colorTableRGB.Length; i < length; i++) {
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