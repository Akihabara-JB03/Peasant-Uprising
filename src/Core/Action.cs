namespace PeasantUprising.Core;
public class GA // Game Action
{
    public bool IsInside(int x, int y)
    {
        return 0 <= x && x < 9 && 0 <= y && y < 3;
    }
}