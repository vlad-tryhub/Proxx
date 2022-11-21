using FluentAssertions;
using Xunit;

namespace Proxx.Vlad.Tryhub.Core.Tests;

public class BoardValidatorTests
{
    private readonly BoardValidator _sut;

    public BoardValidatorTests()
    {
        _sut = new BoardValidator();
    }
    
    [Theory]
    [InlineData(2)]
    [InlineData(11)]
    public void ShouldThrowExceptionIfSizeIsNotValid(int size)
    {
        _sut.Invoking(x => x.ValidateSize(size))
            .Should()
            .Throw<BoardInvalidSizeException>();
    }
    
    [Theory]
    [InlineData(10, 0)]
    [InlineData(10, 100)]
    public void ShouldThrowExceptionIfHolesCountIsNotValid(int size, int holesCount)
    {
        _sut.Invoking(x => x.ValidateHolesCount(size, holesCount))
            .Should()
            .Throw<BoardHolesCountException>();
    }
}