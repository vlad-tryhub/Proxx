namespace Proxx.Vlad.Tryhub.Core;

public static class Helpers
{
    public static bool IsOutsideBounds(this Cell[,] cells, int x, int y)
    {
        return x < 0 || y < 0 || x >= cells.GetLength(0) || y >= cells.GetLength(0);
    }
}