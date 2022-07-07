using SortingThing.Audio;

namespace SortingThing.Array.Actions.Shuffles;

public class Scramble : Shuffler
{
    public override string Name => "Scramble";

    public override void Run(SortingArray array)
    {
        for (int i = 0; i < array.Length - 1; i++)
        {
            Oscillator.UpdateFrequency(i, array);
            int j = Random.Shared.Next(i, array.Length);
            array.Swap(i, j);

            Delay();
            array.FlushColors();
        }
    }
}
