using Proxx.Vlad.Tryhub.Core;

namespace Proxx.Vlad.Tryhub.ConsoleDrawerUi;

public interface IConsoleDrawer
{
    void DrawBoard(Cell[,] cells);
    (int x, int y) GetNextTurn(int length);
}

/// <summary>
///     This class is responsible for all
///     interactions with console
/// </summary>
public class ConsoleDrawer : IConsoleDrawer
{
    public void DrawBoard(Cell[,] cells)
    {
        var length = cells.GetLength(0);
        Console.WriteLine();
        Console.Write("  ");
        
        for (var x = 0; x < length; x++)
            Console.Write(x + " ");
        
        Console.WriteLine();
        
        for (var y = 0; y < length; y++)
        {
            Console.Write(y + " ");
            
            for (var x = 0; x < length; x++)
            {
                Console.Write(GetCellSign(cells[x, y]) + " ");
            }
    
            Console.WriteLine();
        }
    }

    private string GetCellSign(Cell cell)
    {
        return cell switch
        {
            { IsOpened: false } => "?",
            { IsBlackHole: true } => "X",
            _ => cell.AdjacentHolesCount.ToString(),
        };
    }

    public (int x, int y) GetNextTurn(int length)
    {
        var x = 0;
        var y = 0;
        var validInput = false;

        Console.WriteLine("Please enter in format 'X,Y'");
        while (!validInput)
        {
            var input = Console.ReadLine();
            validInput = TryParseInput(input, length, out x, out y);
            if (!validInput)
                Console.WriteLine("Incorrect format. Please try again.");
        }

        return (x, y);
    }

    private bool TryParseInput(string input, int length, out int x, out int y)
    {
        try
        {
            var xStr = input.Split(",")[0];
            var yStr = input.Split(",")[1];
            x = int.Parse(xStr);
            y = int.Parse(yStr);
            
            return x >= 0 && y >= 0 && x < length && y < length;
        }
        // can be improved for more detailed error messages
        catch (Exception)
        {
            x = y = -1;
            
            return false;
        }
    }
}