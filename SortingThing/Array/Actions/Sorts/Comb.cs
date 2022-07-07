using SortingThing.Audio;

namespace SortingThing.Array.Actions.Sorts;

public class Comb : Sorter
{
    public override string Name => "Comb Sort";

    public override void Run(SortingArray array)
    {
        int gap = array.Length;
        float shrink = 1.3f;
        bool sorted = false;

        while (!sorted)
        {
            gap = (int)MathF.Floor(gap / shrink);

            if (gap <= 1)
            {
                gap = 1;
                sorted = true;
            }

            for (int i = 0; i + gap < array.Length; i++)
            {
                Oscillator.UpdateFrequency(i, array);

                if (array.Compare(i, i + gap) > 0)
                {
                    array.Swap(i, i + gap);
                    sorted = false;
                }

                Delay();
                array.FlushColors();
            }
        }
    }
}