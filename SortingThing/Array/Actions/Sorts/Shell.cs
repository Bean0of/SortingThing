using SortingThing.Audio;

namespace SortingThing.Array.Actions.Sorts;

public class Shell : Sorter
{
    public override string Name => "Shell Sort";

    public override void Run(SortingArray array)
    {
        for (int gap = array.Length/2; gap > 0; gap /= 2)
        {
            for (int i = gap; i < array.Length; i++)
            {
                Oscillator.UpdateFrequency(i, array);

                int temp = array[i];
                int j;

                for(j = i; j >= gap && array[j - gap] > temp; j -= gap)
                {
                    array[j] = array[j - gap];
                }

                array[j] = temp;

                Delay();
                array.FlushColors();
            }
        }
    }
}