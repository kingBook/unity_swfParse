using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;
using Debug = UnityEngine.Debug;

public class SwfXmlExporter {

    private Swf m_swf;

    public SwfXmlExporter(Swf swf) {
        m_swf = swf;
    }

    public XmlDocument Export(string swfPath) {
        Stopwatch sw = new Stopwatch();
        sw.Start();
        var xml = m_swf.ToXml();
        sw.Stop();
        Debug.Log("convert xml passed time:" + sw.ElapsedMilliseconds);

        sw.Restart();
        SaveXml(xml, swfPath);
        sw.Stop();
        Debug.Log("save xml passed time:" + sw.ElapsedMilliseconds);
        //Debug.Log(formatXml(swf.toXml()));
        return xml;
    }

    /// <summary> 保存xml文件 </summary>
    private void SaveXml(XmlDocument doc, string swfPath) {
        int dotIdx = swfPath.LastIndexOf('.');
        string xmlPath = $"{swfPath.Substring(0, dotIdx)}.xml";
        doc.Save(xmlPath);
    }

    private string FormatXml(object xml) {
        XmlDocument xd;
        if (xml is XmlDocument document) {
            xd = document;
        } else {
            xd = new XmlDocument();
            xd.LoadXml((string)xml);
        }
        StringBuilder sb = new StringBuilder();
        StringWriter sw = new StringWriter(sb);
        XmlTextWriter xtw = null;
        try {
            xtw = new XmlTextWriter(sw);
            xtw.Formatting = Formatting.Indented;
            xtw.Indentation = 1;
            xtw.IndentChar = '\t';
            xd.WriteTo(xtw);
        } finally {
            xtw?.Close();
        }
        return sb.ToString();
    }
}