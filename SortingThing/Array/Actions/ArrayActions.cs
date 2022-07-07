using SortingThing.UI;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection;

namespace SortingThing.Array.Actions;

public abstract class ArrayAction
{
    public abstract string Name { get; }
    public abstract void Run(SortingArray array);

    public static ReadOnlyDictionary<string, Sorter> Sorters { get; private set; }
    public static ReadOnlyDictionary<string, Shuffler> Shufflers { get; private set; }
    public static ReadOnlyDictionary<string, Visualizer> Visualizers { get; private set; }

    static ArrayAction()
    {
        Sorters = GatherDerivedClasses<Sorter>();
        Shufflers = GatherDerivedClasses<Shuffler>();
        Visualizers = GatherDerivedClasses<Visualizer>();
    }

    protected virtual void Delay()
    {
        long duration = (long)((OptionsMenu.Delay / 1000) * Stopwatch.Frequency);
        Stopwatch sw = Stopwatch.StartNew();
        while (sw.ElapsedTicks < duration) { }
    }

    private static ReadOnlyDictionary<string, T> GatherDerivedClasses<T>() where T : ArrayAction
    {
        Dictionary<string, T> descendants = new();

        foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (Type t in asm.GetTypes())
            {
                if (t.IsSubclassOf(typeof(T)))
                {
                    T cls = (T)Activator.CreateInstance(t)!;
                    descendants.Add(cls.Name, cls);
                }
            }
        }

        return new ReadOnlyDictionary<string, T>(descendants);
    }
}