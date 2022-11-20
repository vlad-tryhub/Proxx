namespace Proxx.Vlad.Tryhub.Core;

public interface IBoardValidator
{
    void ValidateSize(int size);
    void ValidateHolesCount(int size, int holesCount);
}

public class BoardValidator : IBoardValidator
{
    // Let's say board size limits are strict business rules so no need to extract to the configuration
    private const int MinimalBoardSize = 3;
    private const int MaximumBoardSize = 10;

    public void ValidateSize(int size)
    {
        switch (size)
        {
            case < MinimalBoardSize:
                throw new BoardInvalidSizeException($"Board size cannot be smaller than {MinimalBoardSize}");
            case > MaximumBoardSize:
                throw new BoardInvalidSizeException($"Board size cannot be bigger than {MaximumBoardSize}");
        }
    }

    public void ValidateHolesCount(int size, int holesCount)
    {
        if (holesCount < 1)
            throw new BoardHolesCountException("There should be at least 1 hole");
        if (holesCount >= size * size)
            throw new BoardHolesCountException("There should be less holes than cells on the board");
    }
}

// When validation doesn't pass we throw exceptions
// but we could provide some kind of ValidationResult 
// to avoid try-catches and have more flexibility

public class BoardInvalidSizeException : Exception
{
    public BoardInvalidSizeException(string message) : base(message)
    {
    }
}

public class BoardHolesCountException : Exception
{
    public BoardHolesCountException(string message) : base(message)
    {
    }
}