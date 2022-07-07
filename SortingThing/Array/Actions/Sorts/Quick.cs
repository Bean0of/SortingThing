using SortingThing.Audio;

namespace SortingThing.Array.Actions.Sorts;

public class Quick : Sorter
{
    public override string Name => "Quick Sort";

    public override void Run(SortingArray array)
    {
        Quicksort(array, 0, array.Length - 1);
    }
    
    private void Quicksort(SortingArray array, int low, int high)
    {
        Oscillator.UpdateFrequency(low, array);

        if (array.CompareValues(low, 0) >= 0 && array.CompareValues(high, 0) >= 0 && array.CompareValues(low, high) < 0)
        {
            int part = Partition(array, low, high);
            Quicksort(array, low, part);
            Quicksort(array, part + 1, high);
        }

        Delay();
        array.FlushColors();
    }

    private int Partition(SortingArray array, int low, int high)
    {
        int pivot = array.Get((int)MathF.Floor((high + low) / 2f));

        int i = low - 1;
        int j = high + 1;

        while (true)
        {
            do
            {
                Oscillator.UpdateFrequency(++i, array);
                Delay();
                array.FlushColors();
            } while (array.CompareValues(array[i], pivot) < 0);

            do
            {
                Oscillator.UpdateFrequency(--j, array);
                Delay();
                array.FlushColors();
            } while (array.CompareValues(array[j], pivot) > 0);

            if (array.CompareValues(i, j) >= 0) return j;

            array.Swap(i, j);

            array.SetExtraColor(low);
            array.SetExtraColor(high);

            Delay();
            array.FlushColors();
        }
    }
}