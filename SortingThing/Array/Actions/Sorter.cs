using static SortingThing.UI.OptionsMenu;

using SortingThing.UI;
using System.Diagnostics;
using SortingThing.Audio;
using SortingThing.Scripting;

namespace SortingThing.Array.Actions;

public abstract class Sorter : ArrayAction
{
    private Stopwatch? Timer;
    private long Iteration = 0;

    protected override void Delay()
    {
        if (Timer == null) Timer = Stopwatch.StartNew();

        base.Delay();

        Iteration++;
        EstRealTime = (Timer.ElapsedTicks - ((long)((OptionsMenu.Delay / 1000) *
            Stopwatch.Frequency) * Iteration)) /
            (double)Stopwatch.Frequency / 1000d;
    }

    public static void RunSorter(Sorter sorter)
    {
        if (Running) return;

        RunSorterInternal(sorter);
        RunSorterInternal2(sorter);
    }

    public static async void RunSorterAsync(Sorter sorter)
    {
        if (Running) return;

        RunSorterInternal(sorter);
        await Task.Run(() => RunSorterInternal2(sorter));
    }

    private static void RunSorterInternal(Sorter sorter)
    {
        Program.Array.FlushStats();

        TotalSortTime = 0;
        EstRealTime = 0;

        CurrentSort = sorter;
        Oscillator.Play();
    }

    private static void RunSorterInternal2(Sorter sorter)
    {
        sorter.Run(Program.Array);

        Program.Array.FlushColors();
        CurrentSort = null;

        if (DoVerifySweep && !RunScript.ScriptControlled) VerifySweep.RunVerifier();

        Oscillator.Stop();
    }
}