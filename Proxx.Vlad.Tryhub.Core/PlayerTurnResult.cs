namespace Proxx.Vlad.Tryhub.Core;

/// <summary>
///     These are the results of a player turn.
/// </summary>
public enum PlayerTurnResult
{
    // Opened an empty cell
    NoHole = 1,
    // Opened a cell with black hole. Game over
    Hole = 2,
    // Opened the last empty cell.
    Victory = 3,
}