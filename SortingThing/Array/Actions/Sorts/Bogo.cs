using SortingThing.Audio;

namespace SortingThing.Array.Actions.Sorts;

public class Bogo : Sorter
{
    public override string Name => "Bogo Sort";

    public override void Run(SortingArray array)
    {
        while (!array.Sorted)
        {
            int rand = Random.Shared.Next(array.Length - 1);
            Oscillator.UpdateFrequency(rand, array);

            for (int i = 0; array.CompareValues(i, array.Length - 1) < 0; i++)
            {
                int j = Random.Shared.Next(i, array.Length);
                array.Swap(i, j);
            }

            Delay();
            array.FlushColors();
        }
    }
}