using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorUtils
{
    static public Color GetComplementaryColorRGB(Color colorRGB)
    {
        Vector3 colorHSV = new Vector3();
        Color.RGBToHSV(colorRGB, out colorHSV.x, out colorHSV.y, out colorHSV.z);
        ShiftHue(ref colorHSV.x, 0.5f);
        return Color.HSVToRGB(colorHSV.x, colorHSV.y, colorHSV.z);
    }

    static public Color ShiftHueColor(Color colorRGB, float shift)
    {
        Vector3 colorHSV = new Vector3();
        Color.RGBToHSV(colorRGB, out colorHSV.x, out colorHSV.y, out colorHSV.z);
        ShiftHue(ref colorHSV.x, shift);
        return Color.HSVToRGB(colorHSV.x, colorHSV.y, colorHSV.z);
    }

    static public void ShiftHue(ref float h, float shift)
    {
        h = (h + shift) % 1.0f;
    }
}
