using BettySlotGame.Commands;
using BettySlotGame.Exceptions;
using BettySlotGame.Models;
using BettySlotGame.Services.Abtractions;
using Microsoft.Extensions.Logging;
using Moq;

namespace BettySlotGameUnitTests
{
    public class DepositCommandTests
    {
        private Mock<IWalletService> _walletService;
        private Mock<IConsoleService> _consoleService;
        private Mock<ILogger<DepositCommand>> _logger;

        public DepositCommandTests()
        {
            _walletService = new Mock<IWalletService>();
            _consoleService = new Mock<IConsoleService>();
            _logger = new Mock<ILogger<DepositCommand>>();
        }

        [Fact]
        public void NullValueInputThrowsException()
        {
            var depositCommand = new DepositCommand(_walletService.Object, _consoleService.Object, _logger.Object);
            var inputCommand = new InputCommand
            {
                Value = null
            };

            Assert.Throws<ArgumentException>(() => depositCommand.Execute(inputCommand));
        }

        [Fact]
        public void SuccessDeposit()
        {
            var valueToDepoit = 100;

            _walletService.Setup(x => x.Balance).Returns(valueToDepoit);

            var depositCommand = new DepositCommand(_walletService.Object, _consoleService.Object, _logger.Object);
            var inputCommand = new InputCommand
            {
                Value = valueToDepoit
            };

            depositCommand.Execute(inputCommand);

            Assert.Equal(valueToDepoit, _walletService.Object.Balance);
            _consoleService.Verify(x => x.WriteLine(It.Is<string>(s => s.Contains($"Your deposit of ${valueToDepoit} was successful. Your current balance is: ${_walletService.Object.Balance.ToString("0.##")}"))), Times.Once);
        }
    }
}
