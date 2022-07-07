using SortingThing.Audio;

namespace SortingThing.Array.Actions.Shuffles;

public class AlmostSorted : Shuffler
{
    public override string Name => "Almost Sorted";

    public override void Run(SortingArray array)
    {
        for (int i = 0; i < 16; i++)
        {
            int rand = Random.Shared.Next(array.Length);
            int rand2 = Random.Shared.Next(array.Length);

            Oscillator.UpdateFrequency(rand, array);
            array.Swap(rand, rand2);

            Delay();
            array.FlushColors();
        }
    }
}
