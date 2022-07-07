using static SortingThing.UI.OptionsMenu;

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
                array.SetColor(i, VerifyCorrectColor.ToColor());
                array.SetColor(i + 1, VerifyCorrectColor.ToColor());
            }
            else
            {
                array.SetColor(i, VerifyWrongColor.ToColor());
                array.SetColor(i + 1, VerifyWrongColor.ToColor());
            }

            Delay();
        }
    }

    public static void RunVerifier()
    {
        if (Running) return;

        Verifying = true;
        RunVerifierInternal();
    }

    public static async void RunVerifierAsync()
    {
        if (Running) return;

        Verifying = true;
        await Task.Run(() => RunVerifierInternal());
    }

    public static void RunVerifierInternal()
    {
        Oscillator.Play();

        new VerifySweep().Run(Program.Array);
        Program.Array.FlushColors();

        Oscillator.Stop();
        Verifying = false;
    }
}
