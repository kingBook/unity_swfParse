using System.Xml;

[System.Serializable]
public struct FilterRecord {

    public byte filterId;
    public DropShadowFilterRecord dropShadowFilter;
    public BlurFilterRecord blurFilter;
    public GlowFilterRecord glowFilter;
    public BevelFilterRecord bevelFilter;
    public GradientGlowFilterRecord gradientGlowFilter;
    public ConvolutionFilterRecord convolutionFilter;
    public ColorMatrixFilterRecord colorMatrixFilter;
    public GradientBevelFilterRecord gradientBevelFilter;

    public FilterRecord(SwfByteArray bytes) {
        // default value
        dropShadowFilter = new DropShadowFilterRecord();
        blurFilter = new BlurFilterRecord();
        glowFilter = new GlowFilterRecord();
        bevelFilter = new BevelFilterRecord();
        gradientGlowFilter = new GradientGlowFilterRecord();
        convolutionFilter = new ConvolutionFilterRecord();
        colorMatrixFilter = new ColorMatrixFilterRecord();
        gradientBevelFilter = new GradientBevelFilterRecord();
        //
        filterId = bytes.ReadUI8();
        switch (filterId) {
            case 0:
                dropShadowFilter = new DropShadowFilterRecord(bytes);
                break;
            case 1:
                blurFilter = new BlurFilterRecord(bytes);
                break;
            case 2:
                glowFilter = new GlowFilterRecord(bytes);
                break;
            case 3:
                bevelFilter = new BevelFilterRecord(bytes);
                break;
            case 4:
                gradientGlowFilter = new GradientGlowFilterRecord(bytes);
                break;
            case 5:
                convolutionFilter = new ConvolutionFilterRecord(bytes);
                break;
            case 6:
                colorMatrixFilter = new ColorMatrixFilterRecord(bytes);
                break;
            case 7:
                gradientBevelFilter = new GradientBevelFilterRecord(bytes);
                break;
        }
    }

    public XmlElement ToXml(XmlDocument doc) {
        var ele = doc.CreateElement("Filter");
        ele.SetAttribute("filterId", filterId.ToString());
        switch (filterId) {
            case 0:
                ele.AppendChild(dropShadowFilter.ToXml(doc));
                break;
            case 1:
                ele.AppendChild(blurFilter.ToXml(doc));
                break;
            case 2:
                ele.AppendChild(glowFilter.ToXml(doc));
                break;
            case 3:
                ele.AppendChild(bevelFilter.ToXml(doc));
                break;
            case 4:
                ele.AppendChild(gradientGlowFilter.ToXml(doc));
                break;
            case 5:
                ele.AppendChild(convolutionFilter.ToXml(doc));
                break;
            case 6:
                ele.AppendChild(colorMatrixFilter.ToXml(doc));
                break;
            case 7:
                ele.AppendChild(gradientBevelFilter.ToXml(doc));
                break;
        }
        return ele;
    }
}