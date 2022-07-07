using SortingThing.Audio;

namespace SortingThing.Array.Actions.Sorts;

public class Insertion : Sorter
{
    public override string Name => "Insertion Sort";

    public override void Run(SortingArray array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            for (int j = i; j > 0 && array.Compare(j - 1, j) > 0; j--)
            {
                Oscillator.UpdateFrequency(j, array);

                array.Swap(j, j - 1);

                Delay();
                array.FlushColors();
            }
        }
    }
}