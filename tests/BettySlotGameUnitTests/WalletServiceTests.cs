using BettySlotGame.Exceptions;
using BettySlotGame.Models;
using BettySlotGame.Services;
using Microsoft.Extensions.Options;
using Moq;

namespace BettySlotGameUnitTests
{
    public class WalletServiceTests
    {
        private Mock<IOptions<WalletSettings>> _walletSettingsMock;       

        public WalletServiceTests()
        {
            _walletSettingsMock = new Mock<IOptions<WalletSettings>>();
            _walletSettingsMock.SetReturnsDefault(new WalletSettings { StartingBalance = 0});
        }

        [Fact]
        public void DepositShouldIncreaseBalance()
        {
            var walletService = new WalletService(_walletSettingsMock.Object);
            decimal depositAmount = 100.0m;

            walletService.Deposit(depositAmount);

            Assert.Equal(depositAmount, walletService.Balance);
        }

        [Fact]
        public void DepositShouldThrowException()
        {
            var walletService = new WalletService(_walletSettingsMock.Object);

            Assert.Throws<InvalidAmountException>(() => walletService.Deposit(-50.0m));
        }

        [Fact]
        public void WithdrawShouldShouldDecreaseBalanceIfThereIsEnoughtBalance()
        {
            var walletService = new WalletService(_walletSettingsMock.Object);
            walletService.Deposit(200.0m);

            walletService.Withdraw(100.0m);
            var expectedBalance = 100.0m;

            Assert.Equal(expectedBalance, walletService.Balance);
        }

        [Fact]
        public void WithdrawShouldShouldDecreaseBalanceIfThereIsNotEnoughtBalance()
        {
            var walletService = new WalletService(_walletSettingsMock.Object);
            walletService.Deposit(100.0m);

            Assert.Throws<InsufficientExecutionStackException>(() => walletService.Withdraw(200.0m));
        }

        [Fact]
        public void WithdrawShouldShouldDecreaseBalanceIfThereIsNegativeNumberInput()
        {
            var walletService = new WalletService(_walletSettingsMock.Object);
            walletService.Deposit(100.0m);

            Assert.Throws<InvalidAmountException>(() => walletService.Withdraw(-10));
        }
    }
}
