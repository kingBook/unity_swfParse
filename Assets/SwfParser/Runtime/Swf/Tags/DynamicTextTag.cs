using System.Collections.Generic;

public class DynamicTextTag : SwfTag, ICharacterIdTag {

    public ushort characterID;
    public RectangleRecord bounds;
    public bool hasText;
    public bool wordWrap;
    public bool multiline;
    public bool password;
    public bool readOnly;
    public bool hasTextColor;
    public bool hasMaxLength;
    public bool hasFont;
    public bool hasFontClass;
    public bool autoSize;
    public bool hasLayout;
    public bool noSelect;
    public bool border;
    public bool wasStatic;
    public bool html;
    public bool useOutlines;
    public ushort fontID;
    public string fontClass;
    public ushort fontHeight;
    public RGBARecord textColor;
    public ushort maxLength;
    public byte align;
    public ushort leftMargin;
    public ushort rightMargin;
    public ushort indent;
    public short leading;
    public string variableName;
    public string initialText;

    public DynamicTextTag(TagHeaderRecord header) : base(header) {
        // empty constructor
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