using System.Text;
using System.Xml;

public struct AlphaBitmapDataRecord : IAlphaMapData {

    public ARGBRecord[] bitmapPixelData;

    public AlphaBitmapDataRecord(SwfByteArray bytes, uint imageDataSize) {
        bitmapPixelData = new ARGBRecord[imageDataSize];
        for (uint i = 0; i < imageDataSize; i++) {
            bitmapPixelData[i] = new ARGBRecord(bytes);
        }
    }

    public XmlElement ToXml(XmlDocument doc) {
        var ele = doc.CreateElement("AlphaBitmapData");

        var bitmapPixelDataStrBuilder = new StringBuilder();
        int len = bitmapPixelData.Length;
        int maxId = len - 1;
        for (var i = 0; i < len; i++) {
            bitmapPixelDataStrBuilder.Append(bitmapPixelData[i].ToString());
            if (i < maxId) {
                bitmapPixelDataStrBuilder.Append(',');
            }
        }
        ele.SetAttribute("bitmapPixelData", bitmapPixelDataStrBuilder.ToString());
        return ele;
    }
}