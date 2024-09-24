using System.Text;
using System.Xml;

public struct BitmapDataRecord : IMapData {

    public IPixRecord[] bitmapPixelData;

    public BitmapDataRecord(SwfByteArray bytes, byte bitmapformat, uint imageDataSize) {
        bitmapPixelData = new IPixRecord[imageDataSize];
        if (bitmapformat == 4) {
            for (uint i = 0; i < imageDataSize; i++) {
                bitmapPixelData[i] = new Pix15Record(bytes);
            }
        } else if (bitmapformat == 5) {
            for (uint i = 0; i < imageDataSize; i++) {
                bitmapPixelData[i] = new Pix24Record(bytes);
            }
        }
    }

    public XmlElement ToXml(XmlDocument doc) {
        var ele = doc.CreateElement("BitmapData");

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