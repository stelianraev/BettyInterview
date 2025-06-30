using BettySlotGame.Commands;
using BettySlotGame.Services;
using BettySlotGame.Services.Abtractions;
using Moq;

namespace BettySlotGameUnitTests
{
    public class CommandServiceTests
    {
        private ICommandService _commandService;
        private Mock<ICommand> _commandMock;
        public CommandServiceTests()
        {
            _commandMock = new Mock<ICommand>();
        }

        [Fact]
        public void HandleInputShouldExecuteCommandWhenValidCommandIsGiven()
        {
            // Arrange
            _commandMock.Setup(c => c.Name).Returns("bet");

            var commandService = new CommandService(new[] { _commandMock.Object });

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

            var commandService = new CommandService(new[] { _commandMock.Object });

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

            var commandService = new CommandService(new[] { _commandMock.Object });

            var input = "";

            // Act
            commandService.HandleInput(input, CancellationToken.None);

            // Assert
            _commandMock.Verify(c => c.Execute(It.IsAny<string[]>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
