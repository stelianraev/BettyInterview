using BettySlotGame.Commands;
using BettySlotGame.Models;
using BettySlotGame.Services;
using BettySlotGame.Services.Abtractions;
using Moq;

namespace BettySlotGameUnitTests
{
    public class SlotGameServiceTests
    {
        private readonly Mock<ICommandRegistry> _mockCommandRegistry;
        private readonly CancellationTokenSource _cts;
        private readonly Mock<ISlotEngine> _mockSlotEngine;
        private readonly Mock<IWalletService> _mockWallet;
        private readonly Mock<IConsoleService> _consoleService;

        public SlotGameServiceTests()
        {
            _mockCommandRegistry = new Mock<ICommandRegistry>();
            _mockSlotEngine = new Mock<ISlotEngine>();
            _mockWallet = new Mock<IWalletService>();
            _consoleService = new Mock<IConsoleService>();

            _cts = new CancellationTokenSource();
        }

        [Fact]
        public async void StartGameShouldStartGameWhenCalled()
        {
            var inputQueue = new Queue<string>(
            [
                 "bet 10",
                 "exit",
             ]);

            _consoleService.Setup(x => x.ReadLine()).Returns(() => inputQueue.Count > 0 ? inputQueue.Dequeue() : null);
            _consoleService.Setup(x => x.WriteLine(It.IsAny<string>()));

            var mockBetCommand = new Mock<ISlotCommand>();
            mockBetCommand.Setup(c => c.Name).Returns("bet");
            mockBetCommand.Setup(c => c.Execute(It.IsAny<object>()));

            var mockExitCommand = new Mock<ISlotCommand>();
            mockExitCommand.Setup(c => c.Name).Returns("exit");
            mockExitCommand.Setup(c => c.Execute(It.IsAny<object>())).Callback(() => _cts.Cancel());

            var mockRegistry = new Mock<ICommandRegistry>();
            mockRegistry.Setup(r => r.GetCommand("bet")).Returns(mockBetCommand.Object);
            mockRegistry.Setup(r => r.GetCommand("exit")).Returns(mockExitCommand.Object);

            var service = new SlotGameService(null!, null!, mockRegistry.Object, _consoleService.Object, _cts);

            var task = Task.Run(() =>
            {
                service.StartGame();
            });

            await task;

            mockBetCommand.Verify(c => c.Execute(It.IsAny<InputCommand>()), Times.Once);
            mockExitCommand.Verify(c => c.Execute(It.IsAny<InputCommand>()), Times.Once);
        }
    }
}
