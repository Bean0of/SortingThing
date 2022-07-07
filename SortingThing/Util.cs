namespace SortingThing;

public static class Util
{
    public static Color ToColor(this Vector4 vec)
    {
        return new Color((byte)(vec.X * 255), (byte)(vec.Y * 255), (byte)(vec.Z * 255), (byte)(vec.W * 255));
    }

    public static Vector4 ToVector(this Color color)
    {
        return new Vector4(color.r / 255f, color.g / 255f, color.b / 255f, color.a / 255f);
    }

    public static Color Lerp(Color c1, Color c2, float amount)
    {
        return new Color(
            (byte)Raymath.Lerp(c1.r, c2.r, amount),
            (byte)Raymath.Lerp(c1.g, c2.g, amount),
            (byte)Raymath.Lerp(c1.b, c2.b, amount),
            (byte)Raymath.Lerp(c1.a, c2.a, amount)
        );
    }
}