using SortingThing.UI;

namespace SortingThing.Array.Actions.Visualizers;

public class Classic : Visualizer
{
    public override string Name => "Classic";

    public override void Run(SortingArray array)
    {
        bool fancyBars = array.Length <= 256;

        float height = GetHeightWithoutStatus();
        float barWidth = GetScreenWidth() / (float)array.Length;
        float minBarHeight = height / (float)(array.Max() + 1);

        for (int i = 0; i < array.Length; i++)
        {
            int v = array[i, false];

            Rectangle bar = new Rectangle(
                barWidth * i,
                minBarHeight * (array.Length - v - 1) + GetScreenHeight() * StatusBarScale,
                barWidth,
                height * 2
            );

            DrawRectangleRec(bar, array.GetColorOrForeground(i));
            if (fancyBars) DrawRectangleLinesEx(bar, 1, OptionsMenu.BackgroundColor.ToColor());
        }
    }
}
