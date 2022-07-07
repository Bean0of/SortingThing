namespace SortingThing.Array.Actions;

public abstract class Visualizer : ArrayAction
{
    public const float StatusBarScale = 0.025f;
    public const float StatusTextScale = 0.0225f;

    public float GetHeightWithoutStatus()
    {
        return GetScreenHeight() - GetScreenHeight() * StatusBarScale;
    }
}