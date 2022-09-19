﻿using System.Text;
using System.Xml;

public class DefineSceneAndFrameLabelDataTag : SwfTag {

    public uint sceneCount;
    public DefineSceneRecord[] defineSceneList;
    public uint frameLabelCount;
    public FramelabelRecord[] frameLabelList;

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