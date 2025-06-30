using BettySlotGame.Services;
using BettySlotGame.Services.Abtractions;
using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using Moq;

namespace BettySlotGameUnitTests
{
    public class WalletServiceTests
    {
        private Mock<ILogger<WalletService>> _loggerMock;
        private IConsoleService _consoleService;

        public WalletServiceTests()
        {
            _loggerMock = new Mock<ILogger<WalletService>>();
            _consoleService = new ConsoleService();
        }

        [Fact]
        public void DepositShouldIncreaseBalance()
        {
            //Arange
            var walletService = new WalletService(_consoleService, _loggerMock.Object);

            decimal depositAmount = 100.0m;

            // Act
            walletService.Deposit(depositAmount);

            // Assert
            Assert.Equal(depositAmount, walletService.Balance);
        }

        [Fact]
        public void DepositShouldThrowException()
        {
            //Arange
            var walletService = new WalletService(_consoleService, _loggerMock.Object);

            // Assert
            Assert.Throws<ArgumentException>(() => walletService.Deposit(-50.0m));
        }

        [Fact]
        public void WithdrawShouldShouldDecreaseBalanceIfThereIsEnoughtBalance()
        {
            //Arange
            var walletService = new WalletService(_consoleService, _loggerMock.Object);
            walletService.Deposit(200.0m);

            // Act
            walletService.Withdraw(100.0m);
            var expectedBalance = 100.0m;
            // Assert
            Assert.Equal(expectedBalance, walletService.Balance);
        }

        [Fact]
        public void WithdrawShouldShouldDecreaseBalanceIfThereIsNotEnoughtBalance()
        {
            //Arange
            var walletService = new WalletService(_consoleService, _loggerMock.Object);
            walletService.Deposit(100.0m);

            // Assert
            Assert.Throws<InvalidOperationException>(() => walletService.Withdraw(200.0m));
        }

        [Fact]
        public void WithdrawShouldShouldDecreaseBalanceIfThereIsNegativeNumberInput()
        {
            //Arange
            var walletService = new WalletService(_consoleService, _loggerMock.Object);
            walletService.Deposit(100.0m);

            // Assert
            Assert.Throws<ArgumentException>(() => walletService.Withdraw(-10));
        }

        [Fact]
        public void ApplyGameResultShouldUpdateBalanceCorrectly()
        {
            //Arange
            var walletService = new WalletService(_consoleService, _loggerMock.Object);
            walletService.Deposit(100.0m);
            decimal betAmount = 20.0m;
            decimal winAmount = 50.0m;

            // Act
            walletService.ApplyGameResult(betAmount, winAmount);

            // Assert
            Assert.Equal(130.0m, walletService.Balance);
        }
    }
}
