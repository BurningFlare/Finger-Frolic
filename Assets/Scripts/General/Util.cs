using UnityEngine;

public class Util
{
    public static Color32 GetColorFromHash(int hash)
    {
        byte r = (byte)((hash >> 16) & 0xFF);
        byte g = (byte)((hash >> 8) & 0xFF);
        byte b = (byte)(hash & 0xFF);

        return new Color32(r, g, b, 255);
    }
}
