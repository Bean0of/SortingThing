using SortingThing.Array.Actions;
using SortingThing.Audio;
using SortingThing.UI;

namespace SortingThing;

public class VerifySweep : ArrayAction
{
    public override string Name => "Verify";

    public override void Run(SortingArray array)
    {
        for (int i = 0; i < array.Length - 1; i += 2)
        {
            Oscillator.UpdateFrequency(i, array);
            
            if (array.Compare(i, i + 1) <= 0)
            {
                array.SetColor(i, OptionsMenu.VerifyCorrectColor.ToColor());
                array.SetColor(i + 1, OptionsMenu.VerifyCorrectColor.ToColor());
            }
            else
            {
                array.SetColor(i, OptionsMenu.VerifyWrongColor.ToColor());
                array.SetColor(i + 1, OptionsMenu.VerifyWrongColor.ToColor());
            }

            Delay();
        }
    }
}
