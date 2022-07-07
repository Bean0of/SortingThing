using SortingThing.UI;

namespace SortingThing.Array.Actions.Visualizers;

public class Circle : Visualizer
{
    public override string Name => "Circle";

    public override void Run(SortingArray array)
    {
        const float padding = 10;

        Vector2 screenSize = new Vector2(GetScreenWidth(), GetHeightWithoutStatus());
        Vector2 center = screenSize / 2 + new Vector2(0, GetScreenHeight()*StatusBarScale);
        float width = MathF.Tau / array.Length;
        float radius = Math.Min(screenSize.X, screenSize.Y) / 2 - padding;
        int max = array.Max();

        rlCheckRenderBatchLimit(array.Length * 3);
        rlBegin(DrawMode.TRIANGLES);

        for (int i = 0; i < array.Length; i++)
        {
            Color color;

            if (!OptionsMenu.Rainbow)
            {
                color = array.GetColor(i) ??
                    Util.Lerp(OptionsMenu.ForegroundColor.ToColor(), OptionsMenu.ForegroundColor2.ToColor(),
                    array[i, false] / (float)max);
            }
            else
            {
                color = array.GetColorOrForeground(i);
            }

            // CCW
            rlColor4ub(color.r, color.g, color.b, color.a);
            rlVertex2f(center.X, center.Y);

            AddRotatedVertex(center, radius, width * i);
            AddRotatedVertex(center, radius, width * i + width);
        }

        rlEnd();
    }

    private void AddRotatedVertex(Vector2 center, float radius, float rot)
    {
        rlVertex2f(center.X + MathF.Sin(rot) * radius, center.Y + MathF.Cos(rot) * radius);
    }
}