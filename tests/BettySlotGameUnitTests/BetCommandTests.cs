using BettySlotGame.Commands;
using BettySlotGame.Exceptions;
using BettySlotGame.Models;
using BettySlotGame.Services.Abtractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace BettySlotGameUnitTests
{
    public class BetCommandTests
    {
        private Mock<IOptions<BettySlotSettings>> _bettySlotSettings;
        private Mock<IWalletService> _walletService;
        private Mock<IConsoleService> _consoleService;
        private Mock<ISlotEngine> _slotEngine;
        private Mock<ILogger<BetCommand>> _logger;

        public BetCommandTests()
        {
            _bettySlotSettings = new Mock<IOptions<BettySlotSettings>>();
            _walletService = new Mock<IWalletService>();
            _consoleService = new Mock<IConsoleService>();
            _slotEngine = new Mock<ISlotEngine>();
            _logger = new Mock<ILogger<BetCommand>>();
        }

        [Theory]
        [InlineData(-10)]
        [InlineData(0)]
        [InlineData(11)]
        public void ThrowInvalidBetPlacementExceptionIfBetIsOutOfRange(decimal bet)
        {
            _bettySlotSettings.SetReturnsDefault(new BettySlotSettings { MinBet = 1, MaxBet = 10 });

            var inputCommand = new InputCommand { Value = bet };

            var command = new BetCommand(_slotEngine.Object, _walletService.Object, _consoleService.Object, _bettySlotSettings.Object, _logger.Object);

            Assert.Throws<InvalidBetPlacementException>(() => command.Execute(inputCommand));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(8)]
        [InlineData(9)]
        [InlineData(10)]
        public void WinNormalBetPlacementIfBetIsCorrect(decimal bet)
        {
            var isWin = true;
            var winAmount = bet * 2;

            _bettySlotSettings.SetReturnsDefault(new BettySlotSettings { MinBet = 1, MaxBet = 10 });
            _slotEngine.Setup(x => x.Bet(bet)).Returns((isWin, winAmount));
            _walletService.Setup(x => x.Balance).Returns(winAmount);

            var inputCommand = new InputCommand { Value = bet };

            var command = new BetCommand(_slotEngine.Object, _walletService.Object, _consoleService.Object, _bettySlotSettings.Object, _logger.Object);

            command.Execute(inputCommand);

            _walletService.Verify(x => x.Withdraw(bet), Times.Once);
            _walletService.Verify(x => x.Deposit(winAmount), Times.Once);
            _consoleService.Verify(x => x.WriteLine(It.Is<string>(s => s.Contains($"Congrats - you won ${winAmount}! Your current balance is: ${_walletService.Object.Balance.ToString("0.##")}"))), Times.Once);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(8)]
        [InlineData(9)]
        [InlineData(10)]
        public void WinBigWinBetPlacementIfBetIsCorrect(decimal bet)
        {
            var isWin = true;
            var winAmount = bet * 8;

            _bettySlotSettings.SetReturnsDefault(new BettySlotSettings { MinBet = 1, MaxBet = 10 });
            _slotEngine.Setup(x => x.Bet(bet)).Returns((isWin, winAmount));
            _walletService.Setup(x => x.Balance).Returns(winAmount);

            var inputCommand = new InputCommand { Value = bet };

            var command = new BetCommand(_slotEngine.Object, _walletService.Object, _consoleService.Object, _bettySlotSettings.Object, _logger.Object);

            command.Execute(inputCommand);

            _walletService.Verify(x => x.Withdraw(bet), Times.Once);
            _walletService.Verify(x => x.Deposit(winAmount), Times.Once);
            _consoleService.Verify(x => x.WriteLine(It.Is<string>(s => s.Contains($"Congrats - you won ${winAmount}! Your current balance is: ${_walletService.Object.Balance.ToString("0.##")}"))), Times.Once);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(8)]
        [InlineData(9)]
        [InlineData(10)]
        public void LostBetPlacementIfBetIsCorrect(decimal bet)
        {
            var isWin = false;
            var winAmount = 0;

            _bettySlotSettings.SetReturnsDefault(new BettySlotSettings { MinBet = 1, MaxBet = 10 });
            _slotEngine.Setup(x => x.Bet(bet)).Returns((isWin, winAmount));
            _walletService.Setup(x => x.Balance).Returns(winAmount);

            var inputCommand = new InputCommand { Value = bet };

            var command = new BetCommand(_slotEngine.Object, _walletService.Object, _consoleService.Object, _bettySlotSettings.Object, _logger.Object);

            command.Execute(inputCommand);

            _walletService.Verify(x => x.Withdraw(bet), Times.Once);
            _consoleService.Verify(x => x.WriteLine(It.Is<string>(s => s.Contains($"No luck this time! Your cuurent balance is: ${_walletService.Object.Balance.ToString("0.##")}"))), Times.Once);
        }
    }
}
