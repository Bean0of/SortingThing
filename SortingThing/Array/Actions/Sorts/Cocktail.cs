using SortingThing.Audio;

namespace SortingThing.Array.Actions.Sorts;

public class Cocktail : Sorter
{
    public override string Name => "Cocktail Sort";

    public override void Run(SortingArray array)
    {
        bool swapped = true;
        int start = 0;
        int end = array.Length - 1;

        while (swapped)
        {
            swapped = false;

            for (int i = start; i < end; i++)
            {
                Oscillator.UpdateFrequency(i, array);

                if (array.Compare(i, i + 1) > 0)
                {
                    array.Swap(i, i + 1);
                    swapped = true;
                }

                //array.SetExtraColor(start);
                //array.SetExtraColor(end);

                Delay();
                array.FlushColors();
            }

            if (!swapped)
                break;

            swapped = false;
            end--;

            for (int i = end; i >= start; i--)
            {
                Oscillator.UpdateFrequency(i, array);

                if (array.Compare(i, i + 1) > 0)
                {
                    array.Swap(i, i + 1);
                    swapped = true;
                }

                //array.SetExtraColor(start);
                //array.SetExtraColor(end);

                Delay();
                array.FlushColors();
            }

            start++;
        }
    }
}