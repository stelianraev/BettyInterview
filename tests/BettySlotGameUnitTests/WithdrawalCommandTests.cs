using BettySlotGame.Commands;
using BettySlotGame.Models;
using BettySlotGame.Services.Abtractions;
using Microsoft.Extensions.Logging;
using Moq;

namespace BettySlotGameUnitTests
{
    public class WithdrawalCommandTests
    {
        private Mock<IWalletService> _walletService;
        private Mock<IConsoleService> _consoleService;
        private Mock<ILogger<WithdrawalCommand>> _logger;

        public WithdrawalCommandTests()
        {
            _walletService = new Mock<IWalletService>();
            _consoleService = new Mock<IConsoleService>();
            _logger = new Mock<ILogger<WithdrawalCommand>>();
        }

        [Fact]
        public void NullValueInputThrowsException()
        {
            var withdrawCommand = new WithdrawalCommand(_walletService.Object, _consoleService.Object, _logger.Object);
            var inputCommand = new InputCommand
            {
                Value = null
            };

            Assert.Throws<ArgumentException>(() => withdrawCommand.Execute(inputCommand));
        }

        [Fact]
        public void SuccessWithdraw()
        {
            var startingBalance = 100;
            var valueToWithdraw = 100;

            _walletService.Setup(x => x.Balance).Returns(startingBalance - valueToWithdraw);

            var withdrawCommand = new WithdrawalCommand(_walletService.Object, _consoleService.Object, _logger.Object);
            var inputCommand = new InputCommand
            {
                Value = valueToWithdraw
            };

            withdrawCommand.Execute(inputCommand);

            Assert.Equal(startingBalance - valueToWithdraw, _walletService.Object.Balance);
            _consoleService.Verify(x => x.WriteLine(It.Is<string>(s => s.Contains($"Your withdrawal of ${valueToWithdraw} was successful. Your current balance is: ${_walletService.Object.Balance.ToString("0.##")}"))), Times.Once);
        }
    }
}
