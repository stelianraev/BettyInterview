using BettySlotGame.Services;
using BettySlotGame.Services.Abtractions;
using Moq;

namespace BettySlotGameUnitTests
{
    public class SlotGameServiceTests
    {
        private readonly Mock<ICommandService> _mockCommandService;
        private readonly CancellationTokenSource _cts;
        private readonly Mock<ISlotEngine> _mockSlotEngine;
        private readonly Mock<IWalletService> _mockWallet;
        private readonly Mock<IConsoleService> _consoleService;

        public SlotGameServiceTests()
        {
            _mockCommandService = new Mock<ICommandService>();
            _mockSlotEngine = new Mock<ISlotEngine>();
            _mockWallet = new Mock<IWalletService>();
            _consoleService = new Mock<IConsoleService>();

            _cts = new CancellationTokenSource();
        }

        [Fact]
        public void StartGameShouldStartGameWhenCalled()
        {
            var input = "bet 10";
            _consoleService.Setup(x => x.ReadLine()).Returns(input);
            _consoleService.Setup(x => x.WriteLine(It.IsAny<string>()));

            var game = new SlotGameService(_mockSlotEngine.Object, _mockWallet.Object, _mockCommandService.Object, _consoleService.Object, _cts);
            _cts.CancelAfter(1);

            game.StartGame();

            _mockCommandService.Verify(x => x.HandleInput(input, It.IsAny<CancellationToken>()), Times.AtLeastOnce);
        }
    }
}
