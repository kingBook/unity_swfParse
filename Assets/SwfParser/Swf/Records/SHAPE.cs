using System.Collections.Generic;
using System.Xml;

public struct SHAPE {

    public byte numFillBits;
    public byte numLineBits;
    public IShapeRecord[] shapeRecords;

    public SHAPE(SwfReader swfReader, SwfByteArray bytes, byte morphShapeType) {
        numFillBits = (byte)bytes.ReadUB(4);
        numLineBits = (byte)bytes.ReadUB(4);
        var list = new List<IShapeRecord>();
        //3:DefineMorphShape最小支持版本是SWF3与DefineShape3一样；
        //4:DefineMorphShape2最小支持版本是SWF8与DefineShape4一样
        var shapeType = morphShapeType == 1 ? 3 : 4;
        while (true) {
            var shapeRecord = swfReader.ReadShapeRecord(bytes, numFillBits, numLineBits, (byte)shapeType);
            list.Add(shapeRecord);
            if (shapeRecord is StyleChangeRecord) {
                if (((StyleChangeRecord)shapeRecord).stateNewStyles) {
                    numFillBits = ((StyleChangeRecord)shapeRecord).numFillBits;
                    numLineBits = ((StyleChangeRecord)shapeRecord).numLineBits;
                }
            }
            if (shapeRecord is EndShapeRecord) break;
        }
        shapeRecords = list.ToArray();
    }

    public XmlElement ToXml(XmlDocument doc) {
        var ele = doc.CreateElement("SHAPE");
        ele.SetAttribute("numFillBits", numFillBits.ToString());
        ele.SetAttribute("numLineBits", numLineBits.ToString());
        for (var i = 0; i < shapeRecords.Length; i++) {
            ele.AppendChild(shapeRecords[i].ToXml(doc));
        }
        return ele;
    }
}