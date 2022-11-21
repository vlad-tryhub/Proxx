using Microsoft.Extensions.Hosting;
using Moq;
using Proxx.Vlad.Tryhub.ConsoleDrawerUi;
using Proxx.Vlad.Tryhub.Core;
using Xunit;

namespace Proxx.Vlad.Tryhub.Tests;

public class GameRunnerTests
{
    private readonly GameRunner _sut;
    private readonly Mock<IHostApplicationLifetime> _hostApplicationLifetime;
    private readonly Mock<IBoard> _board;
    private readonly Mock<IConsoleDrawer> _consoleDrawer;

    public GameRunnerTests()
    {
        _hostApplicationLifetime = new Mock<IHostApplicationLifetime>();
        _board = new Mock<IBoard>();
        _consoleDrawer = new Mock<IConsoleDrawer>();

        _sut = new GameRunner(_hostApplicationLifetime.Object, _board.Object, _consoleDrawer.Object);
    }

    [Fact]
    public async Task ShouldCallConsoleDrawerOnBoardInvalidSizeException()
    {
        const string errorMessage = "some error message";

        _board.Setup(x => x.Reset())
            .Throws(new BoardInvalidSizeException(errorMessage));

        await _sut.StartAsync(CancellationToken.None);

        _consoleDrawer.Verify(x => x.PostMessage(errorMessage));
    }

    [Fact]
    public async Task ShouldStopApplicationOnBoardInvalidSizeException()
    {
        const string errorMessage = "some error message";

        _board.Setup(x => x.Reset())
            .Throws(new BoardInvalidSizeException(errorMessage));

        await _sut.StartAsync(CancellationToken.None);

        _hostApplicationLifetime.Verify(x => x.StopApplication());
    }

    [Fact]
    public async Task ShouldCallConsoleDrawerOnBoardHolesCountException()
    {
        const string errorMessage = "some error message";

        _board.Setup(x => x.Reset())
            .Throws(new BoardHolesCountException(errorMessage));

        await _sut.StartAsync(CancellationToken.None);

        _consoleDrawer.Verify(x => x.PostMessage(errorMessage));
    }

    [Fact]
    public async Task ShouldStopApplicationOnBoardHolesCountException()
    {
        const string errorMessage = "some error message";

        _board.Setup(x => x.Reset())
            .Throws(new BoardHolesCountException(errorMessage));

        await _sut.StartAsync(CancellationToken.None);

        _hostApplicationLifetime.Verify(x => x.StopApplication());
    }

    [Theory]
    [InlineData(PlayerTurnResult.Hole)]
    [InlineData(PlayerTurnResult.Victory)]
    public async Task ShouldStopApplicationWhenGameEnded(PlayerTurnResult playerTurnResult)
    {
        _board.Setup(x => x.OpenCell(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(playerTurnResult);

        await _sut.StartAsync(CancellationToken.None);

        _hostApplicationLifetime.Verify(x => x.StopApplication());
    }
}