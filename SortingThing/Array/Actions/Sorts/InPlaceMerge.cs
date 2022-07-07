using SortingThing.Audio;

namespace SortingThing.Array.Actions.Sorts;

public class InPlaceMerge : Sorter
{
    public override string Name => "In-Place Merge Sort";

    public override void Run(SortingArray array)
    {
        MergeSort(array, 0, array.Length - 1);
    }

    private void MergeSort(SortingArray array, int left, int right)
    {
        if (array.CompareValues(left, right) < 0)
        {
            int middle = left + (right - left) / 2;

            MergeSort(array, left, middle);
            MergeSort(array, middle + 1, right);

            Merge(array, left, middle, right);
        }
    }

    private void Merge(SortingArray array, int left, int middle, int right)
    {
        int middle2 = middle + 1;

        if (array.Compare(middle, middle2) <= 0)
            return;

        while (array.CompareValues(left, middle) <= 0 && array.CompareValues(middle2, right) <= 0)
        {
            if (array.Compare(left, middle2) <= 0) left++;
            else
            {
                int v = array[middle2];
                int i = middle2;
                
                while (array.CompareValues(i, left) != 0)
                {
                    Oscillator.UpdateFrequency(i, array);

                    array[i] = array[--i];

                    array.SetExtraColor(left);
                    array.SetExtraColor(middle);
                    array.SetExtraColor(middle2);
                    array.SetExtraColor(right);

                    Delay();
                    array.FlushColors();
                }
                array[left] = v;

                left++;
                middle++;
                middle2++;
            }
        }
    }
}