namespace Proxx.Vlad.Tryhub.Core;

public class Cell
{
    public bool IsBlackHole { get; set; }
    public bool IsOpened { get; set; }

    // TODO: introduce validator for adjacent holes amount
    public int AdjacentHolesCount { get; set; }
}
