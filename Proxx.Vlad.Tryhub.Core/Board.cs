using Microsoft.Extensions.Options;

namespace Proxx.Vlad.Tryhub.Core;

public interface IBoard
{
    Cell[,] Cells { get; }
    void Reset();
    PlayerTurnResult OpenCell(int x, int y);
}

public class Board : IBoard
{
    private readonly GameConfiguration _config;
    
    // these dependencies violate OOP but it helps to
    // 1. keep class relatively small
    // 2. easier to cover with tests
    // 3. replace behavior more granularity if needed (Strategies)
    private readonly IBoardValidator _validator;
    private readonly IAdjacentHolesCalculator _adjacentHolesCalculator;
    private readonly IBlackHolesGenerator _blackHolesGenerator;
    
    public Cell[,] Cells { get; private set; }

    public Board(IOptions<GameConfiguration> config,
        IBoardValidator validator,
        IAdjacentHolesCalculator adjacentHolesCalculator,
        IBlackHolesGenerator blackHolesGenerator)
    {
        _config = config.Value;
        _validator = validator;
        _adjacentHolesCalculator = adjacentHolesCalculator;
        _blackHolesGenerator = blackHolesGenerator;
    }

    // initializes new fresh board
    public void Reset()
    {
        _validator.ValidateSize(_config.Size);
        _validator.ValidateHolesCount(_config.Size, _config.HolesCount);
        Cells ??= new Cell[_config.Size, _config.Size];
        CreateDefaultCells();
        _blackHolesGenerator.GenerateHoles(Cells, _config.HolesCount);
        _adjacentHolesCalculator.CalculateAllAdjacentHolesCounts(Cells);
    }

    // a turn of player
    public PlayerTurnResult OpenCell(int x, int y)
    {
        if (Cells.IsOutsideBounds(x, y))
            throw new BoardOutsideCoordinatesException();

        if (Cells[x, y].IsOpened)
            return PlayerTurnResult.NoHole;

        if (Cells[x, y].IsBlackHole)
        {
            Cells[x, y].IsOpened = true;
            return PlayerTurnResult.Hole;
        }

        Open(x, y);

        var isVictory = Cells.Cast<Cell>().All(c => c.IsOpened || c.IsBlackHole);
        if (isVictory)
            return PlayerTurnResult.Victory;
        
        return PlayerTurnResult.NoHole;
    }
    
    // Recursively goes through all neighbour cells and opens
    private void Open(int x, int y)
    {
        if (Cells.IsOutsideBounds(x, y))
            return;
        
        if (Cells[x, y].IsOpened)
            return;
        
        Cells[x, y].IsOpened = true;

        var isHoleNear = _adjacentHolesCalculator.CalculateOneAdjacentHolesCounts(Cells, x, y) != 0;
        if (isHoleNear)
            return;
        
        for (var offsetX = -1; offsetX <= 1; offsetX++)
        for (var offsetY = -1; offsetY <= 1; offsetY++)
        {
            Open(x + offsetX, y + offsetY);
        }
    }

    // just populates our gird with cells
    private void CreateDefaultCells()
    {
        for (var x = 0; x < Cells.GetLength(0); x++)
        for (var y = 0; y < Cells.GetLength(0); y++)
        {
            Cells[x, y] = new Cell
            {
                IsBlackHole = false,
                IsOpened = false,
                AdjacentHolesCount = 0,
            };
        }
    }
}

public class BoardOutsideCoordinatesException : Exception
{
    public BoardOutsideCoordinatesException() : base("Coordinates are outside of bounds")
    {
    }
}
