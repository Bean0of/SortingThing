using static SortingThing.UI.OptionsMenu;

using SortingThing.Audio;

namespace SortingThing.Array.Actions;

public abstract class Shuffler : ArrayAction
{
    public static void RunShuffler(Shuffler shuffler)
    {
        if (Running) return;

        CurrentShuffle = shuffler;
        RunShufflerInternal(shuffler);
    }

    public static async void RunShufflerAsync(Shuffler shuffler)
    {
        if (Running) return;

        CurrentShuffle = shuffler;
        await Task.Run(() => RunShufflerInternal(shuffler));
    }

    private static void RunShufflerInternal(Shuffler shuffler)
    {
        Oscillator.Play();

        shuffler.Run(Program.Array);

        Oscillator.Stop();
        CurrentShuffle = null;
    }
}