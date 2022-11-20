namespace Proxx.Vlad.Tryhub.Core;

public interface IBlackHolesGenerator
{
    void GenerateHoles(Cell[,] cells, int holesToGenerate);
}

/// <summary>
///     Randomly puts holes on the board
///     For more even distribution we can use some of these algorithms
///     http://extremelearning.com.au/unreasonable-effectiveness-of-quasirandom-sequences/
///     by this package https://www.extremeoptimization.com/QuickStart/CSharp/QuasiRandom.aspx
///
///     For sake of simplicity I'd like to have very straightforward solution
/// </summary>
public class BlackHolesGenerator : IBlackHolesGenerator
{
    public void GenerateHoles(Cell[,] cells, int holesToGenerate)
    {
        var generatedAmount = 0;
        while (generatedAmount < holesToGenerate)
        {
            var x = Random.Shared.Next(cells.GetLength(0));
            var y = Random.Shared.Next(cells.GetLength(0));

            if (!cells[x, y].IsBlackHole)
            {
                cells[x, y].IsBlackHole = true;
                generatedAmount++;
            }
        }
    }
}