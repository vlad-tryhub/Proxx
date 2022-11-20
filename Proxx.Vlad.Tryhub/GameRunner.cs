using Microsoft.Extensions.Hosting;
using Proxx.Vlad.Tryhub.ConsoleDrawerUi;
using Proxx.Vlad.Tryhub.Core;

namespace Proxx.Vlad.Tryhub;

public class GameRunner : BackgroundService
{
    private readonly IHostApplicationLifetime _applicationLifetime;
    private readonly IBoard _board;
    private readonly IConsoleDrawer _consoleDrawer;

    public GameRunner(IHostApplicationLifetime applicationLifetime, IBoard board, IConsoleDrawer consoleDrawer)
    {
        _applicationLifetime = applicationLifetime;
        _board = board;
        _consoleDrawer = consoleDrawer;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var started = StartBoard();
        if (!started)
            return Task.CompletedTask;

        // this is our game loop
        while (true)
        {
            _consoleDrawer.DrawBoard(_board.Cells);
            var (x, y) = _consoleDrawer.GetNextTurn(_board.Cells.GetLength(0));
            var trunResult = _board.OpenCell(x, y);

            switch (trunResult)
            {
                case PlayerTurnResult.Hole:
                    Console.WriteLine("You lost!");
                    _consoleDrawer.DrawBoard(_board.Cells);
                    _applicationLifetime.StopApplication();
                    return Task.CompletedTask;
                
                case PlayerTurnResult.Victory:
                    Console.WriteLine("Victory!");
                    _consoleDrawer.DrawBoard(_board.Cells);
                    _applicationLifetime.StopApplication();
                    return Task.CompletedTask;
            }
        }
    }

    private bool StartBoard()
    {
        try
        {
            _board.Reset();
            return true;
        }
        catch (BoardInvalidSizeException e)
        {
            _consoleDrawer.PostMessage(e.Message);
        }
        catch (BoardHolesCountException e)
        {
            _consoleDrawer.PostMessage(e.Message);
        }
        
        _applicationLifetime.StopApplication();
        return false;
    }
}