using SortingThing.UI;

namespace SortingThing;

public class SortingArray
{
    public const int MinSize = 4;
    public const int MaxSize = 8192; // Some visualizers struggle to render at max

    public ulong Reads { get; private set; } = 0;
    public ulong Writes { get; private set; } = 0;
    public ulong Swaps { get; private set; } = 0;
    public ulong Comps { get; private set; } = 0;

    private int[] Array;
    private Color?[] Colors;

    public SortingArray(int size)
    {
        Array = Enumerable.Range(0, size).ToArray();
        Colors = new Color?[size];
        System.Array.Fill(Colors, null);
    }

    public bool Sorted => IsSorted();
    public int Length
    {
        get => Array.Length;
        set => UpdateArraySize(value);
    }

    public int this[int i, bool visual = true]
    {
        get => Get(i, visual);
        set => Set(i, value);
    }

    public int Get(int i, bool visual = true)
    {
        if (visual)
        {
            SetColor(i, OptionsMenu.ReadColor.ToColor());

            if (OptionsMenu.Sorting)
                Reads++;
        }

        return Array[i];
    }

    public void Set(int i, int v)
    {
        SetColor(i, OptionsMenu.WriteColor.ToColor());
        Array[i] = v;

        if (OptionsMenu.Sorting)
            Writes++;
    }

    public int Max()
    {
        return Array.Max();
    }

    public int Min()
    {
        return Array.Min();
    }

    public void Swap(int i1, int i2)
    {
        int temp = Get(i1);
        Set(i1, Get(i2));
        Set(i2, temp);

        if (OptionsMenu.Sorting)
            Swaps++;
    }

    public int Compare(int left, int right)
    {
        return CompareValues(Get(left), Get(right));
    }

    public int CompareValues(int left, int right)
    {
        if (OptionsMenu.Sorting)
            Comps++;

        if (left < right) return -1;
        if (left > right) return 1;
        return 0;
    }

    public Color? GetColor(int i)
    {
        return Colors[i];
    }

    public Color GetColorOrFallback(int i, Color fallback)
    {
        if (OptionsMenu.Rainbow)
        {
            Color c = CalculateRainbow(i);
            return Colors[i] ?? c;
        }

        return Colors[i] ?? fallback;
    }

    public bool HasCustomColor(int i)
    {
        return Colors[i] != null;
    }

    public Color GetColorOrForeground(int i)
    {
        return GetColorOrFallback(i, OptionsMenu.ForegroundColor.ToColor());
    }

    public void SetColor(int i, Color color)
    {
        if (!OptionsMenu.Sorting && !OptionsMenu.Verifying) return;

        if (OptionsMenu.Rainbow && !OptionsMenu.Verifying)
        {
            Colors[i] = OptionsMenu.RainbowInfoColor.ToColor();
            return;
        }

        Colors[i] = color;
    }

    public void SetExtraColor(int i)
    {
        SetColor(i, OptionsMenu.ExtraColor.ToColor());
    }

    public Color CalculateRainbow(int i)
    {
        return ColorFromHSV(Get(i, false) / (float)Length * 360, 1, 1);
    }

    public void FlushColors()
    {
        System.Array.Fill(Colors, null);
    }

    public void FlushStats()
    {
        Reads = 0;
        Writes = 0;
        Swaps = 0;
        Comps = 0;
    }

    public bool IsSorted()
    {
        for (int i = 0; i < Length - 1; i++)
        {
            if (Compare(i, i + 1) > 0)
                return false;
        }

        return true;
    }

    private void UpdateArraySize(int size)
    {
        Array = Enumerable.Range(0, size).ToArray();
        Colors = new Color?[size];
        System.Array.Fill(Colors, null);
    }

    public override string ToString()
    {
        return string.Join(", ", Array);
    }
}