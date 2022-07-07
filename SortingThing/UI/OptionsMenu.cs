using ImGuiNET;
using SortingThing.Array.Actions;
using SortingThing.Audio;
using SortingThing.Scripting;

namespace SortingThing.UI;

public static class OptionsMenu
{
    public const double MaxDelay = 1000;
    public const double MinDelay = 0.001;

    #region Settings
    public static bool DoVerifySweep = true;
    public static bool Rainbow = false;
    public static bool MenuSortHide = true;
    public static bool Fullscreen = false;
    public static int ArraySize = 128;
    public static int Volume = 100;
    public static double Delay = 3;
    public static Vector4 BackgroundColor = Color.BLACK.ToVector();
    public static Vector4 ForegroundColor = Color.WHITE.ToVector();
    public static Vector4 ForegroundColor2 = Color.DARKGRAY.ToVector();
    public static Vector4 StatusColor = Color.WHITE.ToVector();
    public static Vector4 VerifyCorrectColor = Color.GREEN.ToVector();
    public static Vector4 VerifyWrongColor = Color.RED.ToVector();
    public static Vector4 ReadColor = Color.RED.ToVector();
    public static Vector4 WriteColor = Color.BLUE.ToVector();
    public static Vector4 ExtraColor = Color.YELLOW.ToVector();
    public static Vector4 RainbowInfoColor = new Vector4(1, 1, 1, 0.1f);
    #endregion

    #region Debug
    public static bool Wireframe = false;
    public static bool DemoWindow = false;
    #endregion

    public static bool Visible = true;

    public static Shuffler? CurrentShuffle;
    public static Sorter? CurrentSort;
    public static Visualizer CurrentVisualizer = new Array.Actions.Visualizers.Classic();

    public static bool Shuffling => CurrentShuffle != null;
    public static bool Sorting => CurrentSort != null;
    public static bool Verifying = false;

    public static bool Running => Shuffling || Sorting || Verifying;

    public static double TotalSortTime = 0;
    public static double EstRealTime = 0;

    public static void Draw()
    {
        if (Sorting) TotalSortTime += GetFrameTime();

        if (!ImGui.IsAnyItemActive() && (IsKeyPressed(KEY_H) || IsKeyPressed(KEY_ESCAPE))) Visible = !Visible;

        if (!Visible) return;

        ImGui.SetNextWindowSizeConstraints(Vector2.Zero, new Vector2(GetScreenWidth(), GetScreenHeight()) * 0.5f);
        ImGui.Begin("Options (H to toggle)", ref Visible, ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.AlwaysAutoResize);

        if (ImGui.BeginTabBar("ConfigTabs"))
        {
            if (ImGui.BeginTabItem("Shuffles"))
            {
                AddShuffles();
                ImGui.EndTabItem();
            }

            if (ImGui.BeginTabItem("Sorts"))
            {
                AddSorts();
                ImGui.EndTabItem();
            }

            if (ImGui.BeginTabItem("Visuals"))
            {
                AddVisualizers();
                ImGui.EndTabItem();
            }

            if (ImGui.BeginTabItem("Colors"))
            {
                if (ImGui.ColorEdit4("Foreground", ref ForegroundColor)) Program.Array.FlushColors();
                if (ImGui.ColorEdit4("Secondary Foreground", ref ForegroundColor2)) Program.Array.FlushColors();
                ImGui.ColorEdit4("Background", ref BackgroundColor);
                ImGui.ColorEdit4("Text", ref StatusColor);
                ImGui.ColorEdit4("Verify Correct", ref VerifyCorrectColor);
                ImGui.ColorEdit4("Verify Wrong", ref VerifyWrongColor);
                ImGui.ColorEdit4("Read", ref ReadColor);
                ImGui.ColorEdit4("Write", ref WriteColor);
                ImGui.ColorEdit4("Extra Information", ref ExtraColor);
                ImGui.ColorEdit4("Rainbow Information", ref RainbowInfoColor);

                ImGui.Checkbox("Rainbow", ref Rainbow);

                ImGui.EndTabItem();
            }

            if (ImGui.BeginTabItem("Scripts"))
            {
                RunScript.DrawMenu();
                ImGui.EndTabItem();
            }

            if (ImGui.BeginTabItem("Misc"))
            {
                ImGui.SliderInt("Volume", ref Volume, 0, 100);  
                Oscillator.Signal.Gain = 0.2f * (Volume / 100f);

                if (!RunScript.ScriptControlled)
                {
                    InputRange("Array Size", ref ArraySize, 4, 16, SortingArray.MinSize, SortingArray.MaxSize);
                    InputRange("Delay (ms)", ref Delay, 0.001, 0.1, MinDelay, MaxDelay, "%.4g");
                }

                if (ImGui.Checkbox("Fullscreen", ref Fullscreen))
                {
                    if (IsWindowState(FLAG_WINDOW_UNDECORATED))
                    {
                        ClearWindowState(FLAG_WINDOW_UNDECORATED);
                        SetWindowSize(1280, 720);
                        SetWindowPosition(100, 100);
                    }
                    else
                    {
                        SetWindowState(FLAG_WINDOW_UNDECORATED);
                        SetWindowSize(GetMonitorWidth(0)/2, GetMonitorHeight(0)/2);
                        SetWindowPosition(0, 0);
                    }
                }

                ImGui.Checkbox("Hide Menu When Sorting", ref MenuSortHide);
                ImGui.Checkbox("Verify After Sort", ref DoVerifySweep);
                if (ImGui.Button("Verify Array")) RunIndependentVerify();

                ImGui.EndTabItem();
            }

#if DEBUG
            if (ImGui.BeginTabItem("Debug"))
            {
                ImGui.Checkbox("Wireframe", ref Wireframe);
                ImGui.Checkbox("ImGui Demo Window", ref DemoWindow);

                if (DemoWindow) ImGui.ShowDemoWindow();

                ImGui.EndTabItem();
            }
#endif

            ImGui.EndTabBar();
        }

        ImGui.End();

        if (!Running && Program.Array.Length != ArraySize)
            Program.Array.Length = ArraySize;
    }

    private static void InputRange(string label, ref double v, double step, double stepFast, double min, double max, string format)
    {
        ImGui.InputDouble(label, ref v, step, stepFast, format);
        v = Math.Clamp(v, min, max);
    }

    private static void InputRange(string label, ref int v, int step, int stepFast, int min, int max)
    {
        ImGui.InputInt(label, ref v, step, stepFast);
        v = Math.Clamp(v, min, max);
    }
    
    private static void AddShuffles()
    {
        foreach (Shuffler shuffler in ArrayAction.Shufflers.Values)
        {
            if (ImGui.Button(shuffler.Name))
            {
                if (Running) return;
                Shuffler.RunShufflerAsync(shuffler);
            }
        }
    }

    private static void AddSorts()
    {
        foreach (Sorter sorter in ArrayAction.Sorters.Values)
        {
            if (ImGui.Button(sorter.Name))
            {
                if (Running) return;

                if (MenuSortHide)
                    Visible = false;

                Sorter.RunSorterAsync(sorter);
            }
        }
    }

    internal static void RunVerify()
    {
        if (Running) return;

        Verifying = true;
        new VerifySweep().Run(Program.Array);
        Program.Array.FlushColors();
        Verifying = false;
    }

    internal static async void RunIndependentVerify()
    {
        if (Running) return;

        Verifying = true;
        Oscillator.Play();
        await Task.Run(() =>
        {
            new VerifySweep().Run(Program.Array);
            Program.Array.FlushColors();
            Oscillator.Stop();
        });
        Verifying = false;
    }

    private static void AddVisualizers()
    {
        foreach (Visualizer visualizer in ArrayAction.Visualizers.Values)
        {
            if (ImGui.Button(visualizer.Name))
            {
                CurrentVisualizer = visualizer;
            }
        }
    }
}