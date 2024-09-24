using UnityEngine;
using System.Collections;
using System.Xml;

public struct ButtonRecord {

    public byte buttonReserved;
    public bool buttonHasBlendMode;
    public bool buttonHasFilterList;
    public bool buttonStateHitTest;
    public bool buttonStateDown;
    public bool buttonStateOver;
    public bool buttonStateUp;
    public ushort characterID;
    public ushort placeDepth;
    public MatrixRecord placeMatrix;

    public CXFormWithAlphaRecord colorTransform;
    public FilterListRecord filterList;
    public byte blendMode;

    public byte buttonType; //表示DefineButton1/表示DefineButton2

    public ButtonRecord(SwfByteArray bytes, byte reserved, bool hasBlendMode, bool hasFilterList, bool stateHitTest, bool stateDown, bool stateOver, bool stateUp, byte buttonType) {
        // default value
        colorTransform = new CXFormWithAlphaRecord();
        filterList = new FilterListRecord();
        blendMode = 0;
        // 
        buttonReserved = reserved;
        buttonHasBlendMode = hasBlendMode;
        buttonHasFilterList = hasFilterList;
        buttonStateHitTest = stateHitTest;
        buttonStateDown = stateDown;
        buttonStateOver = stateOver;
        buttonStateUp = stateUp;
        characterID = bytes.ReadUI16();
        placeDepth = bytes.ReadUI16();
        placeMatrix = new MatrixRecord(bytes);
        if (buttonType == 2) {
            colorTransform = new CXFormWithAlphaRecord(bytes);
            if (hasFilterList) {
                filterList = new FilterListRecord(bytes);
            }
            if (hasBlendMode) {
                blendMode = bytes.ReadUI8();
            }
        }
        this.buttonType = buttonType;
    }

    public XmlElement ToXml(XmlDocument doc) {
        var ele = doc.CreateElement("ButtonRecord");
        ele.SetAttribute("buttonReserved", buttonReserved.ToString());
        ele.SetAttribute("buttonHasBlendMode", buttonHasBlendMode.ToString());
        ele.SetAttribute("buttonHasFilterList", buttonHasFilterList.ToString());
        ele.SetAttribute("buttonStateHitTest", buttonStateHitTest.ToString());
        ele.SetAttribute("buttonStateDown", buttonStateDown.ToString());
        ele.SetAttribute("buttonStateOver", buttonStateOver.ToString());
        ele.SetAttribute("buttonStateUp", buttonStateUp.ToString());
        ele.SetAttribute("characterID", characterID.ToString());
        ele.SetAttribute("placeDepth", placeDepth.ToString());
        ele.SetAttribute("placeMatrix", placeMatrix.ToString());
        if (buttonType == 2) {
            ele.AppendChild(colorTransform.ToXml(doc));
            if (buttonHasFilterList) ele.AppendChild(filterList.ToXml(doc));
            if (buttonHasBlendMode) ele.SetAttribute("blendMode", blendMode.ToString());
        }
        ele.SetAttribute("buttonType", buttonType.ToString());
        return ele;
    }

}