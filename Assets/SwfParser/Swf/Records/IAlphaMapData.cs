using System.Xml;

/// <summary>
/// 子类：
/// <see cref="AlphaColorMapDataRecord"/>
/// <see cref="AlphaBitmapDataRecord"/>
/// </summary>
public interface IAlphaMapData {
    XmlElement ToXml(XmlDocument doc);

}