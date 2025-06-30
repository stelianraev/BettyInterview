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

        public SlotGameServiceTests()
        {
            _mockCommandService = new Mock<ICommandService>();
            _mockSlotEngine = new Mock<ISlotEngine>();
            _mockWallet = new Mock<IWalletService>();

            _cts = new CancellationTokenSource();
        }

        [Fact]
        public void StartGame_ShouldStartGame_WhenCalled()
        {
            var input = "bet 10";
            var game = new SlotGameService(_mockSlotEngine.Object, _mockWallet.Object, _mockCommandService.Object, _cts);
            _cts.CancelAfter(TimeSpan.FromSeconds(3));

            game.StartGame();

            _mockCommandService.Verify(x => x.HandleInput(input, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
