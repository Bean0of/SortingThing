using SortingThing.Audio;

namespace SortingThing.Array.Actions.Sorts;

public class Gnome : Sorter
{
    public override string Name => "Gnome Sort";

    public override void Run(SortingArray array)
    {
        for (int i = 0; i < array.Length;)
        {
            Oscillator.UpdateFrequency(i, array);

            if (i == 0 || (int)array.Compare(i, i - 1) >= 0)
                i++;
            else
                array.Swap(i, --i);

            Delay();
            array.FlushColors();
        }
    }
}