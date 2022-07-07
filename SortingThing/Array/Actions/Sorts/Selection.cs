using SortingThing.Audio;

namespace SortingThing.Array.Actions.Sorts;

public class Selection : Sorter
{
    public override string Name => "Selection Sort";

    public override void Run(SortingArray array)
    {
        for (int i = 0; i < array.Length-1; i++)
        {
            int min = i;
            for (int j = i+1; j < array.Length; j++)
            {
                Oscillator.UpdateFrequency(j, array);

                if (array.Compare(j, min) < 0)
                    min = j;

                Delay();
                array.FlushColors();
            }

            if (min != i)
                array.Swap(i, min);

        }
    }
}