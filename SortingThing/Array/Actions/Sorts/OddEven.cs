using SortingThing.Audio;

namespace SortingThing.Array.Actions.Sorts;

public class OddEven : Sorter
{
    public override string Name => "Odd-Even Sort";

    public override void Run(SortingArray array)
    {
        bool sorted = false;

        while (!sorted)
        {
            sorted = true;

            for (int i = 1; i < array.Length - 1; i += 2)
            {
                Oscillator.UpdateFrequency(i, array);

                if (array.Compare(i, i + 1) > 0)
                {
                    array.Swap(i, i + 1);
                    sorted = false;
                }

                Delay();
                array.FlushColors();
            }

            for (int i = 0; i < array.Length - 1; i += 2)
            {
                Oscillator.UpdateFrequency(i, array);

                if (array.Compare(i, i + 1) > 0)
                {
                    array.Swap(i, i + 1);
                    sorted = false;
                }

                Delay();
                array.FlushColors();
            }
        }
    }
}