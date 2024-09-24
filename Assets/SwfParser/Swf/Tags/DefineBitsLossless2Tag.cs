using System.Collections.Generic;
using System.Xml;

public class DefineBitsLossless2Tag : SwfTag, ICharacterIdTag {

    public ushort characterID;
    public byte bitmapFormat;
    public ushort bitmapWidth;
    public ushort bitmapHeight;
    public byte bitmapColorTableSize;
    public IAlphaMapData zlibBitmapData;

    public DefineBitsLossless2Tag(SwfByteArray bytes, TagHeaderRecord header) : base(header) {
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
                uint bitmapW = bitmapWidth;
                while ((bitmapW % 4) != 0) {
                    bitmapW = (bitmapW / 4 + 1) * 4;
                }
                uint imageDataSize = bitmapW * bitmapHeight;
                zlibBitmapData = new AlphaColorMapDataRecord(unzippedSwfArray, (uint)(bitmapColorTableSize + 1), imageDataSize);
            } else if (bitmapFormat == 4 || bitmapFormat == 5) {
                uint imageDataSize = (uint)(bitmapWidth * bitmapHeight);
                zlibBitmapData = new AlphaBitmapDataRecord(unzippedSwfArray, imageDataSize);
            }
            unzippedSwfArray.Close();
        }
    }

    public override XmlElement ToXml(XmlDocument doc) {
        var ele = CreateXmlElement(doc, "DefineBitsLossless2");
        ele.SetAttribute("characterID", characterID.ToString());
        ele.SetAttribute("bitmapFormat", bitmapFormat.ToString());
        ele.SetAttribute("bitmapWidth", bitmapWidth.ToString());
        ele.SetAttribute("bitmapHeight", bitmapHeight.ToString());
        ele.SetAttribute("bitmapColorTableSize", bitmapFormat.ToString());
        ele.AppendChild(zlibBitmapData.ToXml(doc));
        return ele;
    }

    public void GetNeededCharacterIds(List<ushort> characterIds, Swf swf) {
        if (characterIds.IndexOf(characterID) < 0) {
            characterIds.Add(characterID);
        }
    }

    public ushort GetCharacterId() {
        return characterID;
    }

}