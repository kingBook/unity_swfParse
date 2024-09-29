using System.Collections;
using System.IO;
using System.Text;
using System.Xml;
using UnityEngine;

public static class Debug2 {

    /// <summary>
    /// 画多点线
    /// </summary>
    /// <param name="points"></param>
    /// <param name="color"></param>
    /// <param name="duration"></param>
    /// <param name="depthTest"> 该线是否应被靠近摄影机的对象遮挡？ </param>
    public static void DrawPoints(Vector3[] points, Color color, float duration = 0f, bool depthTest = true) {
        for (int i = 0, length = points.Length; i < length; i++) {
            if (i < length - 1) {
                Debug.DrawLine(points[i], points[i + 1], color, duration, depthTest);
            }
        }
    }

    public static void Log(params object[] args) {
        int len = args.Length;
        string str = "";
        for (int i = 0; i < len; i++) {
            str += GetObjectString(args[i]);
            if (i < len - 1) str += ' ';
        }
        Debug.Log(str);
    }

    private static string GetListString(IList list) {
        int len = list.Count;
        string str = "";
        for (int i = 0; i < len; i++) {
            str += GetObjectString(list[i]);
            if (i < len - 1) str += ", ";
        }
        return str;
    }

    private static string GetObjectString(object obj) {
        if (obj is Vector3 v3) {
            return $"({v3.x},{v3.y},{v3.z})";
        } else if (obj is Vector2 v2) {
            return $"({v2.x},{v2.y})";
        } else if (obj is IList list) {
            return GetListString(list);
        } else if (obj is Vector4 v4) {
            return $"({v4.x},{v4.y},{v4.z},{v4.w})";
        }
        return (obj == null) ? "Null" : obj.ToString();
    }

    public static string FormatXml(object xml) {
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
            if (xtw != null) xtw.Close();
        }
        return sb.ToString();
    }
}