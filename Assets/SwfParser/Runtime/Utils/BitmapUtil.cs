using System;
using UnityEngine;

public static class BitmapUtil {

    /// <summary>
    /// 垂直翻转位图颜色数据
    /// </summary>
    public static Color32[] FlipVerticalBitmapColors(Color32[] colors, ushort bitmapWidth, ushort bitmapHeight) {
        var tempColors = new Color32[colors.Length];
        int i = bitmapHeight;
        while (--i >= 0) {
            var sourceStartIndex = i * bitmapWidth;
            var destStartIndex = (bitmapHeight - i - 1) * bitmapWidth;
            Array.Copy(colors, sourceStartIndex, tempColors, destStartIndex, bitmapWidth);
        }
        return tempColors;
    }

    /// <summary>
    /// 垂直翻转位图Alpha数据
    /// </summary>
    public static byte[] FlipVerticalBitmapAlphaData(byte[] bitmapAlphaData, ushort bitmapWidth, ushort bitmapHeight) {
        var tempAlphaData = new byte[bitmapAlphaData.Length];
        int i = bitmapHeight;
        while (--i >= 0) {
            var sourceStartIndex = i * bitmapWidth;
            var destStartIndex = (bitmapHeight - i - 1) * bitmapWidth;
            Array.Copy(bitmapAlphaData, sourceStartIndex, tempAlphaData, destStartIndex, bitmapWidth);
        }
        return tempAlphaData;
    }
}
