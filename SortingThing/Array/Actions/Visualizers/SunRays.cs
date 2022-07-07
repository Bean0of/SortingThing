using SortingThing.UI;

namespace SortingThing.Array.Actions.Visualizers;

public class SunRays : Visualizer
{
    public override string Name => "Sun Rays";

    public override void Run(SortingArray array)
    {
        const float padding = 10;

        Vector2 screenSize = new Vector2(GetScreenWidth(), GetHeightWithoutStatus());
        Vector2 center = screenSize / 2 + new Vector2(0, GetScreenHeight() * StatusBarScale);
        float width = MathF.Tau / array.Length;
        float radius = Math.Min(screenSize.X, screenSize.Y) / 2 - padding;
        float coreRadius = radius / 2;
        float minRay = (radius - coreRadius) / array.Max();
        
        rlCheckRenderBatchLimit(array.Length * 3);
        rlBegin(DrawMode.TRIANGLES);

        for (int i = 0; i < array.Length; i++)
        {
            Color color1 = array.GetColorOrFallback(i, OptionsMenu.ForegroundColor2.ToColor());
            Color color2 = array.GetColorOrForeground(i);

            float rot = width * i + width / 2;
            float rayRadius = radius - minRay * Math.Abs(array[i, false] - i);

            color1 = Util.Lerp(color2, color1, rayRadius/radius);

            // CCW
            rlColor4ub(color1.r, color1.g, color1.b, color1.a);
            AddVertex(center, rayRadius, rot);

            rlColor4ub(color2.r, color2.g, color2.b, color2.a);
            AddVertex(center, coreRadius/4, rot + MathF.Tau / 4);
            AddVertex(center, coreRadius/4, rot - MathF.Tau / 4);
        }
        
        rlEnd();

        Color middle = OptionsMenu.ForegroundColor.ToColor();
        Color outerCore = Util.Lerp(middle, OptionsMenu.ForegroundColor2.ToColor(), coreRadius/radius);

        DrawCircleGradient((int)center.X, (int)center.Y, coreRadius, middle, outerCore);
        //DrawCircleV(center, coreRadius, OptionsMenu.ForegroundColor.ToColor());
    }

    private void AddVertex(Vector2 center, float radius, float rot)
    {
        rlVertex2f(center.X + MathF.Sin(rot) * radius, center.Y + MathF.Cos(rot) * radius);
    }
}
