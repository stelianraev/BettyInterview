using BettySlotGame.Commands;
using BettySlotGame.Services;
using BettySlotGame.Services.Abtractions;
using Microsoft.Extensions.Logging;
using Moq;

namespace BettySlotGameUnitTests
{
    public class CommandServiceTests
    {
        private ICommandService _commandService;
        private Mock<ICommand> _commandMock;
        private Mock<ILogger<CommandService>> _loggerMock;
        private IConsoleService _consoleService;

        public CommandServiceTests()
        {
            _commandMock = new Mock<ICommand>();
            _loggerMock = new Mock<ILogger<CommandService>>();
            _consoleService = new ConsoleService();
        }

        [Fact]
        public void HandleInputShouldExecuteCommandWhenValidCommandIsGiven()
        {
            // Arrange
            _commandMock.Setup(c => c.Name).Returns("bet");

            var commandService = new CommandService(new[] { _commandMock.Object }, _loggerMock.Object, _consoleService);

            var input = "bet 10";

            // Act
            commandService.HandleInput(input, CancellationToken.None);

            // Assert
            _commandMock.Verify(c => c.Execute(It.Is<string[]>(a => a[0] == "bet" && a[1] == "10"), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public void HandleInputShouldNotExecuteCommandWhenInValidCommandIsGiven()
        {
            // Arrange
            _commandMock.Setup(c => c.Name).Returns("bet");

            var commandService = new CommandService(new[] { _commandMock.Object }, _loggerMock.Object, _consoleService);

            var input = "test 123";

            // Act
            commandService.HandleInput(input, CancellationToken.None);

            // Assert
            _commandMock.Verify(c => c.Execute(It.Is<string[]>(a => a[0] == "test" && a[1] == "123"), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public void HandleInputShouldNotExecuteCommandWhenEmptyCommandIsGiven()
        {
            // Arrange
            _commandMock.Setup(c => c.Name).Returns("bet");

            var commandService = new CommandService(new[] { _commandMock.Object }, _loggerMock.Object, _consoleService);

            var input = "";

            // Act
            commandService.HandleInput(input, CancellationToken.None);

            // Assert
            _commandMock.Verify(c => c.Execute(It.IsAny<string[]>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
