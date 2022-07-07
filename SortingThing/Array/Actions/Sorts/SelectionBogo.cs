using SortingThing.Audio;

namespace SortingThing.Array.Actions.Sorts;

public class SelectionBogo : Sorter
{
    public override string Name => "Selection Bogo Sort";

    public override void Run(SortingArray array)
    {
        int end = array.Length;

        for (int i = 0; array.CompareValues(i, array.Length) < 0; i++)
        {
            while (true)
            {
                Oscillator.UpdateFrequency(Random.Shared.Next(i, end), array);

                if (i == end) break;

                int min = Min(array, i, end);
                if (array.CompareValues(array[i], min) == 0)
                    break;

                ShuffleSection(array, i, end);

                Delay();
                array.FlushColors();
            }
        }
    }

    private void ShuffleSection(SortingArray array, int start, int end)
    {
        for (int i = start; array.CompareValues(i, end) < 0; i++)
        {
            int j = Random.Shared.Next(i, end);
            array.Swap(i, j);
        }
    }

    private int Min(SortingArray array, int start, int end)
    {
        int min = int.MaxValue;
        for (int i = start; array.CompareValues(i, end) < 0; i++)
        {
            int v = array[i];
            if (array.CompareValues(v, min) < 0)
                min = v;
        }
        return min;
    }
}