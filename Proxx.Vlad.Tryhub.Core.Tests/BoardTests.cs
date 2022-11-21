using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Proxx.Vlad.Tryhub.Core.Tests;

public class BoardTests
{
    private readonly Board _sut;
    private readonly Mock<IBoardValidator> _boardValidator;
    private readonly Mock<IAdjacentHolesCalculator> _adjacentHolesCalculator;
    private readonly Mock<IBlackHolesGenerator> _blackHolesGenerator;
    private readonly GameConfiguration _config;

    public BoardTests()
    {
        _boardValidator = new Mock<IBoardValidator>();
        _adjacentHolesCalculator = new Mock<IAdjacentHolesCalculator>();
        _blackHolesGenerator = new Mock<IBlackHolesGenerator>();

        _config = new GameConfiguration { Size = 3, HolesCount = 3 };
        
        _sut = new Board(
            Options.Create(_config),
            _boardValidator.Object,
            _adjacentHolesCalculator.Object,
            _blackHolesGenerator.Object);
        
        _sut.Reset();
    }
    
    [Fact]
    public void ShouldReset()
    {
        _boardValidator.Verify(x => x.ValidateSize(_config.Size));
        _boardValidator.Verify(x => x.ValidateHolesCount(_config.Size, _config.HolesCount));
        _blackHolesGenerator.Verify(x => x.GenerateHoles(It.IsAny<Cell[,]>(), It.IsAny<int>()));
        _adjacentHolesCalculator.Verify(x => x.CalculateAllAdjacentHolesCounts(It.IsAny<Cell[,]>()));
        _sut.Cells.Should().NotBeNull();
        _sut.Cells.Cast<Cell>().All(x => x != null).Should().BeTrue();
    }
    
    [Fact]
    public void ShouldThrowWhenOpenCellOutsideBounds()
    {
        _sut.Invoking(x => x.OpenCell(100, 100))
            .Should()
            .Throw<BoardOutsideCoordinatesException>();
    }
    
    [Fact]
    public void ShouldReturnNoHoleIfCellIsOpened()
    {
        _sut.Cells[1, 1].IsOpened = true;

        _sut.OpenCell(1, 1).Should().Be(PlayerTurnResult.NoHole);
    }
    
    [Fact]
    public void ShouldReturnHoleIfCellIsBlackHole()
    {
        _sut.Cells[1, 1].IsBlackHole = true;

        _sut.OpenCell(1, 1).Should().Be(PlayerTurnResult.Hole);
    }
    
    [Fact]
    public void ShouldReturnVictoryIfNoOpenedHolesLeft()
    {
        for (var x = 0; x < _config.Size; x++)
        for (var y = 0; y < _config.Size; y++)
        {
            _sut.Cells[x, y] = new Cell
            {
                IsBlackHole = false,
                IsOpened = true,
            };
        }

        _sut.Cells[0, 0].IsBlackHole = true;
        _sut.Cells[0, 1].IsOpened = false;

        _sut.OpenCell(0, 1).Should().Be(PlayerTurnResult.Victory);
    }
    
    [Fact]
    public void ShouldRecursivelyOpenAllCells()
    {
        for (var x = 0; x < _config.Size; x++)
        for (var y = 0; y < _config.Size; y++)
        {
            _sut.Cells[x, y] = new Cell
            {
                IsBlackHole = false,
                IsOpened = false,
            };
        }

        _sut.OpenCell(0, 0);

        _sut.Cells.Cast<Cell>().All(x => x.IsOpened).Should().BeTrue();
    }
}