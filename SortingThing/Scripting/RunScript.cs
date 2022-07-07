using MoonSharp.Interpreter;
using ImGuiNET;
using SortingThing.UI;
using SortingThing.Array.Actions;

namespace SortingThing.Scripting;

public class RunScript
{
    private const string HelpURL = "https://github.com/Bean0of/SortingThing";
    private const string Folder = "RunScripts";

    private const float MaxSleep = 10;

    private const CoreModules LuaModules = CoreModules.Basic |
        CoreModules.Math |
        CoreModules.OS_Time |
        CoreModules.TableIterators |
        CoreModules.Metatables |
        CoreModules.Bit32;

    public static bool ScriptControlled => CurrentScript != null;

    public static RunScript? CurrentScript;
    private static List<string> Scripts = new List<string>();

    static RunScript() { UpdateScriptsList(Scripts); }

    private string ScriptFile;

    public RunScript(string file)
    {
        ScriptFile = file;
    }

    public void Run()
    {
        Script script = new Script(LuaModules);

        script.Globals["print"] = (Action<DynValue>)LuaPrint;
        script.Globals["wait"] = (Action<float>)LuaSleep;
        script.Globals["delay"] = (Action<double>)LuaSetDelay;
        script.Globals["size"] = (Action<int>)LuaSetSize;
        script.Globals["shuffle"] = (Action<string>)LuaShuffle;
        script.Globals["sort"] = (Action<string>)LuaSort;
        script.Globals["visual"] = (Action<string>)LuaVisual;
        script.Globals["verify"] = (Action)LuaVerify;
        script.Globals["rainbow"] = (Action<bool>)LuaSetRainbow;

        try { script.DoFile(ScriptFile); }
        catch (InterpreterException e)
        {
            TraceLog(LOG_ERROR, $"RUNSCRIPT: {e.DecoratedMessage}");
        }
        catch (RunScriptException e)
        {
            TraceLog(LOG_ERROR, $"RUNSCRIPT: {e.Message}");
        }
    }

    public static unsafe void DrawMenu()
    {
        if (ImGui.Button("Help")) OpenURL(HelpURL.ToUTF8Buffer().AsPointer());
        ImGui.SameLine(0, ImGui.GetStyle().ItemInnerSpacing.X);
        if (ImGui.Button("Open Folder")) OpenURL(Folder.ToUTF8Buffer().AsPointer());
        ImGui.SameLine(0, ImGui.GetStyle().ItemInnerSpacing.X);
        if (ImGui.Button("Reload")) UpdateScriptsList(Scripts);

        if (DrawFileList(out string file))
        {
            if (!File.Exists(file)) return;
            RunFile(file);
        }
    }

    private static async void RunFile(string file)
    {
        if (CurrentScript != null) return;

        await Task.Run(() =>
        {
            CurrentScript = new RunScript(file);
            CurrentScript.Run();
            CurrentScript = null;
        });
    }

    private static bool DrawFileList(out string file)
    {
        file = string.Empty;

        if (Scripts.Count > 0) ImGui.Separator();

        foreach (string script in Scripts)
        {
            if (ImGui.Button(Path.GetFileNameWithoutExtension(script)))
            {
                file = script;
            }
        }

        return file != string.Empty;
    }

    private static void UpdateScriptsList(List<string> scripts)
    {
        scripts.Clear();
        foreach (string script in Directory.GetFiles(Folder))
        {
            if (!script.EndsWith(".lua")) continue;
            scripts.Add(script);
        }
    }

    #region Lua Functions
    private void LuaPrint(DynValue v)
    {
        TraceLog(LOG_INFO, $"RUNSCRIPT: {v.ToPrintString()}");
    }

    private void LuaSleep(float seconds)
    {
        seconds = Math.Clamp(seconds, 0, MaxSleep);
        Thread.Sleep((int)(seconds * 1000));
    }

    private void LuaSetDelay(double delay)
    {
        //delay = Math.Clamp(delay, OptionsMenu.MinDelay, OptionsMenu.MaxDelay);
        OptionsMenu.Delay = delay;
    }

    private void LuaSetSize(int size)
    {
        OptionsMenu.ArraySize = size;
    }

    private void LuaSetRainbow(bool rainbow)
    {
        OptionsMenu.Rainbow = rainbow;
    }

    private void LuaShuffle(string name)
    {
        if (!ArrayAction.Shufflers.ContainsKey(name)) throw new RunScriptException($"Unknown shuffle {name}");
        Shuffler.RunShuffler(ArrayAction.Shufflers[name]);
    }

    private void LuaSort(string name)
    {
        if (!ArrayAction.Sorters.ContainsKey(name)) throw new RunScriptException($"Unknown sort {name}");
        Sorter.RunSorter(ArrayAction.Sorters[name]);
    }

    private void LuaVisual(string name)
    {
        if (!ArrayAction.Visualizers.ContainsKey(name)) throw new RunScriptException($"Unknown visual {name}");
        OptionsMenu.CurrentVisualizer = ArrayAction.Visualizers[name];
    }

    private void LuaVerify()
    {
        OptionsMenu.RunIndependentVerify();
    }
    #endregion

    private class RunScriptException : Exception
    {
        public RunScriptException(string err) : base(err) { }
    }
}
