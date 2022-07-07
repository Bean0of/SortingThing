global using System.Numerics;
global using Raylib_cs;
global using static Raylib_cs.Raylib;
global using static Raylib_cs.Rlgl;
global using static Raylib_cs.ConfigFlags;
global using static Raylib_cs.TextureFilter;
global using static Raylib_cs.KeyboardKey;
global using static Raylib_cs.TraceLogLevel;

using SortingThing.UI;
using SortingThing.Array.Actions;

namespace SortingThing;

// This program perfectly describes how messy of a person I am

public static class Program
{
    private static Font MainFont;

    public static SortingArray Array { get; } = new SortingArray(128);

    public static void Main()
    {
        SetConfigFlags(FLAG_VSYNC_HINT | FLAG_WINDOW_RESIZABLE | FLAG_MSAA_4X_HINT);
        InitWindow(1280, 720, "Sorting Algorithms Thingy");
        //SetTargetFPS(60);
        SetExitKey(KEY_NULL);

        MainFont = LoadFontEx("Resources/main.ttf", 45, null, 0);
        SetTextureFilter(MainFont.texture, TEXTURE_FILTER_BILINEAR);

        rlImGui.Setup();

        while (!WindowShouldClose())
        {
            if (OptionsMenu.Wireframe) Rlgl.rlEnableWireMode();

            BeginDrawing();
            ClearBackground(OptionsMenu.BackgroundColor.ToColor());

            DrawStatusText();

            rlImGui.Begin();
            OptionsMenu.Draw();
            rlImGui.End();

            EndDrawing();

            OptionsMenu.CurrentVisualizer.Run(Array);

            Rlgl.rlDisableWireMode();
        }

        rlImGui.Shutdown();

        UnloadFont(MainFont);
        CloseWindow();
    }

    private static void DrawStatusText()
    {
        string text = "By: Beanoof | " +
            $"FPS: {GetFPS()} | " +
            $"Sort: {OptionsMenu.CurrentSort?.Name ?? "Idle"} | " +
            $"Size: {Array.Length} | " +
          //$"Reads: {Array.Reads} | " +
          //$"Writes: {Array.Writes} | " +
            $"Swaps: {Array.Swaps} | " +
            $"Comps: {Array.Comps} | " +
          //$"Speed: {OptionsMenu.Speed}x | " +
            $"Delay: {OptionsMenu.Delay.ToString("0.000")}ms | " +
            $"Real Time: ~{OptionsMenu.EstRealTime.ToString("0.00") ?? "0.00"}ms | " +
            $"Visual Time: {OptionsMenu.TotalSortTime.ToString("0.00")}s";

        float barHeight = GetScreenHeight() * Visualizer.StatusBarScale;
        float fontSize = GetScreenHeight() * Visualizer.StatusTextScale;
        Vector2 size = MeasureTextEx(MainFont, text, fontSize, 0);

        if (size.X > GetScreenWidth())
        {
            float width = GetScreenWidth() * (1 - Visualizer.StatusTextScale);
            fontSize = fontSize * (width / size.X);
            size.X = width;
            size.Y = fontSize;
        }

        DrawRectangle(0, 0, GetScreenWidth(), (int)barHeight, OptionsMenu.BackgroundColor.ToColor());
        DrawTextEx(MainFont, text, new Vector2(GetScreenWidth() - size.X, barHeight - size.Y)/2, fontSize, 0, OptionsMenu.StatusColor.ToColor());
    }
}