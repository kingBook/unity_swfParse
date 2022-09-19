using System.IO;
using System.Text;
using System.Xml;

public static class Debug2 {
    
    public static void Log(params object[] args) {
        string format = "";
        for (int i = 0; i < args.Length; i++) {
            format += "{" + i + "}" + (i < args.Length - 1 ? "," : "");
        }
        UnityEngine.Debug.LogFormat(format, args);
    }

    public static string FormatXml(object xml) {
        XmlDocument xd;
        if (xml is XmlDocument) {
            xd = xml as XmlDocument;
        } else {
            xd = new XmlDocument();
            xd.LoadXml(xml as string);
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
            if (xtw != null) xtw.Close();
        }
        return sb.ToString();
    }
}