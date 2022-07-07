using SortingThing.Audio;

namespace SortingThing.Array.Actions.Shuffles;

public class ReverseEven : Shuffler
{
    public override string Name => "Reverse Even";

    public override void Run(SortingArray array)
    {
        for (int i = 0; i < (int)Math.Floor((array.Length) / 2f); i++)
        {
            Oscillator.UpdateFrequency(i, array);
            
            if (i % 2 == 0) array.Swap(i, array.Length - 1- i);
            
            Delay();
            array.FlushColors();
        }
    }
}
