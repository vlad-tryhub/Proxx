namespace Proxx.Vlad.Tryhub.Core;

public interface IAdjacentHolesCalculator
{
    void CalculateAllAdjacentHolesCounts(Cell[,] cells);
    int CalculateOneAdjacentHolesCounts(Cell[,] cells, int x, int y);
}

public class AdjacentHolesCalculator : IAdjacentHolesCalculator
{
    // goes trough all cells and calculates their adjacent holes amounts
    public void CalculateAllAdjacentHolesCounts(Cell[,] cells)
    {
        for (var x = 0; x < cells.GetLength(0); x++)
        for (var y = 0; y < cells.GetLength(0); y++)
        {
            cells[x, y].AdjacentHolesCount = CalculateOneAdjacentHolesCounts(cells, x, y);
        }
    }

    // calculates adjacent holes amount for a specific sell
    // TODO: check for out of bounds
    public int CalculateOneAdjacentHolesCounts(Cell[,] cells, int x, int y)
    {
        var count = 0;

        for (var offsetX = -1; offsetX <= 1; offsetX++)
        for (var offsetY = -1; offsetY <= 1; offsetY++)
        {
            if (cells.IsOutsideBounds(offsetX + x, offsetY + y))
            {
                continue;
            }

            if (cells[offsetX + x, offsetY + y].IsBlackHole)
            {
                count++;
            }
        }

        return count;
    }
}