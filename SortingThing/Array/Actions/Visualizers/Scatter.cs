namespace SortingThing.Array.Actions.Visualizers;

public class Scatter : Visualizer
{
    public override string Name => "Scatter Plot";

    public override void Run(SortingArray array)
    {
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
                minBarHeight
            );

            DrawRectangleRec(bar, array.GetColorOrForeground(i));
        }
    }
}
