using SortingThing.Audio;

namespace SortingThing.Array.Actions.Sorts;

public class Bubble : Sorter
{
    public override string Name => "Bubble Sort";

    public override void Run(SortingArray array)
    {
        for (int i = array.Length - 1; i >= 0; i--)
        {
            for (int j = 0; array.CompareValues(j, i) < 0; j++)
            {
                Oscillator.UpdateFrequency(j, array);

                if (array.Compare(j, j + 1) > 0)
                    array.Swap(j, j + 1);

                Delay();
                array.FlushColors();
            }
        }
    }
}