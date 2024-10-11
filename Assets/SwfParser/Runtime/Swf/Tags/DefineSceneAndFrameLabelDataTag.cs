using System.Xml;

[System.Serializable]
public class DefineSceneAndFrameLabelDataTag : Tag {

    public uint sceneCount;
    public DefineSceneRecord[] defineSceneList;
    public uint frameLabelCount;
    public FramelabelRecord[] frameLabelList;

    public DefineSceneAndFrameLabelDataTag(SwfByteArray bytes, TagHeaderRecord header) : base(header) {
        sceneCount = bytes.ReadEncodedUI32();
        defineSceneList = new DefineSceneRecord[sceneCount];
        for (uint i = 0; i < sceneCount; i++) {
            defineSceneList[i] = new DefineSceneRecord(bytes);
        }

        frameLabelCount = bytes.ReadEncodedUI32();
        frameLabelList = new FramelabelRecord[frameLabelCount];
        for (uint i = 0; i < frameLabelCount; i++) {
            frameLabelList[i] = new FramelabelRecord(bytes);
        }
    }

    public override XmlElement ToXml(XmlDocument doc) {
        var ele = CreateXmlElement(doc, "DefineSceneAndFrameLabelData");
        ele.SetAttribute("sceneCount", sceneCount.ToString());
        ele.SetAttribute("frameLabelCount", frameLabelCount.ToString());
        for (uint i = 0; i < sceneCount; i++) {
            var record = defineSceneList[i];
            var recordEle = CreateXmlElement(doc, "DefineScene");
            recordEle.SetAttribute("offset", record.offset.ToString());
            recordEle.SetAttribute("name", record.name);
            ele.AppendChild(recordEle);
        }
        for (uint i = 0; i < frameLabelCount; i++) {
            var record = frameLabelList[i];
            var recordEle = CreateXmlElement(doc, "FrameLabel");
            recordEle.SetAttribute("frameNum", record.frameNum.ToString());
            recordEle.SetAttribute("frameLabel", record.frameLabel);
            ele.AppendChild(recordEle);
        }
        return ele;
    }

}