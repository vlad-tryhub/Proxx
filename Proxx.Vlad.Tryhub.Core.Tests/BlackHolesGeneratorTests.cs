using FluentAssertions;
using Xunit;

namespace Proxx.Vlad.Tryhub.Core.Tests;

public class BlackHolesGeneratorTests
{
    private readonly BlackHolesGenerator _sut;

    public BlackHolesGeneratorTests()
    {
        _sut = new BlackHolesGenerator();
    }
    
    [Fact]
    public void ShouldGenerateHoles()
    {
        var cells = new[,]
        {
            { new Cell { IsBlackHole = false }, new Cell { IsBlackHole = false }, new Cell { IsBlackHole = false } },
            { new Cell { IsBlackHole = false }, new Cell { IsBlackHole = false }, new Cell { IsBlackHole = false } },
            { new Cell { IsBlackHole = false }, new Cell { IsBlackHole = false }, new Cell { IsBlackHole = false } },
        };

        const int holesToGenerate = 5;

        _sut.GenerateHoles(cells, holesToGenerate);

        var generatedHoles = cells.Cast<Cell>().Count(x => x.IsBlackHole);

        generatedHoles.Should().Be(holesToGenerate);
    }
}