using FluentAssertions;
using Xunit;

namespace Proxx.Vlad.Tryhub.Core.Tests;

public class AdjacentHolesCalculatorTests
{
    private readonly AdjacentHolesCalculator _sut;

    public AdjacentHolesCalculatorTests()
    {
        _sut = new AdjacentHolesCalculator();
    }
    
    [Fact]
    public void ShouldCalculateAdjacentHolesForOneCell()
    {
        var noHole = new Cell { IsBlackHole = false };
        var hole = new Cell { IsBlackHole = true, };
        var cells = new[,]
        {
            { noHole, hole },
            { noHole, noHole },
        };

        var result = _sut.CalculateOneAdjacentHolesCounts(cells, 0, 0);

        result.Should().Be(1);
    }
    
    [Fact]
    public void ShouldCalculateAdjacentHolesForAllCells()
    {
        var noHole1 = new Cell { IsBlackHole = false, };
        var noHole2 = new Cell { IsBlackHole = false, };
        var hole1 = new Cell { IsBlackHole = true, };
        var hole2 = new Cell { IsBlackHole = true, };
        var cells = new[,]
        {
            { noHole1, hole1 },
            { noHole2, hole2 },
        };

        _sut.CalculateAllAdjacentHolesCounts(cells);

        noHole1.AdjacentHolesCount.Should().Be(2);
        noHole2.AdjacentHolesCount.Should().Be(2);
    }
}